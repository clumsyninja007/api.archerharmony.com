using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.Projects.CreateProject;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("person/{personId}/projects");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        var projectId = await data.CreateProject(req.PersonId, req.En, req.De, username, ct);

        await Send.ResponseAsync(new Response { ProjectId = projectId }, 201, ct);
    }
}
