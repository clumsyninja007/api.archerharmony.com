using FastEndpoints;

namespace Notkace.Api.UseCases.Users.GetUser;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}");
        Group<UsersGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await data.GetUser(req, ct);

        if (user is null)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        Response = user;
    }
}
