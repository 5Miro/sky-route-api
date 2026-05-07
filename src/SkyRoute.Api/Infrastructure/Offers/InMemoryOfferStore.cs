using Microsoft.Extensions.Options;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Domain;
using SkyRoute.Api.Infrastructure.Configuration;

namespace SkyRoute.Api.Infrastructure.Offers;

public sealed class InMemoryOfferStore : IOfferStore
{
    private readonly TimeSpan _expiration;
    private readonly object _lock = new();
    private readonly Dictionary<string, StoredOffer> _offers = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryOfferStore(IOptions<OfferStoreOptions> options)
    {
        var expirationMinutes = Math.Max(1, options.Value.ExpirationMinutes);
        _expiration = TimeSpan.FromMinutes(expirationMinutes);
    }

    public void SaveMany(IEnumerable<FlightOffer> offers)
    {
        lock (_lock)
        {
            RemoveExpiredOffers();

            var now = DateTimeOffset.UtcNow;
            foreach (var offer in offers)
            {
                _offers[offer.OfferId] = new StoredOffer(offer, now.Add(_expiration));
            }
        }
    }

    public FlightOffer? Get(string offerId)
    {
        lock (_lock)
        {
            RemoveExpiredOffers();

            if (_offers.TryGetValue(offerId, out var storedOffer))
            {
                return storedOffer.Offer;
            }

            return null;
        }
    }

    private void RemoveExpiredOffers()
    {
        var now = DateTimeOffset.UtcNow;
        var expiredKeys = _offers
            .Where(pair => pair.Value.ExpiresAt <= now)
            .Select(pair => pair.Key)
            .ToArray();

        foreach (var key in expiredKeys)
        {
            _offers.Remove(key);
        }
    }

    private sealed record StoredOffer(FlightOffer Offer, DateTimeOffset ExpiresAt);
}
