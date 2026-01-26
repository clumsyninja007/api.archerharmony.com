using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.UpdateProject;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("person/{personId}/projects/{projectId}");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        await data.UpdateProject(req.PersonId, req.ProjectId, req.En, req.De, username);

        await Send.NoContentAsync(ct);
    }
}
