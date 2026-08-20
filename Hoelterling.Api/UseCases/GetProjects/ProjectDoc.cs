namespace Hoelterling.Api.UseCases.GetProjects;

public record ProjectDoc(
    string Id,
    string Title,
    string Description,
    string LongDescription,
    string? ImageUrl,
    string? LiveUrl,
    string? DemoUrl,
    string? GithubUrl,
    int DisplayOrder,
    List<ProjectTechnologyItem> Technologies,
    Dictionary<string, Dictionary<string, string?>>? Localizations);
