using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data;

namespace Notkace.Api.UseCases.Users.GetUser;

public interface IData
{
    Task<Response?> GetUser(Request req, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<Response?> GetUser(Request req, CancellationToken ct = default)
    {
        return context.Users
            .Where(x => x.Id == req.Id)
            .Select(x => new Response(x.FullName!))
            .FirstOrDefaultAsync(ct);
    }
}
