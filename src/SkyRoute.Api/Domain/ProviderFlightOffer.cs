namespace SkyRoute.Api.Domain;

public sealed record ProviderFlightOffer(
    string Provider,
    string FlightNumber,
    Airport Origin,
    Airport Destination,
    DateTimeOffset DepartureTime,
    DateTimeOffset ArrivalTime,
    TimeSpan Duration,
    CabinClass CabinClass,
    PriceBreakdown PriceBreakdown,
    RouteType RouteType,
    DocumentType DocumentType,
    string DocumentRequirementLabel);
