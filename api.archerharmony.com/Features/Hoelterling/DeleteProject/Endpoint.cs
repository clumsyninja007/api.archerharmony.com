namespace api.archerharmony.com.Features.Hoelterling.DeleteProject;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("person/{personId}/projects/{projectId}");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await data.DeleteProject(req.PersonId, req.ProjectId);

        await Send.NoContentAsync(ct);
    }
}
