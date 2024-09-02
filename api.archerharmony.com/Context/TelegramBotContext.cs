using api.archerharmony.com.Entities.Telegram;

namespace api.archerharmony.com.Context;

public partial class TelegramBotContext : DbContext
{
    public TelegramBotContext()
    {
    }

    public TelegramBotContext(DbContextOptions<TelegramBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatTracker> ChatTrackers { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<ChatTracker>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PRIMARY");

            entity.ToTable("chat_tracker");

            entity.Property(e => e.ChatId)
                .HasColumnType("bigint(20)")
                .HasColumnName("chat_id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.WaterReminder).HasColumnName("water_reminder");
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(95);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
