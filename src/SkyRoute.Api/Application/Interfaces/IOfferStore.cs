using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IOfferStore
{
    void SaveMany(IEnumerable<FlightOffer> offers);

    FlightOffer? Get(string offerId);
}
