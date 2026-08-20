namespace Hoelterling.Api.UseCases.GetWorkExperience;

public record WorkExperienceSkillItem(
    string Skill,
    int DisplayOrder,
    Dictionary<string, Dictionary<string, string?>>? Localizations);