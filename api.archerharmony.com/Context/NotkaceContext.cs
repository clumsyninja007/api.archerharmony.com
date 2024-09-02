using api.archerharmony.com.Entities.Notkace;

namespace api.archerharmony.com.Context;

public partial class NotkaceContext : Microsoft.EntityFrameworkCore.DbContext
{
    public NotkaceContext()
    {
    }

    public NotkaceContext(DbContextOptions<NotkaceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<HdPriority> HdPriorities { get; set; }

    public virtual DbSet<HdStatus> HdStatuses { get; set; }

    public virtual DbSet<HdTicket> HdTickets { get; set; }

    public virtual DbSet<HdTicketChange> HdTicketChanges { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ASSET");

            entity.HasIndex(e => new { e.Archive, e.ArchiveDate }, "INDEX_ARCHIVE");

            entity.HasIndex(e => e.AssetStatusId, "INDEX_ASSET_STATUS_ID");

            entity.HasIndex(e => e.AssetClassId, "INDEX_CLASS_ID");

            entity.HasIndex(e => e.Name, "INDEX_NAME");

            entity.HasIndex(e => e.OwnerId, "INDEX_OWNER_ID");

            entity.HasIndex(e => new { e.AssetTypeId, e.AssetDataId }, "INDEX_TYPE_DATA_ID").IsUnique();

            entity.HasIndex(e => new { e.AssetTypeId, e.MappedId }, "INDEX_TYPE_MAPPED_ID");

            entity.HasIndex(e => new { e.AssetTypeId, e.Name }, "INDEX_TYPE_NAME");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Archive)
                .HasColumnType("enum('PENDING','COMPLETED','')")
                .HasColumnName("ARCHIVE");
            entity.Property(e => e.ArchiveDate)
                .HasMaxLength(6)
                .HasColumnName("ARCHIVE_DATE");
            entity.Property(e => e.ArchiveReason)
                .HasMaxLength(255)
                .HasColumnName("ARCHIVE_REASON")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.AssetClassId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ASSET_CLASS_ID");
            entity.Property(e => e.AssetDataId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ASSET_DATA_ID");
            entity.Property(e => e.AssetStatusId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ASSET_STATUS_ID");
            entity.Property(e => e.AssetTypeId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ASSET_TYPE_ID");
            entity.Property(e => e.Created)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("CREATED");
            entity.Property(e => e.LocationId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LOCATION_ID");
            entity.Property(e => e.MappedId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MAPPED_ID");
            entity.Property(e => e.Modified)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("MODIFIED");
            entity.Property(e => e.Name)
                .HasColumnName("NAME")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.OwnerId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("OWNER_ID");
        });
        
        modelBuilder.Entity<HdPriority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_PRIORITY");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .HasColumnName("COLOR")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.EscalationMinutes)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ESCALATION_MINUTES");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.IsSlaEnabled)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IS_SLA_ENABLED");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
            entity.Property(e => e.ResolutionDueDateMinutes)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("RESOLUTION_DUE_DATE_MINUTES");
            entity.Property(e => e.SlaNotificationRecurrenceMinutes)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SLA_NOTIFICATION_RECURRENCE_MINUTES");
            entity.Property(e => e.UseBusinessHoursForEscalation)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USE_BUSINESS_HOURS_FOR_ESCALATION");
            entity.Property(e => e.UseBusinessHoursForSla)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USE_BUSINESS_HOURS_FOR_SLA");
        });

        modelBuilder.Entity<HdStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_STATUS");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("STATE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<HdTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_TICKET");

            entity.HasIndex(e => e.HdCategoryId, "HD_CATEGORY_IDX");

            entity.HasIndex(e => e.HdImpactId, "HD_IMPACT_IDX");

            entity.HasIndex(e => e.HdPriorityId, "HD_PRIORITY_IDX");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.HasIndex(e => e.HdStatusId, "HD_STATUS_IDX");

            entity.HasIndex(e => e.AssetId, "IX_HD_TICKET_ASSET_ID");

            entity.HasIndex(e => e.SubmitterId, "IX_HD_TICKET_SUBMITTER_ID");

            entity.HasIndex(e => e.MachineId, "MACHINE_IDX");

            entity.HasIndex(e => new { e.OwnerId, e.HdStatusId }, "OWNER_STATUS");

