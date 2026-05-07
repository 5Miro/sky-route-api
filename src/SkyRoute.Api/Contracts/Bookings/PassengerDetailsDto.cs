using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Api.Contracts.Bookings;

public sealed class PassengerDetailsDto
{
    [Required]
    public string FullName { get; init; } = string.Empty;

    [Required]
    public string EmailAddress { get; init; } = string.Empty;

    [Required]
    public string DocumentNumber { get; init; } = string.Empty;
}
