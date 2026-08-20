using FastEndpoints;

namespace Notkace.Api.UseCases.Users;

public sealed class UsersGroup : Group
{
    public UsersGroup()
    {
        Configure("users", ep => { ep.AllowAnonymous(); });
    }
}
