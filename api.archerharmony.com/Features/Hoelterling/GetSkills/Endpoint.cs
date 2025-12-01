using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetSkills;

public class Endpoint(IData data) : Endpoint<Request, IEnumerable<string>>
{
    public override void Configure()
    {
        Get("person/{personId}/skills");
        Group<HoelterlingGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var language = HttpContext.Request.GetLanguage();
        var skills = await data.GetSkills(req.PersonId, language, ct);

        if (skills.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(skills, ct);
    }
}