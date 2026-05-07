namespace SkyRoute.Api.Domain;

public static class RouteTypeResolver
{
    public static RouteType Resolve(Airport origin, Airport destination) =>
        string.Equals(origin.CountryCode, destination.CountryCode, StringComparison.OrdinalIgnoreCase)
            ? RouteType.Domestic
            : RouteType.International;
}
