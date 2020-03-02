using api.archerharmony.com.Models.Notkace;
using Microsoft.EntityFrameworkCore;

namespace api.archerharmony.com.DbContext
{
    public class NotkaceContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public NotkaceContext()
        {
        }

        public NotkaceContext(DbContextOptions<NotkaceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<HdPriority> HdPriority { get; set; }
        public virtual DbSet<HdStatus> HdStatus { get; set; }
        public virtual DbSet<HdTicket> HdTicket { get; set; }
        public virtual DbSet<HdTicketChange> HdTicketChange { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("ASSET", "notkace");

                entity.HasIndex(e => e.AssetClassId)
                    .HasName("INDEX_CLASS_ID");

                entity.HasIndex(e => e.AssetStatusId)
                    .HasName("INDEX_ASSET_STATUS_ID");

                entity.HasIndex(e => e.Name)
                    .HasName("INDEX_NAME");

                entity.HasIndex(e => e.OwnerId)
                    .HasName("INDEX_OWNER_ID");

                entity.HasIndex(e => new { e.Archive, e.ArchiveDate })
                    .HasName("INDEX_ARCHIVE");

                entity.HasIndex(e => new { e.AssetTypeId, e.AssetDataId })
                    .HasName("INDEX_TYPE_DATA_ID")
                    .IsUnique();

                entity.HasIndex(e => new { e.AssetTypeId, e.MappedId })
                    .HasName("INDEX_TYPE_MAPPED_ID");

                entity.HasIndex(e => new { e.AssetTypeId, e.Name })
                    .HasName("INDEX_TYPE_NAME");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Archive)
                    .HasColumnName("ARCHIVE")
                    .HasColumnType("enum('PENDING','COMPLETED','')");

                entity.Property(e => e.ArchiveDate).HasColumnName("ARCHIVE_DATE");

                entity.Property(e => e.ArchiveReason)
                    .HasColumnName("ARCHIVE_REASON")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AssetClassId)
                    .HasColumnName("ASSET_CLASS_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AssetDataId)
                    .HasColumnName("ASSET_DATA_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AssetStatusId)
                    .HasColumnName("ASSET_STATUS_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AssetTypeId)
                    .HasColumnName("ASSET_TYPE_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Created)
                    .HasColumnName("CREATED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.LocationId)
                    .HasColumnName("LOCATION_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MappedId)
                    .HasColumnName("MAPPED_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Modified)
                    .HasColumnName("MODIFIED")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<HdPriority>(entity =>
            {
                entity.ToTable("HD_PRIORITY", "notkace");

                entity.HasIndex(e => e.HdQueueId)
                    .HasName("HD_QUEUE_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("COLOR")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EscalationMinutes)
                    .HasColumnName("ESCALATION_MINUTES")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.HdQueueId)
                    .HasColumnName("HD_QUEUE_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsSlaEnabled)
                    .HasColumnName("IS_SLA_ENABLED")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ORDINAL")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ResolutionDueDateMinutes)
                    .HasColumnName("RESOLUTION_DUE_DATE_MINUTES")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SlaNotificationRecurrenceMinutes)
                    .HasColumnName("SLA_NOTIFICATION_RECURRENCE_MINUTES")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UseBusinessHoursForEscalation)
                    .HasColumnName("USE_BUSINESS_HOURS_FOR_ESCALATION")
                    .HasColumnType("tinyint(1) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UseBusinessHoursForSla)
                    .HasColumnName("USE_BUSINESS_HOURS_FOR_SLA")
                    .HasColumnType("tinyint(1) unsigned")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<HdStatus>(entity =>
            {
                entity.ToTable("HD_STATUS", "notkace");

                entity.HasIndex(e => e.HdQueueId)
                    .HasName("HD_QUEUE_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.HdQueueId)
                    .HasColumnName("HD_QUEUE_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ORDINAL")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("STATE")
                    .HasColumnType("enum('opened','closed','stalled')")
                    .HasDefaultValueSql("opened");
            });

            modelBuilder.Entity<HdTicket>(entity =>
            {
                entity.ToTable("HD_TICKET", "notkace");

                entity.HasIndex(e => e.HdCategoryId)
                    .HasName("HD_CATEGORY_IDX");

                entity.HasIndex(e => e.HdImpactId)
                    .HasName("HD_IMPACT_IDX");

                entity.HasIndex(e => e.HdPriorityId)
                    .HasName("HD_PRIORITY_IDX");

                entity.HasIndex(e => e.HdQueueId)
                    .HasName("HD_QUEUE_IDX");

                entity.HasIndex(e => e.HdStatusId)
                    .HasName("HD_STATUS_IDX");

                entity.HasIndex(e => e.MachineId)
                    .HasName("MACHINE_IDX");

                entity.HasIndex(e => e.ParentId)
                    .HasName("PARENT");

                entity.HasIndex(e => new { e.OwnerId, e.HdStatusId })
                    .HasName("OWNER_STATUS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Approval)
                    .HasColumnName("APPROVAL")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovalNote)
                    .HasColumnName("APPROVAL_NOTE")
                    .IsUnicode(false);

                entity.Property(e => e.ApproveState)
                    .HasColumnName("APPROVE_STATE")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ApproverId)
                    .HasColumnName("APPROVER_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.AssetId)
                    .HasColumnName("ASSET_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CcList)
                    .IsRequired()
                    .HasColumnName("CC_LIST")
                    .IsUnicode(false);

                entity.Property(e => e.Created)
                    .HasColumnName("CREATED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.CustomFieldValue0)
                    .HasColumnName("CUSTOM_FIELD_VALUE0")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue1)
                    .HasColumnName("CUSTOM_FIELD_VALUE1")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue10)
                    .HasColumnName("CUSTOM_FIELD_VALUE10")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue11)
                    .HasColumnName("CUSTOM_FIELD_VALUE11")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue12)
                    .HasColumnName("CUSTOM_FIELD_VALUE12")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue13)
                    .HasColumnName("CUSTOM_FIELD_VALUE13")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue14)
                    .HasColumnName("CUSTOM_FIELD_VALUE14")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue15)
                    .HasColumnName("CUSTOM_FIELD_VALUE15")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue2)
                    .HasColumnName("CUSTOM_FIELD_VALUE2")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue3)
                    .HasColumnName("CUSTOM_FIELD_VALUE3")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue4)
                    .HasColumnName("CUSTOM_FIELD_VALUE4")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue5)
                    .HasColumnName("CUSTOM_FIELD_VALUE5")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue6)
                    .HasColumnName("CUSTOM_FIELD_VALUE6")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue7)
                    .HasColumnName("CUSTOM_FIELD_VALUE7")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue8)
                    .HasColumnName("CUSTOM_FIELD_VALUE8")
                    .IsUnicode(false);

