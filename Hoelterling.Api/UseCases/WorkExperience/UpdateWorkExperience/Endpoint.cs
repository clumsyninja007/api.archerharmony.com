using FastEndpoints;
using Hoelterling.Api.Extensions;

namespace Hoelterling.Api.UseCases.WorkExperience.UpdateWorkExperience;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Put("person/{personId}/experience/{experienceId}");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var username = User.GetUsername();
        await data.UpdateWorkExperience(req.ExperienceId, req.PersonId, req.En, req.De, username, ct);

        await Send.NoContentAsync(ct);
    }
}
