using FastEndpoints;

namespace Notkace.Api.UseCases.Assets;

public sealed class AssetsGroup : Group
{
    public AssetsGroup()
    {
        Configure("assets", ep => { ep.AllowAnonymous(); });
    }
}
