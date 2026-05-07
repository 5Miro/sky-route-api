using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IFlightProviderClient
{
    string ProviderName { get; }

    Task<IReadOnlyCollection<ProviderFlightOffer>> SearchAsync(FlightSearchCriteria criteria, CancellationToken cancellationToken);
}
