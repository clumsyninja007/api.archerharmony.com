using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.WorkExperience.GetWorkExperience;

public class Endpoint(IData data) : Endpoint<Request, IEnumerable<WorkExperience>>
{
    public override void Configure()
    {
        Get("person/{personId}/experience");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var language = HttpContext.Request.GetLanguage();
        var experience = await data.GetWorkExperiences(req.PersonId, language, ct);

        if (experience.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(experience, ct);
    }
}
