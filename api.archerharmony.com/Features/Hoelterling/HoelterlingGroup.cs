namespace api.archerharmony.com.Features.Hoelterling;

public sealed class HoelterlingGroup : Group
{
    public HoelterlingGroup()
    {
        Configure("hoelterling", ep =>
        {
            ep.AllowAnonymous();
        });
    }
}