namespace Hoelterling.Api.Helpers;

public static class LocalizationHelper
{
    public static string Localize(
        string baseValue,
        Dictionary<string, Dictionary<string, string?>>? localizations,
        string language,
        string field)
    {
        var lang = language.Split('-')[0]; // "de-DE" -> "de"
        if (localizations is not null
            && localizations.TryGetValue(lang, out var fields)
            && fields.TryGetValue(field, out var value)
            && value is not null)
        {
            return value;
        }
        return baseValue; // base fields are the English/default
    }
}