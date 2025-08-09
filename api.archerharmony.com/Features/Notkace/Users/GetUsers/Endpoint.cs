namespace api.archerharmony.com.Features.Notkace.Users.GetUsers;

public class Endpoint(IData data) : EndpointWithoutRequest<List<Response>>
{
    public override void Configure()
    {
        Get("");
        Group<UsersGroup>();
        AllowAnonymous();
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