using FastEndpoints;

namespace Hoelterling.Api.UseCases.WorkExperience.DeleteWorkExperience;

public class Endpoint(IData data) : Endpoint<Request>
{
    public override void Configure()
    {
        Delete("person/{personId}/experience/{experienceId}");
        Roles("content-admin");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await data.DeleteWorkExperience(req.ExperienceId, ct);

        await Send.NoContentAsync(ct);
    }
}
