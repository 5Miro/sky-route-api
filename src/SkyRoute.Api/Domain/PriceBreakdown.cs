namespace SkyRoute.Api.Domain;

public sealed record PriceBreakdown(
    decimal PerPassengerPrice,
    int PassengerCount,
    decimal TotalPrice);
