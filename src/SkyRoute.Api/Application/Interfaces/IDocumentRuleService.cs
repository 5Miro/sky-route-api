using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Application.Interfaces;

public interface IDocumentRuleService
{
    DocumentRule GetRule(RouteType routeType);

    bool TryNormalizeAndValidate(DocumentRule rule, string? documentNumber, out string normalizedDocumentNumber, out string errorMessage);
}
