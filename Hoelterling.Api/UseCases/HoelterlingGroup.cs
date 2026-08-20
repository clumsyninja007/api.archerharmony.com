using FastEndpoints;

namespace Hoelterling.Api.UseCases;

public sealed class HoelterlingGroup : Group
{
    public HoelterlingGroup()
    {
        Configure("", ep =>
        {
            ep.AllowAnonymous();
        });
    }
}