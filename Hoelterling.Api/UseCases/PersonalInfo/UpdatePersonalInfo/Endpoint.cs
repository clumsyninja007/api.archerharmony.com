using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.PersonalInfo.UpdatePersonalInfo;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("person/{personId}");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        await data.UpdatePersonalInfo(req.PersonId, req.En, req.De, username, ct);

        await Send.NoContentAsync(ct);
    }
}
