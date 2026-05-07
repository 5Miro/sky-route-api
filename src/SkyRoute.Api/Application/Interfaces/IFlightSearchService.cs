using SkyRoute.Api.Contracts.Flights;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IFlightSearchService
{
    Task<IReadOnlyCollection<FlightOffer>> SearchAsync(FlightSearchRequest request, CancellationToken cancellationToken);
}
