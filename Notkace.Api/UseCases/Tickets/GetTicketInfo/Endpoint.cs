using FastEndpoints;

namespace Notkace.Api.UseCases.Tickets.GetTicketInfo;

public class Endpoint(IData data) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}/info");
        Group<TicketsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var info = await data.GetTicketInfo(req.Id, ct);

        if (info is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Response = info;
    }
}
