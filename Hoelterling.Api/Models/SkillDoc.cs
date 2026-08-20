namespace Hoelterling.Api.Models;

public record SkillDoc(
    string Label,
    Dictionary<string, Dictionary<string, string?>>? Localizations);