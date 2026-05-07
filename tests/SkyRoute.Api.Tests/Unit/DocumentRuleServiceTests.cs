using SkyRoute.Api.Domain;
using SkyRoute.Api.Infrastructure.Lookups;

namespace SkyRoute.Api.Tests.Unit;

public sealed class DocumentRuleServiceTests
{
    private readonly DocumentRuleService _service = new();

    [Fact]
    public void GetRule_ForDomesticRoute_ReturnsNationalIdRule()
    {
        var rule = _service.GetRule(RouteType.Domestic);

        Assert.Equal(DocumentType.NationalId, rule.DocumentType);
        Assert.Equal("National ID", rule.Label);
    }

    [Fact]
    public void TryNormalizeAndValidate_ForDomesticRoute_RequiresDigitsOnly()
    {
        var rule = _service.GetRule(RouteType.Domestic);

        var valid = _service.TryNormalizeAndValidate(rule, "12345678", out var normalized, out _);
        var invalid = _service.TryNormalizeAndValidate(rule, "AB1234", out _, out _);

        Assert.True(valid);
        Assert.Equal("12345678", normalized);
        Assert.False(invalid);
    }

    [Fact]
    public void TryNormalizeAndValidate_ForInternationalRoute_NormalizesPassportToUppercase()
    {
        var rule = _service.GetRule(RouteType.International);

        var valid = _service.TryNormalizeAndValidate(rule, "ab123cd", out var normalized, out _);

        Assert.True(valid);
        Assert.Equal("AB123CD", normalized);
    }
}
