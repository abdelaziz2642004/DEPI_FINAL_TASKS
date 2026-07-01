namespace BookStore.Extensions;

public static class StringExtensions
{
    public static bool IsBlank(this string? s) => string.IsNullOrWhiteSpace(s);

    public static decimal ToDecimal(this string s, decimal fallback = 0m)
        => decimal.TryParse(s, out var val) ? val : fallback;

    public static int ToInt(this string s, int fallback = 0)
        => int.TryParse(s, out var val) ? val : fallback;

    public static double ToDouble(this string s, double fallback = 0)
        => double.TryParse(s, out var val) ? val : fallback;
}

public static class DecimalExtensions
{
    public static string ToPrice(this decimal d) => $"${d:F2}";

    public static decimal Discounted(this decimal price, int percent)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentOutOfRangeException(nameof(percent), "Discount must be between 0 and 100.");
        return Math.Round(price * (1 - percent / 100m), 2);
    }
}
