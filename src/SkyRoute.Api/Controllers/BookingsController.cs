using Microsoft.AspNetCore.Mvc;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Contracts.Bookings;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public sealed class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookingConfirmationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingConfirmationResponse>> CreateBooking(
        [FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var confirmation = await _bookingService.CreateBookingAsync(request, cancellationToken);
        return Ok(MapResponse(confirmation));
    }

    private static BookingConfirmationResponse MapResponse(BookingConfirmation confirmation) =>
        new(
            BookingReference: confirmation.BookingReference,
            OfferId: confirmation.OfferId,
            Provider: confirmation.Provider,
            OriginAirportCode: confirmation.Origin.Code,
            OriginCity: confirmation.Origin.City,
            DestinationAirportCode: confirmation.Destination.Code,
            DestinationCity: confirmation.Destination.City,
            CabinClass: confirmation.CabinClass.ToCode(),
            CabinClassDisplayName: confirmation.CabinClass.ToDisplayName(),
            PassengerCount: confirmation.PassengerCount,
            PerPassengerPrice: confirmation.PriceBreakdown.PerPassengerPrice,
            TotalPrice: confirmation.PriceBreakdown.TotalPrice);
}
