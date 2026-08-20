namespace Hoelterling.Api.UseCases.Projects.Dtos;

public record ProjectTechnologyItem(
    string Technology,
    int DisplayOrder,
    Dictionary<string, Dictionary<string, string?>>? Localizations);
