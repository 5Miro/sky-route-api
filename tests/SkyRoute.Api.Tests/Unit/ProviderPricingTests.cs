using SkyRoute.Api.Domain;
using SkyRoute.Api.Infrastructure.Lookups;
using SkyRoute.Api.Infrastructure.Providers;

namespace SkyRoute.Api.Tests.Unit;

public sealed class ProviderPricingTests
{
    private readonly InMemoryAirportCatalog _airportCatalog = new();

    [Fact]
    public async Task GlobalAir_AppliesFifteenPercentSurchargeAndRoundsToTwoDecimals()
    {
        var provider = new GlobalAirFlightProviderClient();
        var criteria = BuildCriteria("EZE", "GRU", new DateOnly(2026, 6, 15), 2, CabinClass.Economy);

        var offers = await provider.SearchAsync(criteria, CancellationToken.None);
        var offer = Assert.Single(offers.Take(1));

        var total = decimal.Round(offer.PriceBreakdown.PerPassengerPrice * criteria.PassengerCount, 2, MidpointRounding.AwayFromZero);
        Assert.Equal(total, offer.PriceBreakdown.TotalPrice);
        Assert.Equal(decimal.Round(offer.PriceBreakdown.PerPassengerPrice, 2), offer.PriceBreakdown.PerPassengerPrice);
    }

    [Fact]
    public async Task BudgetWings_AppliesDiscountWithMinimumPriceFloor()
    {
        var provider = new BudgetWingsFlightProviderClient();
        var criteria = BuildCriteria("AEP", "COR", new DateOnly(2026, 6, 16), 1, CabinClass.Economy);

        var offers = await provider.SearchAsync(criteria, CancellationToken.None);

        Assert.NotEmpty(offers);
        Assert.All(offers, offer => Assert.True(offer.PriceBreakdown.PerPassengerPrice >= 29.99m));
    }

    [Fact]
    public async Task BudgetWings_DoesNotReturnFirstClassResults()
    {
        var provider = new BudgetWingsFlightProviderClient();
        var criteria = BuildCriteria("EZE", "GRU", new DateOnly(2026, 6, 17), 1, CabinClass.FirstClass);

        var offers = await provider.SearchAsync(criteria, CancellationToken.None);

        Assert.Empty(offers);
    }

    private FlightSearchCriteria BuildCriteria(string originCode, string destinationCode, DateOnly departureDate, int passengerCount, CabinClass cabinClass)
    {
        var origin = _airportCatalog.FindByCode(originCode)!;
        var destination = _airportCatalog.FindByCode(destinationCode)!;
        return new FlightSearchCriteria(origin, destination, departureDate, passengerCount, cabinClass);
    }
}
