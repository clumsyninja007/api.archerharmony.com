using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.CreateWorkExperience;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("person/{personId}/experience");
        Group<HoelterlingGroup>();
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        var experienceId = await data.CreateWorkExperience(req.PersonId, req.En, req.De, username);

        await Send.ResponseAsync(new Response { ExperienceId = experienceId }, 201, ct);
    }
}
