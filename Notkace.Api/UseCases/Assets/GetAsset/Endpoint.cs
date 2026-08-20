using FastEndpoints;

namespace Notkace.Api.UseCases.Assets.GetAsset;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}");
        Group<AssetsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var asset = await data.GetAsset(req.Id, ct);

        if (asset == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Response = asset;
    }
}
