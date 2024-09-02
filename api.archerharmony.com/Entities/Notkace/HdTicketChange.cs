using System;
using System.Collections.Generic;

namespace api.archerharmony.com.Entities.Notkace;

public partial class HdTicketChange
{
    public ulong Id { get; set; }

    public ulong HdTicketId { get; set; }

    public DateTime Timestamp { get; set; }

    public ulong? UserId { get; set; }

    public string? Comment { get; set; }

    public string? CommentLoc { get; set; }

    public string? Description { get; set; }

    public string? OwnersOnlyDescription { get; set; }

    public string? LocalizedDescription { get; set; }

    public string? LocalizedOwnersOnlyDescription { get; set; }

    public byte? Mailed { get; set; }

    public DateTime? MailedTimestamp { get; set; }

    public uint? MailerSession { get; set; }

    public string? NotifyUsers { get; set; }

    public string ViaEmail { get; set; } = null!;

    public bool OwnersOnly { get; set; }

    public bool? ResolutionChanged { get; set; }

    public byte? SystemComment { get; set; }

    public byte? TicketDataChange { get; set; }

    public virtual HdTicket HdTicket { get; set; } = null!;

    public virtual User? User { get; set; }
}
