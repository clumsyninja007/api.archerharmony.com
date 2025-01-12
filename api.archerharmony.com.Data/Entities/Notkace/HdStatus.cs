namespace api.archerharmony.com.Entities.Entities.Notkace;

public partial class HdStatus
{
    public ulong Id { get; set; }

    public ulong HdQueueId { get; set; }

    public string Name { get; set; } = null!;

    public ulong Ordinal { get; set; }

    public string State { get; set; } = null!;

    public virtual ICollection<HdTicket> HdTickets { get; set; } = new List<HdTicket>();
}
