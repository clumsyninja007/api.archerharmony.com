namespace api.archerharmony.com.Features.Hoelterling.CreateProject;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("person/{personId}/projects");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.Identity?.Name ?? "unknown";
        var projectId = await data.CreateProject(req.PersonId, req.En, req.De, username);

        await Send.ResponseAsync(new Response { ProjectId = projectId }, 201, ct);
    }
}
