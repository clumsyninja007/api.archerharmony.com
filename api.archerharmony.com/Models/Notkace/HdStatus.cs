namespace api.archerharmony.com.Models.Notkace;

public class HdStatus
{
    public long Id { get; set; }
    public long HdQueueId { get; set; }
    public string Name { get; set; }
    public long Ordinal { get; set; }
    public string State { get; set; }

    public virtual ICollection<HdTicket> HdTickets { get; set; }
}