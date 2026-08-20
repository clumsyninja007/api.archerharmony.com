namespace Hoelterling.Api.UseCases.Projects.DeleteProject;

public sealed record Request
{
    public int PersonId { get; init; }
    public string ProjectId { get; init; } = null!;
}
