using api.archerharmony.com.Models.Notkace;

namespace api.archerharmony.com.Features.Notkace.Users.GetOwners;

public class Endpoint(NotkaceContext context) : EndpointWithoutRequest<List<User>>
{
    public override void Configure()
    {
        Get("owners");
        Group<UsersGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await context.User
            .AsNoTracking()
            .Where(u => u.RoleId == 5)
            .OrderBy(u => u.FullName)
            .ToListAsync(ct);

        if (users.Count == 0)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        Response = users;
    }
}