            entity.HasIndex(e => e.ParentId, "PARENT");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Approval)
                .HasMaxLength(20)
                .HasColumnName("APPROVAL")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.ApprovalNote)
                .HasColumnName("APPROVAL_NOTE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.ApproveState)
                .HasMaxLength(20)
                .HasColumnName("APPROVE_STATE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.ApproverId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("APPROVER_ID");
            entity.Property(e => e.AssetId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ASSET_ID");
            entity.Property(e => e.CcList)
                .HasColumnName("CC_LIST")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Created)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("CREATED");
            entity.Property(e => e.CustomFieldValue0)
                .HasColumnName("CUSTOM_FIELD_VALUE0")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue1)
                .HasColumnName("CUSTOM_FIELD_VALUE1")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue10)
                .HasColumnName("CUSTOM_FIELD_VALUE10")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue11)
                .HasColumnName("CUSTOM_FIELD_VALUE11")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue12)
                .HasColumnName("CUSTOM_FIELD_VALUE12")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue13)
                .HasColumnName("CUSTOM_FIELD_VALUE13")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue14)
                .HasColumnName("CUSTOM_FIELD_VALUE14")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue15)
                .HasColumnName("CUSTOM_FIELD_VALUE15")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue2)
                .HasColumnName("CUSTOM_FIELD_VALUE2")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue3)
                .HasColumnName("CUSTOM_FIELD_VALUE3")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue4)
                .HasColumnName("CUSTOM_FIELD_VALUE4")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue5)
                .HasColumnName("CUSTOM_FIELD_VALUE5")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue6)
                .HasColumnName("CUSTOM_FIELD_VALUE6")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue7)
                .HasColumnName("CUSTOM_FIELD_VALUE7")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue8)
                .HasColumnName("CUSTOM_FIELD_VALUE8")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CustomFieldValue9)
                .HasColumnName("CUSTOM_FIELD_VALUE9")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.DueDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("DUE_DATE");
            entity.Property(e => e.Escalated)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("ESCALATED");
            entity.Property(e => e.HdCategoryId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_CATEGORY_ID");
            entity.Property(e => e.HdImpactId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_IMPACT_ID");
            entity.Property(e => e.HdPriorityId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_PRIORITY_ID");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.HdServiceStatusId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_SERVICE_STATUS_ID");
            entity.Property(e => e.HdStatusId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_STATUS_ID");
            entity.Property(e => e.HdUseProcessStatus)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("HD_USE_PROCESS_STATUS");
            entity.Property(e => e.IsManualDueDate)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IS_MANUAL_DUE_DATE");
            entity.Property(e => e.IsParent)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IS_PARENT");
            entity.Property(e => e.MachineId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MACHINE_ID");
            entity.Property(e => e.Modified)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("MODIFIED");
            entity.Property(e => e.OwnerId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("OWNER_ID");
            entity.Property(e => e.ParentId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("PARENT_ID");
            entity.Property(e => e.Resolution)
                .HasColumnName("RESOLUTION")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.SatisfactionComment)
                .HasColumnName("SATISFACTION_COMMENT")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.SatisfactionRating)
                .HasColumnType("int(11)")
                .HasColumnName("SATISFACTION_RATING");
            entity.Property(e => e.ServiceTicketId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SERVICE_TICKET_ID");
            entity.Property(e => e.SlaNotified)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("SLA_NOTIFIED");
            entity.Property(e => e.SubmitterId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SUBMITTER_ID");
            entity.Property(e => e.Summary)
                .HasColumnName("SUMMARY")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.TimeClosed)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("TIME_CLOSED");
            entity.Property(e => e.TimeOpened)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("TIME_OPENED");
            entity.Property(e => e.TimeStalled)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("TIME_STALLED");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("TITLE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.Asset).WithMany(p => p.HdTicketAssets).HasForeignKey(d => d.AssetId);

            entity.HasOne(d => d.HdPriority).WithMany(p => p.HdTickets).HasForeignKey(d => d.HdPriorityId);

            entity.HasOne(d => d.HdStatus).WithMany(p => p.HdTickets).HasForeignKey(d => d.HdStatusId);

            entity.HasOne(d => d.Machine).WithMany(p => p.HdTicketMachines).HasForeignKey(d => d.MachineId);

            entity.HasOne(d => d.Owner).WithMany(p => p.HdTicketOwners).HasForeignKey(d => d.OwnerId);

            entity.HasOne(d => d.Submitter).WithMany(p => p.HdTicketSubmitters).HasForeignKey(d => d.SubmitterId);
        });

        modelBuilder.Entity<HdTicketChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_TICKET_CHANGE");

            entity.HasIndex(e => e.HdTicketId, "HD_TICKET_IDX");

            entity.HasIndex(e => e.Mailed, "MAILED");

            entity.HasIndex(e => new { e.Mailed, e.MailerSession }, "MAILED_MAILER_SESSION_IDX");

            entity.HasIndex(e => e.MailerSession, "MAILER_SESSION");

            entity.HasIndex(e => e.UserId, "USER_IDX");

            entity.HasIndex(e => new { e.ViaEmail, e.Timestamp }, "VIA_EMAIL_TIMESTAMP_IDX");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasColumnType("mediumtext")
                .HasColumnName("COMMENT");
            entity.Property(e => e.CommentLoc)
                .HasColumnType("mediumtext")
                .HasColumnName("COMMENT_LOC");
            entity.Property(e => e.Description)
                .HasColumnType("mediumtext")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.HdTicketId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_TICKET_ID");
            entity.Property(e => e.LocalizedDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("LOCALIZED_DESCRIPTION");
            entity.Property(e => e.LocalizedOwnersOnlyDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("LOCALIZED_OWNERS_ONLY_DESCRIPTION");
            entity.Property(e => e.Mailed)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("MAILED");
            entity.Property(e => e.MailedTimestamp)
                .HasMaxLength(6)
                .HasColumnName("MAILED_TIMESTAMP");
            entity.Property(e => e.MailerSession)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11) unsigned")
                .HasColumnName("MAILER_SESSION");
            entity.Property(e => e.NotifyUsers)
                .HasColumnName("NOTIFY_USERS")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.OwnersOnly).HasColumnName("OWNERS_ONLY");
            entity.Property(e => e.OwnersOnlyDescription)
                .HasColumnType("mediumtext")
                .HasColumnName("OWNERS_ONLY_DESCRIPTION");
            entity.Property(e => e.ResolutionChanged)
                .HasDefaultValueSql("'0'")
                .HasColumnName("RESOLUTION_CHANGED");
            entity.Property(e => e.SystemComment)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SYSTEM_COMMENT");
            entity.Property(e => e.TicketDataChange)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("TICKET_DATA_CHANGE");
            entity.Property(e => e.Timestamp)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("TIMESTAMP");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("USER_ID");
            entity.Property(e => e.ViaEmail)
                .HasMaxLength(200)
                .HasColumnName("VIA_EMAIL")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.HdTicket).WithMany(p => p.HdTicketChanges).HasForeignKey(d => d.HdTicketId);

            entity.HasOne(d => d.User).WithMany(p => p.HdTicketChanges).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("USER");

            entity.HasIndex(e => e.LdapUid, "IDX_LDAP_UID");

            entity.HasIndex(e => e.UserName, "IDX_NAME");

            entity.HasIndex(e => e.ManagerId, "IDX_PARENT");

            entity.HasIndex(e => e.Path, "IDX_PATH");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.ApiEnabled)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("API_ENABLED");
            entity.Property(e => e.ArchivedBy)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ARCHIVED_BY");
            entity.Property(e => e.ArchivedDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("ARCHIVED_DATE");
            entity.Property(e => e.BudgetCode)
                .HasMaxLength(100)
                .HasColumnName("BUDGET_CODE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Created)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("CREATED");
            entity.Property(e => e.DeviceCount)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEVICE_COUNT");
            entity.Property(e => e.Domain)
                .HasMaxLength(100)
                .HasColumnName("DOMAIN")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("EMAIL")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("FULL_NAME")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.HdDefaultQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_DEFAULT_QUEUE_ID");
            entity.Property(e => e.HdDefaultView)
                .HasMaxLength(255)
                .HasColumnName("HD_DEFAULT_VIEW")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.HomePhone)
                .HasMaxLength(255)
                .HasColumnName("HOME_PHONE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.IsArchived)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IS_ARCHIVED");
            entity.Property(e => e.LdapImported)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("LDAP_IMPORTED");
            entity.Property(e => e.LdapUid)
                .HasColumnName("LDAP_UID")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Level)
                .HasColumnType("int(11)")
                .HasColumnName("LEVEL");
            entity.Property(e => e.LinkedApplianceId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LINKED_APPLIANCE_ID");
            entity.Property(e => e.LocaleBrowserId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LOCALE_BROWSER_ID");
            entity.Property(e => e.LocationId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LOCATION_ID");
            entity.Property(e => e.ManagerId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MANAGER_ID");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(255)
                .HasColumnName("MOBILE_PHONE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Modified)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("MODIFIED");
            entity.Property(e => e.PagerPhone)
                .HasMaxLength(255)
                .HasColumnName("PAGER_PHONE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("PASSWORD")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Path)
                .HasColumnName("PATH")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Permissions)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("PERMISSIONS");
            entity.Property(e => e.PrimaryDeviceId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("PRIMARY_DEVICE_ID");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.SalesNotifications)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SALES_NOTIFICATIONS");
            entity.Property(e => e.SecurityNotifications)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SECURITY_NOTIFICATIONS");
            entity.Property(e => e.Theme)
                .HasMaxLength(50)
                .HasColumnName("THEME")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("USER_NAME")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(255)
                .HasColumnName("WORK_PHONE")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e._2faConfigured)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(2)")
                .HasColumnName("2FA_CONFIGURED");
            entity.Property(e => e._2faCutoffDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("current_timestamp(6)")
                .HasColumnName("2FA_CUTOFF_DATE");
            entity.Property(e => e._2faRequired)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(2)")
                .HasColumnName("2FA_REQUIRED");
            entity.Property(e => e._2faSecret)
                .HasColumnType("tinyblob")
                .HasColumnName("2FA_SECRET");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
