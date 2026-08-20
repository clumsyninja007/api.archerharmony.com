using FastEndpoints;

namespace Notkace.Api.UseCases.Tickets.GetTickets;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("");
        Group<TicketsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        Response = await data.GetTickets(req, ct);

        if (Response.Count == 0)
        {
            await Send.NoContentAsync(ct);
        }
    }
}
