namespace Hoelterling.Api.UseCases.GetProjects;

public sealed record Project(
    string Id,
    string Title,
    string Description,
    string LongDescription,
    IEnumerable<string> Technologies,
    string? ImageUrl,
    string? LiveUrl,
    string? DemoUrl,
    string? GithubUrl,
    int DisplayOrder);
