namespace api.archerharmony.com.Features.Hoelterling.GetEducation;

public class Endpoint(IData data) : Endpoint<Request, IEnumerable<EducationRecord>>
{
    public override void Configure()
    {
        Get("person/{personId}/education");
        Group<HoelterlingGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var education = await data.GetEducation(req.PersonId, ct);

        if (education.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }
        
        await Send.OkAsync(education, ct);
    }
}