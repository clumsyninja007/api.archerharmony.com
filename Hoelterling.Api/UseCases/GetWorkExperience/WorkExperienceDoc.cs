namespace Hoelterling.Api.UseCases.GetWorkExperience;

public record WorkExperienceDoc(
    string Id,
    string Title,
    string Company,
    string Location,
    DateTime StartDate,
    DateTime? EndDate,
    List<WorkExperienceSkillItem> Skills,
    Dictionary<string, Dictionary<string, string?>>? Localizations);