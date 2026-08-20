using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.Auth.GetAuthStatus;

// No AllowAnonymous and no Roles() => any *authenticated* caller (401 without a token).
// The server is the authority on the role; the client just reads the result.
public class Endpoint : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Get("auth/me");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = new Response
        {
            IsAdmin = User.IsInRole("content-admin"),
            Username = User.GetUsername()
        };

        await Send.OkAsync(response, ct);
    }
}
