using Hoelterling.Api.UseCases.WorkExperience.Dtos;

namespace Hoelterling.Api.UseCases.WorkExperience.CreateWorkExperience;

public sealed record Request
{
    public int PersonId { get; init; }
    public WorkExperienceData En { get; init; } = null!;
    public WorkExperienceData De { get; init; } = null!;
}
