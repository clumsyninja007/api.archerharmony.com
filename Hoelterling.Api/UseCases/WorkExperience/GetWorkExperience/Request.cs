namespace Hoelterling.Api.UseCases.WorkExperience.GetWorkExperience;

public sealed record Request
{
    public int PersonId { get; init; }
}
