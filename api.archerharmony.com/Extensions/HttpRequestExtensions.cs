using Microsoft.AspNetCore.Http;

namespace api.archerharmony.com.Extensions;

public static class HttpRequestExtensions
{
    /// <summary>
    /// Parses the Accept-Language header and returns the best matching supported locale.
    /// Falls back to base language codes (e.g., "de" from "de-DE") and defaults to "en" if no match.
    /// </summary>
    public static string GetLanguage(this HttpRequest request)
    {
        var acceptLanguage = request.Headers.AcceptLanguage.ToString();

        if (string.IsNullOrEmpty(acceptLanguage))
            return "en";

        // Parse Accept-Language header (e.g., "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7")
        var languages = acceptLanguage
            .Split(',')
            .Select(lang => lang.Split(';')[0].Trim())
            .ToList();

        // Try to find exact locale match first (e.g., "de-DE")
        foreach (var lang in languages)
        {
            if (IsSupportedLocale(lang))
                return lang;
        }

        // Fall back to base language (e.g., "de" from "de-DE")
        foreach (var lang in languages)
        {
            var baseLanguage = lang.Split('-')[0].ToLower();
            if (IsSupportedBaseLanguage(baseLanguage))
                return baseLanguage;
        }

        // Default to English
        return "en";
    }

    private static bool IsSupportedLocale(string locale)
    {
        var supportedLocales = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "en", "en-US", "en-GB",
            "de", "de-DE", "de-AT", "de-CH"
        };

        return supportedLocales.Contains(locale);
    }

    private static bool IsSupportedBaseLanguage(string language)
    {
        return language is "en" or "de";
    }
}