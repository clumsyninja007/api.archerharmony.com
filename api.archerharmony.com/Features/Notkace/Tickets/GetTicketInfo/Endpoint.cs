using System;

namespace api.archerharmony.com.Features.Notkace.Tickets.GetTicketInfo;

public partial class Endpoint(NotkaceContext context) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("{id}/info");
        Group<TicketsGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var info = await context.HdTicketChanges
            .Where(c => c.Id == req.Id)
            .Where(c => !string.IsNullOrEmpty(c.Comment))
            .Where(c => c.Comment != "")
            .Select(c => new Response
            {
                Ticket = c.HdTicket.Id,
                Summary = c.HdTicket.Summary,
                Comment = CleanComment(c.Comment),
                Timestamp = c.Timestamp,
                Commenter = c.User.FullName,
                OwnersOnly = c.OwnersOnly
            })
            .OrderByDescending(c => c.Timestamp)
            .FirstOrDefaultAsync(ct);

        if (info is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        Response = info;
    }
    
    private static string? CleanComment(string? comment)
    {
        if (string.IsNullOrEmpty(comment))
        {
            return comment;
        }
        
        comment = comment.Replace("/packages/hd_attachments", new Uri("http://cty-k1k/packages/hd_attachments").ToString());
        comment = MyRegex().Replace(comment, "");
        return comment;
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"<p>\s*</p>")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
}