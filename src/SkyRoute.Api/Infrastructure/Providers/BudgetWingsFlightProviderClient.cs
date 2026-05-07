using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Infrastructure.Providers;

public sealed class BudgetWingsFlightProviderClient : IFlightProviderClient
{
    public string ProviderName => "BudgetWings";

    public Task<IReadOnlyCollection<ProviderFlightOffer>> SearchAsync(FlightSearchCriteria criteria, CancellationToken cancellationToken)
    {
        if (criteria.CabinClass == CabinClass.FirstClass)
        {
            return Task.FromResult<IReadOnlyCollection<ProviderFlightOffer>>([]);
        }

        var routeType = RouteTypeResolver.Resolve(criteria.Origin, criteria.Destination);
        var baseSeed = ComputeSeed(criteria, 250);
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
        var departureTime = BuildDeparture(criteria.DepartureDate, 7 + (baseSeed % 5) + (index * 6), (baseSeed + index * 9) % 60);
        var durationMinutes = CalculateDurationMinutes(criteria, routeType, baseSeed, index);
        var arrivalTime = departureTime.AddMinutes(durationMinutes);
        var baseFare = CalculateBaseFare(criteria, routeType, baseSeed, index);
        var discountedFare = decimal.Round(baseFare * 0.90m, 2, MidpointRounding.AwayFromZero);
        var perPassengerPrice = Math.Max(29.99m, discountedFare);
        var priceBreakdown = new PriceBreakdown(
            PerPassengerPrice: perPassengerPrice,
            PassengerCount: criteria.PassengerCount,
            TotalPrice: decimal.Round(perPassengerPrice * criteria.PassengerCount, 2, MidpointRounding.AwayFromZero));

        return new ProviderFlightOffer(
            Provider: ProviderName,
            FlightNumber: $"BW{720 + (baseSeed % 160) + (index * 13)}",
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

    private static int CalculateDurationMinutes(FlightSearchCriteria criteria, RouteType routeType, int baseSeed, int index)
    {
        var routeWeight = routeType == RouteType.Domestic ? 130 : 295;
        var cabinAdjustment = criteria.CabinClass == CabinClass.Business ? -5 : 0;
        return routeWeight + (baseSeed % 40) + (index * 28) + cabinAdjustment;
    }

    private static decimal CalculateBaseFare(FlightSearchCriteria criteria, RouteType routeType, int baseSeed, int index)
    {
        var cabinMultiplier = criteria.CabinClass switch
        {
            CabinClass.Economy => 1.0m,
            CabinClass.Business => 1.3m,
            _ => 1.0m
        };

        var routeBonus = routeType == RouteType.International ? 55m : 0m;
        var baseFare = 70m + routeBonus + (baseSeed % 32) + (index * 14);
        return decimal.Round(baseFare * cabinMultiplier, 2, MidpointRounding.AwayFromZero);
    }

    private static int ComputeSeed(FlightSearchCriteria criteria, int providerBias)
    {
        var routeSeed = string.Concat(criteria.Origin.Code, criteria.Destination.Code, criteria.DepartureDate.DayNumber, criteria.PassengerCount, criteria.CabinClass.ToCode());
        return Math.Abs(routeSeed.Aggregate(providerBias, (current, character) => current + character));
    }
}
