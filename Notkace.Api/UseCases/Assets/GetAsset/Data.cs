using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data;

namespace Notkace.Api.UseCases.Assets.GetAsset;

public interface IData
{
    Task<Response?> GetAsset(long id, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<Response?> GetAsset(long id, CancellationToken ct = default)
    {
        return context.Assets
            .Where(x => x.Id == id)
            .Select(x => new Response(x.Id, x.Name))
            .FirstOrDefaultAsync(ct);
    }
}
