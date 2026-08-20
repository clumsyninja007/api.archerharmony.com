namespace Hoelterling.Api.UseCases.Projects.Dtos;

// Shared request-input shape for Create + Update (one language's worth of fields).
public record ProjectData
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string LongDescription { get; init; } = null!;
    public List<string> Technologies { get; init; } = [];
    public string? ImageUrl { get; init; }
    public string? LiveUrl { get; init; }
    public string? DemoUrl { get; init; }
    public string? GithubUrl { get; init; }
    public int DisplayOrder { get; init; }
}
