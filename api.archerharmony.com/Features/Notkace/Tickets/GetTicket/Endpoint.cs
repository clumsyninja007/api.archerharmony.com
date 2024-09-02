namespace api.archerharmony.com.Features.Notkace.Tickets.GetTicket;

public class Endpoint(NotkaceContext context) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}");
        Group<TicketsGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var hdTicket = await context.HdTickets
            .Where(t => t.Id == req.Id)
            .Select(t => new Response
            {
                Ticket = t.Id,
                Title = t.Title,
                Priority = t.HdPriority.Name,
                Owner = t.Owner.FullName,
                Submitter = t.Submitter.FullName,
                Asset = t.Asset.Name,
                Status = t.HdStatus.Name,
                ReferredTo = t.CustomFieldValue5,
                UserName = t.Owner.UserName,
                Dept = t.CustomFieldValue1,
                Location = t.CustomFieldValue2,
                PriOrd = t.HdPriority.Ordinal,
                StatOrd = t.HdStatus.Ordinal,
                Created = t.Created
            })
            .FirstOrDefaultAsync(ct);

        if (hdTicket is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = hdTicket;
    }
}