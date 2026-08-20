using Hoelterling.Api.UseCases.Projects.Dtos;

namespace Hoelterling.Api.UseCases.Projects.CreateProject;

public sealed record Request
{
    public int PersonId { get; init; }
    public ProjectData En { get; init; } = null!;
    public ProjectData De { get; init; } = null!;
}
