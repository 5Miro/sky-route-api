namespace SkyRoute.Api.Domain;

public static class CabinClassParser
{
    public static bool TryParse(string? value, out CabinClass cabinClass)
    {
        cabinClass = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = value.Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim()
            .ToLowerInvariant();

        cabinClass = normalized switch
        {
            "economy" => CabinClass.Economy,
            "business" => CabinClass.Business,
            "firstclass" => CabinClass.FirstClass,
            _ => default
        };

        return normalized is "economy" or "business" or "firstclass";
    }

    public static string ToCode(this CabinClass cabinClass) =>
        cabinClass switch
        {
            CabinClass.Economy => "Economy",
            CabinClass.Business => "Business",
            CabinClass.FirstClass => "FirstClass",
            _ => throw new ArgumentOutOfRangeException(nameof(cabinClass))
        };

    public static string ToDisplayName(this CabinClass cabinClass) =>
        cabinClass switch
        {
            CabinClass.Economy => "Economy",
            CabinClass.Business => "Business",
            CabinClass.FirstClass => "First Class",
            _ => throw new ArgumentOutOfRangeException(nameof(cabinClass))
        };
}
