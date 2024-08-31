using api.archerharmony.com.Models.Notkace;

namespace api.archerharmony.com.Features.Notkace.Assets.GetAsset;

public class Endpoint(NotkaceContext context) : Endpoint<Request, Asset>
{
    public override void Configure()
    {
        Get("{id}");
        Group<AssetsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var asset = await context.Asset.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (asset == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = asset;
    }
}