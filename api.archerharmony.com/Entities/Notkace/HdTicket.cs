using System;
using System.Collections.Generic;

namespace api.archerharmony.com.Entities.Notkace;

public partial class HdTicket
{
    public ulong Id { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public ulong? HdPriorityId { get; set; }

    public ulong? HdImpactId { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime Created { get; set; }

    public ulong? OwnerId { get; set; }

    public ulong? SubmitterId { get; set; }

    public ulong? HdStatusId { get; set; }

    public ulong HdQueueId { get; set; }

    public ulong? HdCategoryId { get; set; }

    public string CcList { get; set; } = null!;

    public DateTime? Escalated { get; set; }

    public string? CustomFieldValue0 { get; set; }

    public string? CustomFieldValue1 { get; set; }

    public string? CustomFieldValue2 { get; set; }

    public string? CustomFieldValue3 { get; set; }

    public string? CustomFieldValue4 { get; set; }

    public string? CustomFieldValue5 { get; set; }

    public string? CustomFieldValue6 { get; set; }

    public string? CustomFieldValue7 { get; set; }

    public string? CustomFieldValue8 { get; set; }

    public string? CustomFieldValue9 { get; set; }

    public string? CustomFieldValue10 { get; set; }

    public string? CustomFieldValue11 { get; set; }

    public string? CustomFieldValue12 { get; set; }

    public string? CustomFieldValue13 { get; set; }

    public string? CustomFieldValue14 { get; set; }

    public string? CustomFieldValue15 { get; set; }

    public DateTime? DueDate { get; set; }

    public bool? IsManualDueDate { get; set; }

    public DateTime? SlaNotified { get; set; }

    public DateTime? TimeOpened { get; set; }

    public DateTime? TimeClosed { get; set; }

    public DateTime? TimeStalled { get; set; }

    public ulong? MachineId { get; set; }

    public int? SatisfactionRating { get; set; }

    public string? SatisfactionComment { get; set; }

    public string? Resolution { get; set; }

    public ulong? AssetId { get; set; }

    public ulong? ParentId { get; set; }

    public bool? IsParent { get; set; }

    public ulong? ApproverId { get; set; }

    public string? ApproveState { get; set; }

    public string? Approval { get; set; }

    public string? ApprovalNote { get; set; }

    public ulong? ServiceTicketId { get; set; }

    public ulong? HdServiceStatusId { get; set; }

    public sbyte? HdUseProcessStatus { get; set; }

    public virtual Asset? Asset { get; set; }

    public virtual HdPriority? HdPriority { get; set; }

    public virtual HdStatus? HdStatus { get; set; }

    public virtual ICollection<HdTicketChange> HdTicketChanges { get; set; } = new List<HdTicketChange>();

    public virtual Asset? Machine { get; set; }

    public virtual User? Owner { get; set; }

    public virtual User? Submitter { get; set; }
}
