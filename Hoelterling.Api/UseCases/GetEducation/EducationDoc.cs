namespace Hoelterling.Api.UseCases.GetEducation;

public sealed record EducationDoc(
    string School,
    string? DegreeEarned,
    string? Major,
    DateTime? StartDate,
    DateTime? EndDate,
    Dictionary<string, Dictionary<string, string?>>? Localizations);