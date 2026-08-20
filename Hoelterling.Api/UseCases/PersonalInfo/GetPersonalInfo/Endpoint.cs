using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.PersonalInfo.GetPersonalInfo;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("person/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var language = HttpContext.Request.GetLanguage();

        // Fire both single-partition reads concurrently, then assemble.
        var personalInfoTask = data.GetPersonalInfo(req.Id, language, ct);
        var contactInfoTask = data.GetContactInfo(req.Id, language, ct);

        var personalInfo = await personalInfoTask;
        if (personalInfo is null)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        var contactInfo = await contactInfoTask;

        var response = new Response
        {
            Name = personalInfo.Name,
            Title = personalInfo.Title,
            HeroDescription = personalInfo.HeroDescription,
            ContactInfo = contactInfo.Count > 0 ? contactInfo : null
        };

        await Send.OkAsync(response, ct);
    }
}
