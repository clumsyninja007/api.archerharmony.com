using FastEndpoints;

namespace Notkace.Api.UseCases.Assets.GetAssets;

public class Endpoint(IData data) : Endpoint<Request, List<Response>>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("", "type/{id}");
        Group<AssetsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        Response = await data.GetAssets(req.Id, ct);

        if (Response.Count == 0)
        {
            await Send.NoContentAsync(ct);
        }
    }
}
