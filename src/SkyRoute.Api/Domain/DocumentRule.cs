namespace SkyRoute.Api.Domain;

public sealed record DocumentRule(
    DocumentType DocumentType,
    string Label,
    string ValidationMessage);
