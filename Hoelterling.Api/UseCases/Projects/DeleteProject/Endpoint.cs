using FastEndpoints;

namespace Hoelterling.Api.UseCases.Projects.DeleteProject;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("person/{personId}/projects/{projectId}");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await data.DeleteProject(req.ProjectId, ct);

        await Send.NoContentAsync(ct);
    }
}
