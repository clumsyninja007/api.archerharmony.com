using api.archerharmony.com.Entities.Context;

namespace api.archerharmony.com.Features.Notkace.Tickets.GetTicketInfo;

public interface IData
{
    Task<Response?> GetTicketInfo(ulong id, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public partial class Data(NotkaceContext context) : IData
{
    public Task<Response?> GetTicketInfo(ulong id, CancellationToken ct = default)
    {
        return context.HdTicketChanges
            .Where(c => c.HdTicketId == id)
            .Where(c => !string.IsNullOrEmpty(c.Comment))
            .Select(c => new Response
            {
                Ticket = c.HdTicket.Id,
                Summary = c.HdTicket.Summary,
                Comment = CleanComment(c.Comment),
                Timestamp = c.Timestamp,
                Commenter = c.User!.FullName,
                OwnersOnly = c.OwnersOnly
            })
            .OrderByDescending(c => c.Timestamp)
            .FirstOrDefaultAsync(ct);
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