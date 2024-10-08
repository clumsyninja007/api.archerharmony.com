using api.archerharmony.com.Entities.Notkace;

namespace api.archerharmony.com.Features.Notkace.Users.GetUsers;

public class Endpoint(NotkaceContext context) : EndpointWithoutRequest<List<User>>
{
    public override void Configure()
    {
        Get("");
        Group<UsersGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await context.Users
            .AsNoTracking()
            .ToListAsync(ct);

        if (users.Count == 0)
        {
            await SendNoContentAsync(ct);
            return;
        }

        Response = users;
    }
}