using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Infrastructure.Providers;

public sealed class GlobalAirFlightProviderClient : IFlightProviderClient
{
    public string ProviderName => "GlobalAir";

    public Task<IReadOnlyCollection<ProviderFlightOffer>> SearchAsync(FlightSearchCriteria criteria, CancellationToken cancellationToken)
    {
        if (criteria.CabinClass == CabinClass.FirstClass && RouteTypeResolver.Resolve(criteria.Origin, criteria.Destination) == RouteType.Domestic)
        {
            return Task.FromResult<IReadOnlyCollection<ProviderFlightOffer>>([]);
        }

        var routeType = RouteTypeResolver.Resolve(criteria.Origin, criteria.Destination);
        var baseSeed = ComputeSeed(criteria, 100);
        var offers = BuildOffers(criteria, routeType, baseSeed);
        return Task.FromResult<IReadOnlyCollection<ProviderFlightOffer>>(offers);
    }

    private ProviderFlightOffer[] BuildOffers(FlightSearchCriteria criteria, RouteType routeType, int baseSeed)
    {
        return
        [
            CreateOffer(criteria, routeType, baseSeed, 0),
            CreateOffer(criteria, routeType, baseSeed, 1)
        ];
    }

    private ProviderFlightOffer CreateOffer(FlightSearchCriteria criteria, RouteType routeType, int baseSeed, int index)
    {
        var departureTime = BuildDeparture(criteria.DepartureDate, 6 + (baseSeed % 4) + (index * 5), (baseSeed + index * 11) % 60);
        var durationMinutes = CalculateDurationMinutes(criteria, routeType, baseSeed, index, 145, 50);
        var arrivalTime = departureTime.AddMinutes(durationMinutes);
        var baseFare = CalculateBaseFare(criteria, routeType, baseSeed, index, 145m, 42m, 95m);
        var perPassengerPrice = decimal.Round(baseFare * 1.15m, 2, MidpointRounding.AwayFromZero);
        var priceBreakdown = new PriceBreakdown(
            PerPassengerPrice: perPassengerPrice,
            PassengerCount: criteria.PassengerCount,
            TotalPrice: decimal.Round(perPassengerPrice * criteria.PassengerCount, 2, MidpointRounding.AwayFromZero));

        return new ProviderFlightOffer(
            Provider: ProviderName,
            FlightNumber: $"GA{410 + (baseSeed % 200) + (index * 7)}",
            Origin: criteria.Origin,
            Destination: criteria.Destination,
            DepartureTime: departureTime,
            ArrivalTime: arrivalTime,
            Duration: TimeSpan.FromMinutes(durationMinutes),
            CabinClass: criteria.CabinClass,
            PriceBreakdown: priceBreakdown,
            RouteType: routeType,
            DocumentType: routeType == RouteType.Domestic ? DocumentType.NationalId : DocumentType.Passport,
            DocumentRequirementLabel: routeType == RouteType.Domestic ? "National ID" : "Passport Number");
    }

    private static DateTimeOffset BuildDeparture(DateOnly departureDate, int hour, int minute) =>
        new(departureDate.Year, departureDate.Month, departureDate.Day, hour, minute, 0, TimeSpan.Zero);

    private static int CalculateDurationMinutes(FlightSearchCriteria criteria, RouteType routeType, int baseSeed, int index, int domesticBaseMinutes, int internationalStep)
    {
        var routeWeight = routeType == RouteType.Domestic ? domesticBaseMinutes : domesticBaseMinutes + 180;
        var cabinAdjustment = criteria.CabinClass switch
        {
            CabinClass.Economy => 0,
            CabinClass.Business => -10,
            CabinClass.FirstClass => -15,
            _ => 0
        };

        return routeWeight + (baseSeed % internationalStep) + (index * 35) + cabinAdjustment;
    }

    private static decimal CalculateBaseFare(FlightSearchCriteria criteria, RouteType routeType, int baseSeed, int index, decimal basePrice, decimal cabinStep, decimal internationalBonus)
    {
        var cabinMultiplier = criteria.CabinClass switch
        {
            CabinClass.Economy => 1.0m,
            CabinClass.Business => 1.45m,
            CabinClass.FirstClass => 2.05m,
            _ => 1.0m
        };

        var routeBonus = routeType == RouteType.International ? internationalBonus : 0m;
        var variation = (baseSeed % 35) + (index * 18);
        return decimal.Round((basePrice + routeBonus + variation + cabinStep) * cabinMultiplier, 2, MidpointRounding.AwayFromZero);
    }

    private static int ComputeSeed(FlightSearchCriteria criteria, int providerBias)
    {
        var routeSeed = string.Concat(criteria.Origin.Code, criteria.Destination.Code, criteria.DepartureDate.DayNumber, criteria.PassengerCount, criteria.CabinClass.ToCode());
        return Math.Abs(routeSeed.Aggregate(providerBias, (current, character) => current + character));
    }
}
