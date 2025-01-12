using api.archerharmony.com.Entities.Context;
using api.archerharmony.com.Entities.Entities.Notkace;

namespace api.archerharmony.com.Features.Notkace.Assets.GetAsset;

public class Endpoint(NotkaceContext context) : Endpoint<Request, Asset>
{
    public override void Configure()
    {
        Get("{id}");
        Group<AssetsGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var asset = await context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (asset == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = asset;
    }
}