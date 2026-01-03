namespace api.archerharmony.com.Features.Hoelterling.DeleteWorkExperience;

public record Request
{
    public int PersonId { get; init; }
    public int ExperienceId { get; init; }
}