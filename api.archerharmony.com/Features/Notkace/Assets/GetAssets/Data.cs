using api.archerharmony.com.Entities.Context;

namespace api.archerharmony.com.Features.Notkace.Assets.GetAssets;

public interface IData
{
    Task<List<Response>> GetAssets(ulong? id, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<List<Response>> GetAssets(ulong? id, CancellationToken ct = default)
    {
        return context.Assets
            .Where(a => id == null || a.AssetTypeId == id)
            .OrderBy(a => a.Name)
            .Select(a => new Response(a.Id, a.Name))
            .ToListAsync(ct);
    }
}