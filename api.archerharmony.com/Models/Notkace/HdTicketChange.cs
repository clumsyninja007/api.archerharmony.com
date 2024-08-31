using System;

namespace api.archerharmony.com.Models.Notkace;

public class HdTicketChange
{
    public long Id { get; set; }
    public long HdTicketId { get; set; }
    public DateTime Timestamp { get; set; }
    public long? UserId { get; set; }
    public string? Comment { get; set; }
    public string? CommentLoc { get; set; }
    public string? Description { get; set; }
    public string? OwnersOnlyDescription { get; set; }
    public string? LocalizedDescription { get; set; }
    public string? LocalizedOwnersOnlyDescription { get; set; }
    public bool? Mailed { get; set; }
    public DateTime? MailedTimestamp { get; set; }
    public int? MailerSession { get; set; }
    public string? NotifyUsers { get; set; }
    public string? ViaEmail { get; set; }
    public bool OwnersOnly { get; set; }
    public bool? ResolutionChanged { get; set; }
    public bool? SystemComment { get; set; }
    public bool? TicketDataChange { get; set; }

    public virtual HdTicket HdTicket { get; set; }
    public virtual User User { get; set; }
}