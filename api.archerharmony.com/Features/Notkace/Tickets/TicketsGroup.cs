namespace api.archerharmony.com.Features.Notkace.Tickets;

public sealed class TicketsGroup : Group
{
    public TicketsGroup()
    {
        Configure("hdTickets", ep => { });
    }
}