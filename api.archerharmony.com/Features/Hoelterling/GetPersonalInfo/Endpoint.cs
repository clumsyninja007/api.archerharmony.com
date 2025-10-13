namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/hoelterling/personal/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var personalInfo = await data.GetPersonalInfo(request.Id);

        if (personalInfo is null)
        {
            await Send.NoContentAsync(ct);
            return;
        }
        
        await Send.OkAsync(personalInfo, ct);
    }
}