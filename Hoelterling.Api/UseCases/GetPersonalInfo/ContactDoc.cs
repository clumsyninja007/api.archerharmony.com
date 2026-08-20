namespace Hoelterling.Api.UseCases.GetPersonalInfo;

public record ContactDoc(
    string Label,
    string? Link,
    string Icon,
    Dictionary<string, Dictionary<string, string?>>? Localizations);
