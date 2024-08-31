using api.archerharmony.com.Models.Notkace;

namespace api.archerharmony.com.Features.Notkace.Users.GetUser;

public class Endpoint(NotkaceContext context) : Endpoint<Request, User>
{
    public override void Configure()
    {
        Get("{id}");
        Group<UsersGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await context.User
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