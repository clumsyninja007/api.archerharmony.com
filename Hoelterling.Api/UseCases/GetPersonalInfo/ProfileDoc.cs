namespace Hoelterling.Api.UseCases.GetPersonalInfo;

public record ProfileDoc(
    string Name,
    string Title,
    string? HeroDescription,
    Dictionary<string, Dictionary<string, string?>>? Localizations);
