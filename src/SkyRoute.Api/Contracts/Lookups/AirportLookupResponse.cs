namespace SkyRoute.Api.Contracts.Lookups;

public sealed record AirportLookupResponse(
    string Code,
    string Name,
    string City,
    string CountryCode,
    string CountryName);
