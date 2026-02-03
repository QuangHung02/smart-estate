using System.Text;
using System.Text.RegularExpressions;

namespace SmartEstate.Shared.Text;

public static class Slugify
{
    private static readonly Regex NonSlug = new(@"[^a-z0-9\-]+", RegexOptions.Compiled);

    public static string ToSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "";
        var s = input.Trim().ToLowerInvariant();

        s = RemoveDiacritics(s);
        s = s.Replace(" ", "-");
        s = NonSlug.Replace(s, "");
        s = Regex.Replace(s, "-{2,}", "-").Trim('-');

        return s;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
