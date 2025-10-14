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
        var personalInfo = await data.GetPersonalInfo(req.Id);

        if (personalInfo is null)
        {
            await Send.NoContentAsync(ct);
            return;
        }
        
        await Send.OkAsync(personalInfo, ct);
    }
}