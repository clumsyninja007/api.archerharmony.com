namespace Hoelterling.Api.UseCases.Languages.GetLanguages;

public sealed record Request
{
    public int PersonId { get; init; }
}
