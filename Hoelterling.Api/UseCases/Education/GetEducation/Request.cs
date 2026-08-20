namespace Hoelterling.Api.UseCases.Education.GetEducation;

public sealed record Request
{
    public int PersonId { get; init; }
}
