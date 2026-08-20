namespace Hoelterling.Api.UseCases.GetWorkExperience;

public sealed record Request
{
    public int PersonId { get; init; }
}