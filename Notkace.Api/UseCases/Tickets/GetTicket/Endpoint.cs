using FastEndpoints;

namespace Notkace.Api.UseCases.Tickets.GetTicket;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}");
        Group<TicketsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var ticket = await data.GetTicket(req.Id, ct);

        if (ticket is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Response = ticket;
    }
}
