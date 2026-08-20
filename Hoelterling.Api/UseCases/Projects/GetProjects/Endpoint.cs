using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.Projects.GetProjects;

public class Endpoint(IData data) : Endpoint<Request, IEnumerable<Project>>
{
    public override void Configure()
    {
        Get("person/{personId}/projects");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var language = HttpContext.Request.GetLanguage();
        var projects = await data.GetProjects(req.PersonId, language, ct);

        if (projects.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(projects, ct);
    }
}
