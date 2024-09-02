using api.archerharmony.com.Entities.Notkace;

namespace api.archerharmony.com.Features.Notkace.Users.GetUser;

public class Endpoint(NotkaceContext context) : Endpoint<Request, User>
{
    public override void Configure()
    {
        Get("{id}");
        Group<UsersGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (user == null)
        {
            await SendNoContentAsync(ct);
            return;
        }

        Response = user;
    }
}