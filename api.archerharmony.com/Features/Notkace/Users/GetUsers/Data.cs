using api.archerharmony.com.Entities.Context;

namespace api.archerharmony.com.Features.Notkace.Users.GetUsers;

public interface IData
{
    Task<List<Response>> GetUsers(CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<List<Response>> GetUsers(CancellationToken ct = default)
    {
        return context.Users
            .Where(x => x.FullName != null)
            .Select(x => new Response(x.FullName!))
            .ToListAsync(ct);
    }
}