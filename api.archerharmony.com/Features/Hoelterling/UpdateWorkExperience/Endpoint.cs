using FastEndpoints;

namespace api.archerharmony.com.Features.Hoelterling.UpdateWorkExperience;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("person/{personId}/experience/{experienceId}");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.Identity?.Name ?? "unknown";
        await data.UpdateWorkExperience(req.ExperienceId, req.En, req.De, username);
        await Send.NoContentAsync(ct);
    }
}