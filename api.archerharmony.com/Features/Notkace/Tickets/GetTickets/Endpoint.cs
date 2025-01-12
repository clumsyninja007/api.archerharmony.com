namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("");
        Group<TicketsGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        Response = await data.GetTickets(req, ct);

        if (Response.Count == 0)
        {
            await SendNoContentAsync(ct);
        }
    }
}