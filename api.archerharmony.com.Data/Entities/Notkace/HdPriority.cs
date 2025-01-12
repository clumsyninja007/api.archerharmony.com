namespace api.archerharmony.com.Entities.Entities.Notkace;

public partial class HdPriority
{
    public ulong Id { get; set; }

    public ulong HdQueueId { get; set; }

    public string Name { get; set; } = null!;

    public ulong Ordinal { get; set; }

    public string Color { get; set; } = null!;

    public ulong? EscalationMinutes { get; set; }

    public byte? UseBusinessHoursForEscalation { get; set; }

    public bool? IsSlaEnabled { get; set; }

    public ulong? ResolutionDueDateMinutes { get; set; }

    public byte? UseBusinessHoursForSla { get; set; }

    public ulong? SlaNotificationRecurrenceMinutes { get; set; }

    public virtual ICollection<HdTicket> HdTickets { get; set; } = new List<HdTicket>();
}
