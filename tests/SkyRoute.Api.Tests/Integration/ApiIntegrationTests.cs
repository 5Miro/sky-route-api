using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SkyRoute.Api.Contracts.Bookings;
using SkyRoute.Api.Contracts.Flights;
using SkyRoute.Api.Contracts.Lookups;

namespace SkyRoute.Api.Tests.Integration;

public sealed class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAirports_ReturnsSupportedAirportLookupData()
    {
        var response = await _client.GetAsync("/api/lookups/airports");

        response.EnsureSuccessStatusCode();
        var airports = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<AirportLookupResponse>>();

        Assert.NotNull(airports);
        Assert.True(airports.Count >= 6);
        Assert.Contains(airports, airport => airport.Code == "EZE");
        Assert.Contains(airports, airport => airport.Code == "GRU");
    }

    [Fact]
    public async Task SearchFlights_ReturnsOffersForSupportedSearch()
    {
        var request = new FlightSearchRequest
        {
            OriginAirportCode = "EZE",
            DestinationAirportCode = "GRU",
            DepartureDate = new DateOnly(2026, 6, 20),
            PassengerCount = 2,
            CabinClass = "Economy"
        };

        var response = await _client.PostAsJsonAsync("/api/flights/search", request);

        response.EnsureSuccessStatusCode();
        var offers = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<FlightOfferResponse>>();

        Assert.NotNull(offers);
        Assert.NotEmpty(offers);
        Assert.All(offers, offer =>
        {
            Assert.Equal(2, offer.PassengerCount);
            Assert.True(offer.TotalPrice > offer.PerPassengerPrice);
            Assert.Equal("Passport Number", offer.DocumentRequirementLabel);
        });
    }

    [Fact]
    public async Task SearchFlights_ReturnsEmptyListWhenNoFlightsMatch()
    {
        var request = new FlightSearchRequest
        {
            OriginAirportCode = "AEP",
            DestinationAirportCode = "COR",
            DepartureDate = new DateOnly(2026, 6, 21),
            PassengerCount = 1,
            CabinClass = "FirstClass"
        };

        var response = await _client.PostAsJsonAsync("/api/flights/search", request);

        response.EnsureSuccessStatusCode();
        var offers = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<FlightOfferResponse>>();

        Assert.NotNull(offers);
        Assert.Empty(offers);
    }

    [Fact]
    public async Task CreateBooking_ReturnsBookingReferenceForValidOffer()
    {
        var searchRequest = new FlightSearchRequest
        {
            OriginAirportCode = "EZE",
            DestinationAirportCode = "GRU",
            DepartureDate = new DateOnly(2026, 6, 22),
            PassengerCount = 2,
            CabinClass = "Business"
        };

        var searchResponse = await _client.PostAsJsonAsync("/api/flights/search", searchRequest);
        searchResponse.EnsureSuccessStatusCode();

        var offers = await searchResponse.Content.ReadFromJsonAsync<IReadOnlyCollection<FlightOfferResponse>>();
        var selectedOffer = Assert.Single(offers!.Take(1));

        var bookingRequest = new CreateBookingRequest
        {
            OfferId = selectedOffer.OfferId,
            PassengerDetails = new PassengerDetailsDto
            {
                FullName = "Taylor Martinez",
                EmailAddress = "taylor@example.com",
                DocumentNumber = "ab123cd"
            }
        };

        var bookingResponse = await _client.PostAsJsonAsync("/api/bookings", bookingRequest);

        bookingResponse.EnsureSuccessStatusCode();
        var confirmation = await bookingResponse.Content.ReadFromJsonAsync<BookingConfirmationResponse>();

        Assert.NotNull(confirmation);
        Assert.StartsWith("SR-", confirmation.BookingReference);
        Assert.Equal(selectedOffer.OfferId, confirmation.OfferId);
        Assert.Equal(selectedOffer.TotalPrice, confirmation.TotalPrice);
    }

    [Fact]
    public async Task CreateBooking_ReturnsValidationProblemForInvalidDocument()
    {
        var searchRequest = new FlightSearchRequest
        {
            OriginAirportCode = "EZE",
            DestinationAirportCode = "GRU",
            DepartureDate = new DateOnly(2026, 6, 23),
            PassengerCount = 1,
            CabinClass = "Economy"
        };

        var searchResponse = await _client.PostAsJsonAsync("/api/flights/search", searchRequest);
        searchResponse.EnsureSuccessStatusCode();

        var offers = await searchResponse.Content.ReadFromJsonAsync<IReadOnlyCollection<FlightOfferResponse>>();
        var selectedOffer = Assert.Single(offers!.Take(1));

        var bookingRequest = new CreateBookingRequest
        {
            OfferId = selectedOffer.OfferId,
            PassengerDetails = new PassengerDetailsDto
            {
                FullName = "Taylor Martinez",
                EmailAddress = "taylor@example.com",
                DocumentNumber = "12-34"
            }
        };

        var bookingResponse = await _client.PostAsJsonAsync("/api/bookings", bookingRequest);

        Assert.Equal(HttpStatusCode.BadRequest, bookingResponse.StatusCode);
        var content = await bookingResponse.Content.ReadAsStringAsync();
        Assert.Contains("DocumentNumber", content, StringComparison.OrdinalIgnoreCase);
    }
}
