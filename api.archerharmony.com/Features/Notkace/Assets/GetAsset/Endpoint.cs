namespace api.archerharmony.com.Features.Notkace.Assets.GetAsset;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}");
        Group<AssetsGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var asset = await data.GetAsset(req.Id, ct);

        if (asset == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = asset;
    }
}