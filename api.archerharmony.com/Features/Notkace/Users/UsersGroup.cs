namespace api.archerharmony.com.Features.Notkace.Users;

public sealed class UsersGroup : Group
{
    public UsersGroup()
    {
        Configure("users", ep => { });
    }
}