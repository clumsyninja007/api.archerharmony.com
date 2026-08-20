using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data.Entities;

namespace Notkace.Api.Data;

public class NotkaceContext(DbContextOptions<NotkaceContext> options) : DbContext(options)
{
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<HdPriority> HdPriorities => Set<HdPriority>();
    public DbSet<HdStatus> HdStatuses => Set<HdStatus>();
    public DbSet<HdTicket> HdTickets => Set<HdTicket>();
    public DbSet<HdTicketChange> HdTicketChanges => Set<HdTicketChange>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table/column names are kept identical to the source KACE (MySQL) schema so the
        // one-time data snapshot copies over column-for-column. IDs originate in KACE, so
        // every key is ValueGeneratedNever.
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.ToTable("ASSET");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.AssetTypeId).HasColumnName("ASSET_TYPE_ID");
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<HdPriority>(entity =>
        {
            entity.ToTable("HD_PRIORITY");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.Ordinal).HasColumnName("ORDINAL");
        });

        modelBuilder.Entity<HdStatus>(entity =>
        {
            entity.ToTable("HD_STATUS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.Ordinal).HasColumnName("ORDINAL");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.UserName).HasColumnName("USER_NAME");
            entity.Property(e => e.FullName).HasColumnName("FULL_NAME");
            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
        });

        modelBuilder.Entity<HdTicket>(entity =>
        {
            entity.ToTable("HD_TICKET");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.Title).HasColumnName("TITLE");
            entity.Property(e => e.Summary).HasColumnName("SUMMARY");
            entity.Property(e => e.HdQueueId).HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.Created).HasColumnName("CREATED");
            entity.Property(e => e.HdPriorityId).HasColumnName("HD_PRIORITY_ID");
            entity.Property(e => e.HdStatusId).HasColumnName("HD_STATUS_ID");
            entity.Property(e => e.OwnerId).HasColumnName("OWNER_ID");
            entity.Property(e => e.SubmitterId).HasColumnName("SUBMITTER_ID");
            entity.Property(e => e.AssetId).HasColumnName("ASSET_ID");
            entity.Property(e => e.CustomFieldValue1).HasColumnName("CUSTOM_FIELD_VALUE1");
            entity.Property(e => e.CustomFieldValue2).HasColumnName("CUSTOM_FIELD_VALUE2");
            entity.Property(e => e.CustomFieldValue5).HasColumnName("CUSTOM_FIELD_VALUE5");

            entity.HasIndex(e => new { e.OwnerId, e.HdStatusId });
            entity.HasIndex(e => e.HdQueueId);

            entity.HasOne(e => e.HdPriority).WithMany().HasForeignKey(e => e.HdPriorityId);
            entity.HasOne(e => e.HdStatus).WithMany().HasForeignKey(e => e.HdStatusId);
            entity.HasOne(e => e.Owner).WithMany().HasForeignKey(e => e.OwnerId);
            entity.HasOne(e => e.Submitter).WithMany().HasForeignKey(e => e.SubmitterId);
            entity.HasOne(e => e.Asset).WithMany().HasForeignKey(e => e.AssetId);
        });

        modelBuilder.Entity<HdTicketChange>(entity =>
        {
            entity.ToTable("HD_TICKET_CHANGE");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.HdTicketId).HasColumnName("HD_TICKET_ID");
            entity.Property(e => e.Timestamp).HasColumnName("TIMESTAMP");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.Comment).HasColumnName("COMMENT");
            entity.Property(e => e.OwnersOnly).HasColumnName("OWNERS_ONLY");

            entity.HasIndex(e => e.HdTicketId);

            entity.HasOne(e => e.HdTicket).WithMany().HasForeignKey(e => e.HdTicketId);
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });
    }
}
