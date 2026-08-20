namespace Hoelterling.Api.UseCases.GetEducation;

public sealed record Request
{
    public int PersonId { get; init; }
}