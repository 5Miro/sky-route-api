using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IAirportCatalog
{
    IReadOnlyCollection<Airport> GetAll();

    Airport? FindByCode(string code);
}
