namespace Hoelterling.Api.UseCases.Projects.CreateProject;

public sealed record Response
{
    public required string ProjectId { get; init; }
}
