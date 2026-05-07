namespace SkyRoute.Api.Infrastructure.Configuration;

public sealed class OfferStoreOptions
{
    public const string SectionName = "OfferStore";

    public int ExpirationMinutes { get; init; } = 30;
}
