namespace api.archerharmony.com.Features.Hoelterling.DeleteProject;

public record Request
{
    public int PersonId { get; init; }
    public int ProjectId { get; init; }
}
