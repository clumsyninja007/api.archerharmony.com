namespace Notkace.Api.Data.Entities;

public class HdTicket
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public long HdQueueId { get; set; }
    public DateTime Created { get; set; }

    public long? HdPriorityId { get; set; }
    public long? HdStatusId { get; set; }
    public long? OwnerId { get; set; }
    public long? SubmitterId { get; set; }
    public long? AssetId { get; set; }

    // KACE custom fields surfaced by the ticket list: Dept / Location / ReferredTo.
    public string? CustomFieldValue1 { get; set; }
    public string? CustomFieldValue2 { get; set; }
    public string? CustomFieldValue5 { get; set; }

    public HdPriority? HdPriority { get; set; }
    public HdStatus? HdStatus { get; set; }
    public User? Owner { get; set; }
    public User? Submitter { get; set; }
    public Asset? Asset { get; set; }
}
