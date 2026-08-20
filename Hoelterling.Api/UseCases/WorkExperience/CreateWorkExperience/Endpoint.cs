using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.WorkExperience.CreateWorkExperience;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Post("person/{personId}/experience");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        var experienceId = await data.CreateWorkExperience(req.PersonId, req.En, req.De, username, ct);

        await Send.ResponseAsync(new Response { ExperienceId = experienceId }, 201, ct);
    }
}
