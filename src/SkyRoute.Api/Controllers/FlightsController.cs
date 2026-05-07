using Microsoft.AspNetCore.Mvc;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Contracts.Flights;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Controllers;

[ApiController]
[Route("api/flights")]
public sealed class FlightsController : ControllerBase
{
    private readonly IFlightSearchService _flightSearchService;

    public FlightsController(IFlightSearchService flightSearchService)
    {
        _flightSearchService = flightSearchService;
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FlightOfferResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyCollection<FlightOfferResponse>>> SearchFlights(
        [FromBody] FlightSearchRequest request,
        CancellationToken cancellationToken)
    {
        var offers = await _flightSearchService.SearchAsync(request, cancellationToken);

        var response = offers.Select(MapResponse).ToArray();
        return Ok(response);
    }

    private static FlightOfferResponse MapResponse(FlightOffer offer) =>
        new(
            OfferId: offer.OfferId,
            Provider: offer.Provider,
            FlightNumber: offer.FlightNumber,
            OriginAirportCode: offer.Origin.Code,
            OriginCity: offer.Origin.City,
            OriginCountryName: offer.Origin.CountryName,
            DestinationAirportCode: offer.Destination.Code,
            DestinationCity: offer.Destination.City,
            DestinationCountryName: offer.Destination.CountryName,
            DepartureTime: offer.DepartureTime,
            ArrivalTime: offer.ArrivalTime,
            DurationMinutes: (int)offer.Duration.TotalMinutes,
            CabinClass: offer.CabinClass.ToCode(),
            CabinClassDisplayName: offer.CabinClass.ToDisplayName(),
            PerPassengerPrice: offer.PriceBreakdown.PerPassengerPrice,
            TotalPrice: offer.PriceBreakdown.TotalPrice,
            PassengerCount: offer.PriceBreakdown.PassengerCount,
            RouteType: offer.RouteType.ToString(),
            DocumentRequirementLabel: offer.DocumentRequirementLabel);
}
