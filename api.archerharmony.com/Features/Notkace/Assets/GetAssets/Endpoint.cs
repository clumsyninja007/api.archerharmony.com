using api.archerharmony.com.Models.Notkace;

namespace api.archerharmony.com.Features.Notkace.Assets.GetAssets;

public class Endpoint(NotkaceContext context) : Endpoint<Request, List<Asset>>
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("", "type/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var query = context.Assets.AsNoTracking();

        if (req.Id is not null)
        {
            query = query.Where(x => x.AssetTypeId == req.Id)
                .OrderBy(x => x.Name);
        }
        
        var assets = await query.ToListAsync(ct);

        if (assets.Count == 0)
        {
            await SendNoContentAsync(ct);
        }

        Response = assets;
    }
}