using Microsoft.AspNetCore.Mvc;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Contracts.Lookups;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Controllers;

[ApiController]
[Route("api/lookups")]
public sealed class LookupsController : ControllerBase
{
    private readonly IAirportCatalog _airportCatalog;

    public LookupsController(IAirportCatalog airportCatalog)
    {
        _airportCatalog = airportCatalog;
    }

    [HttpGet("airports")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AirportLookupResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<AirportLookupResponse>> GetAirports()
    {
        var response = _airportCatalog.GetAll()
            .OrderBy(airport => airport.CountryName)
            .ThenBy(airport => airport.City)
            .Select(airport => new AirportLookupResponse(
                airport.Code,
                airport.Name,
                airport.City,
                airport.CountryCode,
                airport.CountryName))
            .ToArray();

        return Ok(response);
    }

    [HttpGet("cabin-classes")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CabinClassLookupResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<CabinClassLookupResponse>> GetCabinClasses()
    {
        var response = Enum.GetValues<CabinClass>()
            .Select(cabinClass => new CabinClassLookupResponse(cabinClass.ToCode(), cabinClass.ToDisplayName()))
            .ToArray();

        return Ok(response);
    }
}
