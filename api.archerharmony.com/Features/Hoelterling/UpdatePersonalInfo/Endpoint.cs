using FastEndpoints;

namespace api.archerharmony.com.Features.Hoelterling.UpdatePersonalInfo;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("person/{personId}");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.Identity?.Name ?? "unknown";
        await data.UpdatePersonalInfo(req.PersonId, req.En, req.De, username, ct);
        await Send.NoContentAsync(ct);
    }
}