using System;
using System.Collections.Generic;

namespace api.archerharmony.com.Entities.Notkace;

public partial class User
{
    public ulong Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? BudgetCode { get; set; }

    public string? Domain { get; set; }

    public string? FullName { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? LdapImported { get; set; }

    public ulong? Permissions { get; set; }

    public string? LdapUid { get; set; }

    public string? HomePhone { get; set; }

    public string? WorkPhone { get; set; }

    public string? MobilePhone { get; set; }

    public string? PagerPhone { get; set; }

    public ulong? RoleId { get; set; }

    public ulong? LinkedApplianceId { get; set; }

    public string? Path { get; set; }

    public int? Level { get; set; }

    public ulong? ManagerId { get; set; }

    public ulong? LocationId { get; set; }

    public ulong? LocaleBrowserId { get; set; }

    public ulong? HdDefaultQueueId { get; set; }

    public string? HdDefaultView { get; set; }

    public ulong? ApiEnabled { get; set; }

    public ulong? SecurityNotifications { get; set; }

    public ulong? SalesNotifications { get; set; }

    public ulong? PrimaryDeviceId { get; set; }

    public ulong? DeviceCount { get; set; }

    public bool? IsArchived { get; set; }

    public DateTime? ArchivedDate { get; set; }

    public ulong? ArchivedBy { get; set; }

    public sbyte? _2faRequired { get; set; }

    public sbyte? _2faConfigured { get; set; }

    public byte[]? _2faSecret { get; set; }

    public DateTime? _2faCutoffDate { get; set; }

    public string? Theme { get; set; }

    public virtual ICollection<HdTicketChange> HdTicketChanges { get; set; } = new List<HdTicketChange>();

    public virtual ICollection<HdTicket> HdTicketOwners { get; set; } = new List<HdTicket>();

    public virtual ICollection<HdTicket> HdTicketSubmitters { get; set; } = new List<HdTicket>();
}
