namespace Hoelterling.Api.UseCases.GetLanguages;

public sealed record Request
{
    public int PersonId { get; init; }
}