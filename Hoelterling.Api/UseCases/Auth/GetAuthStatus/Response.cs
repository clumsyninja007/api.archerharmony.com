namespace Hoelterling.Api.UseCases.Auth.GetAuthStatus;

public sealed record Response
{
    public required bool IsAdmin { get; init; }
    public required string Username { get; init; }
}
