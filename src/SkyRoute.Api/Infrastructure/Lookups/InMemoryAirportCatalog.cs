using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Infrastructure.Lookups;

public sealed class InMemoryAirportCatalog : IAirportCatalog
{
    private static readonly Airport[] Airports =
    [
        new("EZE", "Ministro Pistarini International Airport", "Buenos Aires", "AR", "Argentina"),
        new("AEP", "Jorge Newbery Airfield", "Buenos Aires", "AR", "Argentina"),
        new("COR", "Ingeniero Aeronautico Ambrosio L.V. Taravella International Airport", "Cordoba", "AR", "Argentina"),
        new("GRU", "Sao Paulo/Guarulhos International Airport", "Sao Paulo", "BR", "Brazil"),
        new("GIG", "Rio de Janeiro/Galeao International Airport", "Rio de Janeiro", "BR", "Brazil"),
        new("JFK", "John F. Kennedy International Airport", "New York", "US", "United States"),
        new("MIA", "Miami International Airport", "Miami", "US", "United States")
    ];

    public IReadOnlyCollection<Airport> GetAll() => Airports;

    public Airport? FindByCode(string code) =>
        Airports.FirstOrDefault(airport => string.Equals(airport.Code, code.Trim(), StringComparison.OrdinalIgnoreCase));
}
