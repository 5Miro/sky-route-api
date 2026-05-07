namespace SkyRoute.Api.Contracts.Flights;

public sealed record FlightOfferResponse(
    string OfferId,
    string Provider,
    string FlightNumber,
    string OriginAirportCode,
    string OriginCity,
    string OriginCountryName,
    string DestinationAirportCode,
    string DestinationCity,
    string DestinationCountryName,
    DateTimeOffset DepartureTime,
    DateTimeOffset ArrivalTime,
    int DurationMinutes,
    string CabinClass,
    string CabinClassDisplayName,
    decimal PerPassengerPrice,
    decimal TotalPrice,
    int PassengerCount,
    string RouteType,
    string DocumentRequirementLabel);
