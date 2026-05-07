using System.Text.RegularExpressions;
using SkyRoute.Api.Application.Interfaces;
using SkyRoute.Api.Domain;

namespace SkyRoute.Api.Infrastructure.Lookups;

public sealed class DocumentRuleService : IDocumentRuleService
{
    private static readonly Regex NationalIdRegex = new(@"^\d{6,12}$", RegexOptions.Compiled);
    private static readonly Regex PassportRegex = new(@"^[A-Z0-9]{6,12}$", RegexOptions.Compiled);

    public DocumentRule GetRule(RouteType routeType) =>
        routeType switch
        {
            RouteType.Domestic => new DocumentRule(DocumentType.NationalId, "National ID", "National ID must contain 6 to 12 digits."),
            RouteType.International => new DocumentRule(DocumentType.Passport, "Passport Number", "Passport Number must contain 6 to 12 uppercase letters or digits."),
            _ => throw new ArgumentOutOfRangeException(nameof(routeType))
        };

    public bool TryNormalizeAndValidate(DocumentRule rule, string? documentNumber, out string normalizedDocumentNumber, out string errorMessage)
    {
        normalizedDocumentNumber = string.Empty;
        errorMessage = rule.ValidationMessage;

        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            return false;
        }

        normalizedDocumentNumber = documentNumber.Trim().ToUpperInvariant();
        return rule.DocumentType switch
        {
            DocumentType.NationalId => NationalIdRegex.IsMatch(normalizedDocumentNumber),
            DocumentType.Passport => PassportRegex.IsMatch(normalizedDocumentNumber),
            _ => false
        };
    }
}
