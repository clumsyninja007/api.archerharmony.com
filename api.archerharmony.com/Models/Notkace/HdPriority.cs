namespace api.archerharmony.com.Models.Notkace;

public class HdPriority
{
    public long Id { get; set; }
    public long HdQueueId { get; set; }
    public string Name { get; set; }
    public long Ordinal { get; set; }
    public string Color { get; set; }
    public long? EscalationMinutes { get; set; }
    public bool? UseBusinessHoursForEscalation { get; set; }
    public bool? IsSlaEnabled { get; set; }
    public long? ResolutionDueDateMinutes { get; set; }
    public bool? UseBusinessHoursForSla { get; set; }
    public long? SlaNotificationRecurrenceMinutes { get; set; }

    public virtual ICollection<HdTicket> HdTickets { get; set; }
}