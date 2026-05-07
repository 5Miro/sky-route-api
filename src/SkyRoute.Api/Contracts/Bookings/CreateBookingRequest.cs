using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Api.Contracts.Bookings;

public sealed class CreateBookingRequest
{
    [Required]
    public string OfferId { get; init; } = string.Empty;

    [Required]
    public PassengerDetailsDto? PassengerDetails { get; init; }
}
