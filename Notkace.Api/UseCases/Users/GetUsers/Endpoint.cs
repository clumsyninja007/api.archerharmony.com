using FastEndpoints;

namespace Notkace.Api.UseCases.Users.GetUsers;

public class Endpoint(IData data) : EndpointWithoutRequest<List<Response>>
{
    public override void Configure()
    {
        Get("");
        Group<UsersGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await data.GetUsers(ct);

        if (Response.Count == 0)
        {
            await Send.NoContentAsync(ct);
        }
    }
}
