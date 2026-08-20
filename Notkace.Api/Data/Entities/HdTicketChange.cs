namespace Notkace.Api.Data.Entities;

public class HdTicketChange
{
    public long Id { get; set; }
    public long HdTicketId { get; set; }
    public DateTime Timestamp { get; set; }
    public long? UserId { get; set; }
    public string? Comment { get; set; }
    public bool OwnersOnly { get; set; }

    public HdTicket HdTicket { get; set; } = null!;
    public User? User { get; set; }
}
