namespace Hoelterling.Api.UseCases.GetSkills;

public record Request
{
    public int PersonId { get; init; }
}