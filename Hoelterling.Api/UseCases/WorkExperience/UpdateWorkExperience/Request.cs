using Hoelterling.Api.UseCases.WorkExperience.Dtos;

namespace Hoelterling.Api.UseCases.WorkExperience.UpdateWorkExperience;

public sealed record Request
{
    public int PersonId { get; init; }
    public string ExperienceId { get; init; } = null!;
    public WorkExperienceData En { get; init; } = null!;
    public WorkExperienceData De { get; init; } = null!;
}
