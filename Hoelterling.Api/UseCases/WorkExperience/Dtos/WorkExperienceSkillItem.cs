namespace Hoelterling.Api.UseCases.WorkExperience.Dtos;

public record WorkExperienceSkillItem(
    string Skill,
    int DisplayOrder,
    Dictionary<string, Dictionary<string, string?>>? Localizations);
