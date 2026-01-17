using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetLanguages;

public class Endpoint(IData data) : Endpoint<Request, IEnumerable<string>>
{
    public override void Configure()
    {
        Get("person/{personId}/languages");
        Group<HoelterlingGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var language = HttpContext.Request.GetLanguage();
        var languages = await data.GetLanguages(req.PersonId, language, ct);

        if (languages.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(languages, ct);
    }
}