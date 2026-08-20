namespace Hoelterling.Api.UseCases.GetProjects;

public record ProjectTechnologyItem(
    string Technology,
    int DisplayOrder,
    Dictionary<string, Dictionary<string, string?>>? Localizations);
