namespace api.archerharmony.com.Features.Notkace.Users.GetUser;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}");
        Group<UsersGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await data.GetUser(req, ct);

        if (user is null)
        {
            await SendNoContentAsync(ct);
            return;
        }

        Response = user;
    }
}