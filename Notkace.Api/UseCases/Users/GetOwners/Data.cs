using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data;

namespace Notkace.Api.UseCases.Users.GetOwners;

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
            .Select(u => new Response(u.FullName!))
            .ToListAsync(ct);
    }
}
