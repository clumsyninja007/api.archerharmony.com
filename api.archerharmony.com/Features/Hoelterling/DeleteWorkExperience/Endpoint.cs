namespace api.archerharmony.com.Features.Hoelterling.DeleteWorkExperience;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("person/{personId}/experience/{experienceId}");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await data.DeleteWorkExperience(req.ExperienceId);
        await Send.NoContentAsync(ct);
    }
}