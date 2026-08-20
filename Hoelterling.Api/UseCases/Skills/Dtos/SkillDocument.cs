namespace Hoelterling.Api.UseCases.Skills.Dtos;

public record SkillDocument(
    string Id,
    string Type,
    int PersonId,
    string Label,
    Dictionary<string, Dictionary<string, string?>>? Localizations)
{
    public const string PartitionKeyValue = "skill";
}
