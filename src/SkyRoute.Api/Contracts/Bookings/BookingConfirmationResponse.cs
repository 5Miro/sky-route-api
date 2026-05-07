namespace SkyRoute.Api.Contracts.Bookings;

public sealed record BookingConfirmationResponse(
    string BookingReference,
    string OfferId,
    string Provider,
    string OriginAirportCode,
    string OriginCity,
    string DestinationAirportCode,
    string DestinationCity,
    string CabinClass,
    string CabinClassDisplayName,
    int PassengerCount,
    decimal PerPassengerPrice,
    decimal TotalPrice);
