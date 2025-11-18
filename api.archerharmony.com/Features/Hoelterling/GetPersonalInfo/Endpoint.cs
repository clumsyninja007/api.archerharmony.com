namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("person/{id}");
        Group<HoelterlingGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var personalInfoTask = data.GetPersonalInfo(req.Id, ct);
        var contactInfoTask = data.GetContactInfo(req.Id, ct);
        
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
            ContactInfo = contactInfo.Count > 0 ? contactInfo : null
        };
        
        await Send.OkAsync(response, ct);
    }
}