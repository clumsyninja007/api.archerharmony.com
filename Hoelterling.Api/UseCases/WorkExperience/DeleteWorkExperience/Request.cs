namespace Hoelterling.Api.UseCases.WorkExperience.DeleteWorkExperience;

public sealed record Request
{
    public int PersonId { get; init; }
    public string ExperienceId { get; init; } = null!;
}
