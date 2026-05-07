using SkyRoute.Api.Application.Exceptions;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Contracts.Flights;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Services;

public sealed class FlightSearchService : IFlightSearchService
{
    private static readonly string[] SupportedCabinClasses = ["Economy", "Business", "FirstClass"];
    private readonly IAirportCatalog _airportCatalog;
    private readonly IDocumentRuleService _documentRuleService;
    private readonly IEnumerable<IFlightProviderClient> _flightProviderClients;
    private readonly IOfferStore _offerStore;

    public FlightSearchService(
        IAirportCatalog airportCatalog,
        IDocumentRuleService documentRuleService,
        IEnumerable<IFlightProviderClient> flightProviderClients,
        IOfferStore offerStore)
    {
        _airportCatalog = airportCatalog;
        _documentRuleService = documentRuleService;
        _flightProviderClients = flightProviderClients;
        _offerStore = offerStore;
    }

    public async Task<IReadOnlyCollection<FlightOffer>> SearchAsync(FlightSearchRequest request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

        var origin = ResolveAirport(request.OriginAirportCode, nameof(request.OriginAirportCode), errors);
        var destination = ResolveAirport(request.DestinationAirportCode, nameof(request.DestinationAirportCode), errors);

        if (origin is not null &&
            destination is not null &&
            string.Equals(origin.Code, destination.Code, StringComparison.OrdinalIgnoreCase))
        {
            errors[nameof(request.DestinationAirportCode)] = ["Destination airport must be different from origin airport."];
        }

        if (request.PassengerCount is < 1 or > 9)
        {
            errors[nameof(request.PassengerCount)] = ["Passenger count must be between 1 and 9."];
        }

        if (!CabinClassParser.TryParse(request.CabinClass, out var cabinClass))
        {
            errors[nameof(request.CabinClass)] = [$"Cabin class must be one of: {string.Join(", ", SupportedCabinClasses)}."];
        }

        if (request.DepartureDate == default)
        {
            errors[nameof(request.DepartureDate)] = ["Departure date is required."];
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }

        var criteria = new FlightSearchCriteria(origin!, destination!, request.DepartureDate, request.PassengerCount, cabinClass);
        var providerResults = await Task.WhenAll(_flightProviderClients.Select(client => client.SearchAsync(criteria, cancellationToken)));

        var offers = providerResults
            .SelectMany(result => result)
            .Select(result =>
            {
                var routeType = RouteTypeResolver.Resolve(result.Origin, result.Destination);
                var rule = _documentRuleService.GetRule(routeType);

                return new FlightOffer(
                    OfferId: Guid.NewGuid().ToString("N"),
                    Provider: result.Provider,
                    FlightNumber: result.FlightNumber,
                    Origin: result.Origin,
                    Destination: result.Destination,
                    DepartureTime: result.DepartureTime,
                    ArrivalTime: result.ArrivalTime,
                    Duration: result.Duration,
                    CabinClass: result.CabinClass,
                    PriceBreakdown: result.PriceBreakdown,
                    RouteType: routeType,
                    DocumentType: rule.DocumentType,
                    DocumentRequirementLabel: rule.Label);
            })
            .OrderBy(offer => offer.DepartureTime)
            .ToArray();

        _offerStore.SaveMany(offers);
        return offers;
    }

    private Airport? ResolveAirport(string? code, string fieldName, IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            errors[fieldName] = ["Airport code is required."];
            return null;
        }

        var airport = _airportCatalog.FindByCode(code);
        if (airport is null)
        {
            errors[fieldName] = ["Airport code is not supported by SkyRoute."];
        }

        return airport;
    }
}
