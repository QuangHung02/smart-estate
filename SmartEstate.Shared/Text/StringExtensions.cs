namespace SmartEstate.Shared.Text;

public static class StringExtensions
{
    public static string? TrimToNull(this string? s)
    {
        if (s is null) return null;
        var t = s.Trim();
        return t.Length == 0 ? null : t;
    }
}
