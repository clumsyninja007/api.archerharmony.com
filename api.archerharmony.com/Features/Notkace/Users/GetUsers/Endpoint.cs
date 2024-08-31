using api.archerharmony.com.Models.Notkace;

namespace api.archerharmony.com.Features.Notkace.Users.GetUsers;

public class Endpoint(NotkaceContext context) : EndpointWithoutRequest<List<User>>
{
    public override void Configure()
    {
        Get("");
        Group<UsersGroup>();
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