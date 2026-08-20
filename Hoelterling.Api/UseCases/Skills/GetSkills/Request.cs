namespace Hoelterling.Api.UseCases.Skills.GetSkills;

public sealed record Request
{
    public int PersonId { get; init; }
}
