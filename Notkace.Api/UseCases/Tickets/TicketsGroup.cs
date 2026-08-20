using FastEndpoints;

namespace Notkace.Api.UseCases.Tickets;

public sealed class TicketsGroup : Group
{
    public TicketsGroup()
    {
        Configure("hdTickets", ep => { ep.AllowAnonymous(); });
    }
}
