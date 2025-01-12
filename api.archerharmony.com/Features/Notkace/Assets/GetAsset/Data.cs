using api.archerharmony.com.Entities.Context;

namespace api.archerharmony.com.Features.Notkace.Assets.GetAsset;

public interface IData
{
    Task<Response?> GetAsset(ulong id, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<Response?> GetAsset(ulong id, CancellationToken ct = default)
    {
        return context.Assets
            .Where(x => x.Id == id)
            .Select(x => new Response(x.Id, x.Name))
            .FirstOrDefaultAsync(ct);
    }
}