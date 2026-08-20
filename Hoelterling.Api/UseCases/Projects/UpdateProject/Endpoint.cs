using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.Projects.UpdateProject;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("person/{personId}/projects/{projectId}");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        await data.UpdateProject(req.ProjectId, req.PersonId, req.En, req.De, username, ct);

        await Send.NoContentAsync(ct);
    }
}
