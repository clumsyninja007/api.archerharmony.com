namespace Hoelterling.Api.UseCases.GetProjects;

public sealed record Request
{
    public int PersonId { get; init; }
}
