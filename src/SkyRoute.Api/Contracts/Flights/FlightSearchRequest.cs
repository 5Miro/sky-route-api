using System.ComponentModel.DataAnnotations;

namespace SkyRoute.Api.Contracts.Flights;

public sealed class FlightSearchRequest
{
    [Required]
    public string OriginAirportCode { get; init; } = string.Empty;

    [Required]
    public string DestinationAirportCode { get; init; } = string.Empty;

    [Required]
    public DateOnly DepartureDate { get; init; }

    [Range(1, 9)]
    public int PassengerCount { get; init; }

    [Required]
    public string CabinClass { get; init; } = string.Empty;
}
