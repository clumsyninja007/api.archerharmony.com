using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data;

namespace Notkace.Api.UseCases.Assets.GetAssets;

public interface IData
{
    Task<List<Response>> GetAssets(long? id, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<List<Response>> GetAssets(long? id, CancellationToken ct = default)
    {
        return context.Assets
            .Where(a => id == null || a.AssetTypeId == id)
            .OrderBy(a => a.Name)
            .Select(a => new Response(a.Id, a.Name))
            .ToListAsync(ct);
    }
}
