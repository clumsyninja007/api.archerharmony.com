namespace Hoelterling.Api.UseCases.GetSkills;

public sealed record SkillDoc(
    string Label,
    Dictionary<string, Dictionary<string, string?>>? Localizations);