using Hoelterling.Api.UseCases.Projects.Dtos;

namespace Hoelterling.Api.UseCases.Projects.UpdateProject;

public sealed record Request
{
    public int PersonId { get; init; }
    public string ProjectId { get; init; } = null!;
    public ProjectData En { get; init; } = null!;
    public ProjectData De { get; init; } = null!;
}
