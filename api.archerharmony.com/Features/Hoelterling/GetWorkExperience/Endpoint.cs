using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetWorkExperience;

public class Endpoint(IData data) : Endpoint<Request, IEnumerable<WorkExperience>>
{
    public override void Configure()
    {
        Get("person/{personId}/experience");
        Group<HoelterlingGroup>();
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