namespace api.archerharmony.com.Features.Hoelterling.GetProjects;

public record Project(
    int Id,
    string Title,
    string Description,
    string LongDescription,
    IEnumerable<string> Technologies,
    string? ImageUrl,
    string? LiveUrl,
    string? DemoUrl,
    string? GithubUrl,
    int DisplayOrder);