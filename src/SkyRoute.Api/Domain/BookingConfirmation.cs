namespace SkyRoute.Api.Domain;

public sealed record BookingConfirmation(
    string BookingReference,
    string OfferId,
    string Provider,
    Airport Origin,
    Airport Destination,
    CabinClass CabinClass,
    int PassengerCount,
    PriceBreakdown PriceBreakdown);
