using FastEndpoints;

namespace Notkace.Api.UseCases.Users.GetOwners;

public class Endpoint(IData data) : EndpointWithoutRequest<List<Response>>
{
    public override void Configure()
    {
        Get("owners");
        Group<UsersGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await data.GetOwners(ct);

        if (Response.Count == 0)
        {
            await Send.NotFoundAsync(ct);
        }
    }
}
