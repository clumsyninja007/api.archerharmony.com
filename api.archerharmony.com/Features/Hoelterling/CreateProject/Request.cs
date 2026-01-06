namespace api.archerharmony.com.Features.Hoelterling.CreateProject;

public record Request
{
    public int PersonId { get; init; }
    public ProjectData En { get; init; } = null!;
    public ProjectData De { get; init; } = null!;
}

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
