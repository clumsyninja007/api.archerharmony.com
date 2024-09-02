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
                Priority = t.Priority.Name,
                Owner = t.Owner.FullName,
                Submitter = t.Submitter.FullName,
                Asset = t.Asset.Name,
                Status = t.Status.Name,
                ReferredTo = t.CustomFieldValue5,
                UserName = t.Owner.UserName,
                Dept = t.CustomFieldValue1,
                Location = t.CustomFieldValue2,
                PriOrd = t.Priority.Ordinal,
                StatOrd = t.Status.Ordinal,
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