using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data;

namespace Notkace.Api.UseCases.Users.GetUsers;

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
