namespace Hoelterling.Api.UseCases.Education.Dtos;

public record EducationDocument(
    string Id,
    string Type,
    int PersonId,
    string School,
    string? DegreeEarned,
    string? Major,
    DateTime? StartDate,
    DateTime? EndDate,
    Dictionary<string, Dictionary<string, string?>>? Localizations)
{
    public const string PartitionKeyValue = "education";
}
