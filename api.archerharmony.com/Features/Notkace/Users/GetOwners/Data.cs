using api.archerharmony.com.Entities.Context;

namespace api.archerharmony.com.Features.Notkace.Users.GetOwners;

public interface IData
{
    Task<List<Response>> GetOwners(CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<List<Response>> GetOwners(CancellationToken ct = default)
    {
        return context.Users
            .Where(u => u.RoleId == 5)
            .Where(u => u.FullName != null)
            .OrderBy(u => u.FullName)
            .Select(u => new Response(u.FullName))
            .ToListAsync(ct);
    }
}