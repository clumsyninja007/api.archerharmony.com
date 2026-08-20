namespace Hoelterling.Api.UseCases.Languages.Dtos;

public record LanguageDocument(
    string Id,
    string Type,
    int PersonId,
    string Language,
    string ProficiencyLevel,
    int DisplayOrder,
    bool IsActive,
    Dictionary<string, Dictionary<string, string?>>? Localizations)
{
    public const string PartitionKeyValue = "personLanguage";
}
