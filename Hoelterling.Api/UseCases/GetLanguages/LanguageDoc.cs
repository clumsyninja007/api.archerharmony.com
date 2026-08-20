namespace Hoelterling.Api.UseCases.GetLanguages;

public sealed record LanguageDoc(
    string Language,
    string ProficiencyLevel,
    int DisplayOrder,
    Dictionary<string, Dictionary<string, string?>>? Localizations);