                entity.Property(e => e.CustomFieldValue9)
                    .HasColumnName("CUSTOM_FIELD_VALUE9")
                    .IsUnicode(false);

                entity.Property(e => e.DueDate)
                    .HasColumnName("DUE_DATE")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.Escalated)
                    .HasColumnName("ESCALATED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.HdCategoryId)
                    .HasColumnName("HD_CATEGORY_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HdImpactId)
                    .HasColumnName("HD_IMPACT_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HdPriorityId)
                    .HasColumnName("HD_PRIORITY_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HdQueueId)
                    .HasColumnName("HD_QUEUE_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HdServiceStatusId)
                    .HasColumnName("HD_SERVICE_STATUS_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.HdStatusId)
                    .HasColumnName("HD_STATUS_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HdUseProcessStatus)
                    .HasColumnName("HD_USE_PROCESS_STATUS")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsManualDueDate)
                    .HasColumnName("IS_MANUAL_DUE_DATE")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsParent)
                    .HasColumnName("IS_PARENT")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MachineId)
                    .HasColumnName("MACHINE_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Modified)
                    .HasColumnName("MODIFIED")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Resolution)
                    .HasColumnName("RESOLUTION")
                    .IsUnicode(false);

                entity.Property(e => e.SatisfactionComment)
                    .HasColumnName("SATISFACTION_COMMENT")
                    .IsUnicode(false);

                entity.Property(e => e.SatisfactionRating)
                    .HasColumnName("SATISFACTION_RATING")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ServiceTicketId)
                    .HasColumnName("SERVICE_TICKET_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.SlaNotified)
                    .HasColumnName("SLA_NOTIFIED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.SubmitterId)
                    .HasColumnName("SUBMITTER_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Summary)
                    .HasColumnName("SUMMARY")
                    .IsUnicode(false);

                entity.Property(e => e.TimeClosed)
                    .HasColumnName("TIME_CLOSED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.TimeOpened)
                    .HasColumnName("TIME_OPENED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.TimeStalled)
                    .HasColumnName("TIME_STALLED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(e => e.Owner)
                    .WithMany(u => u.HdTicketOwner)
                    .HasForeignKey(t => t.OwnerId);

                entity.HasOne(e => e.Submitter)
                    .WithMany(u => u.HdTicketSubmitter)
                    .HasForeignKey(t => t.SubmitterId);

                entity.HasOne(e => e.Asset)
                    .WithMany(a => a.HdTicketsAsset)
                    .HasForeignKey(t => t.AssetId);

                entity.HasOne(e => e.Machine)
                    .WithMany(a => a.HdTicketsMachine)
                    .HasForeignKey(t => new { t.MachineId });

                entity.HasOne(e => e.Status)
                    .WithMany(s => s.HdTickets)
                    .HasForeignKey(t => t.HdStatusId);

                entity.HasOne(e => e.Priority)
                    .WithMany(p => p.HdTickets)
                    .HasForeignKey(t => t.HdPriorityId);
            });

            modelBuilder.Entity<HdTicketChange>(entity =>
            {
                entity.ToTable("HD_TICKET_CHANGE", "notkace");

                entity.HasIndex(e => e.HdTicketId)
                    .HasName("HD_TICKET_IDX");

                entity.HasIndex(e => e.Mailed)
                    .HasName("MAILED");

                entity.HasIndex(e => e.MailerSession)
                    .HasName("MAILER_SESSION");

                entity.HasIndex(e => e.UserId)
                    .HasName("USER_IDX");

                entity.HasIndex(e => new { e.Mailed, e.MailerSession })
                    .HasName("MAILED_MAILER_SESSION_IDX");

                entity.HasIndex(e => new { e.ViaEmail, e.Timestamp })
                    .HasName("VIA_EMAIL_TIMESTAMP_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Comment)
                    .HasColumnName("COMMENT")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.CommentLoc)
                    .HasColumnName("COMMENT_LOC")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.HdTicketId)
                    .HasColumnName("HD_TICKET_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.LocalizedDescription)
                    .HasColumnName("LOCALIZED_DESCRIPTION")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.LocalizedOwnersOnlyDescription)
                    .HasColumnName("LOCALIZED_OWNERS_ONLY_DESCRIPTION")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.Mailed)
                    .HasColumnName("MAILED")
                    .HasColumnType("tinyint(1) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MailedTimestamp).HasColumnName("MAILED_TIMESTAMP");

                entity.Property(e => e.MailerSession)
                    .HasColumnName("MAILER_SESSION")
                    .HasColumnType("int(11) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.NotifyUsers)
                    .HasColumnName("NOTIFY_USERS")
                    .IsUnicode(false);

                entity.Property(e => e.OwnersOnly)
                    .HasColumnName("OWNERS_ONLY")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.OwnersOnlyDescription)
                    .HasColumnName("OWNERS_ONLY_DESCRIPTION")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.ResolutionChanged)
                    .HasColumnName("RESOLUTION_CHANGED")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SystemComment)
                    .HasColumnName("SYSTEM_COMMENT")
                    .HasColumnType("tinyint(1) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TicketDataChange)
                    .HasColumnName("TICKET_DATA_CHANGE")
                    .HasColumnType("tinyint(1) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("TIMESTAMP")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ViaEmail)
                    .IsRequired()
                    .HasColumnName("VIA_EMAIL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(e => e.HdTicket)
                    .WithMany(t => t.HdTicketChanges)
                    .HasForeignKey(e => e.HdTicketId);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.HdTicketChanges)
                    .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("USER", "notkace");

                entity.HasIndex(e => e.LdapUid)
                    .HasName("IDX_LDAP_UID");

                entity.HasIndex(e => e.ManagerId)
                    .HasName("IDX_PARENT");

                entity.HasIndex(e => e.Path)
                    .HasName("IDX_PATH");

                entity.HasIndex(e => e.UserName)
                    .HasName("IDX_NAME");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApiEnabled)
                    .HasColumnName("API_ENABLED")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ArchivedBy)
                    .HasColumnName("ARCHIVED_BY")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ArchivedDate)
                    .HasColumnName("ARCHIVED_DATE")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.BudgetCode)
                    .HasColumnName("BUDGET_CODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Created)
                    .HasColumnName("CREATED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.DeviceCount)
                    .HasColumnName("DEVICE_COUNT")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Domain)
                    .HasColumnName("DOMAIN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("FULL_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HdDefaultQueueId)
                    .HasColumnName("HD_DEFAULT_QUEUE_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.HdDefaultView)
                    .HasColumnName("HD_DEFAULT_VIEW")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("HOME_PHONE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsArchived)
                    .HasColumnName("IS_ARCHIVED")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LdapImported)
                    .HasColumnName("LDAP_IMPORTED")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.LdapUid)
                    .HasColumnName("LDAP_UID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .HasColumnName("LEVEL")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LinkedApplianceId)
                    .HasColumnName("LINKED_APPLIANCE_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.LocaleBrowserId)
                    .HasColumnName("LOCALE_BROWSER_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LocationId)
                    .HasColumnName("LOCATION_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ManagerId)
                    .HasColumnName("MANAGER_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("MOBILE_PHONE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnName("MODIFIED")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PagerPhone)
                    .HasColumnName("PAGER_PHONE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("PASSWORD")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Path)
                    .HasColumnName("PATH")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Permissions)
                    .HasColumnName("PERMISSIONS")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.PrimaryDeviceId)
                    .HasColumnName("PRIMARY_DEVICE_ID")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SalesNotifications)
                    .HasColumnName("SALES_NOTIFICATIONS")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.SecurityNotifications)
                    .HasColumnName("SECURITY_NOTIFICATIONS")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Theme)
                    .HasColumnName("THEME")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WorkPhone)
                    .HasColumnName("WORK_PHONE")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e._2faConfigured)
                    .HasColumnName("2FA_CONFIGURED")
                    .HasColumnType("tinyint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e._2faCutoffDate)
                    .HasColumnName("2FA_CUTOFF_DATE")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e._2faRequired)
                    .HasColumnName("2FA_REQUIRED")
                    .HasColumnType("tinyint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e._2faSecret)
                    .HasColumnName("2FA_SECRET")
                    .HasColumnType("tinyblob")
                    .IsRequired(false);
            });
        }
    }
}
