using Microsoft.EntityFrameworkCore;
using TelegramBot.Data.Entities;

namespace TelegramBot.Data;

public class TelegramBotContext(DbContextOptions<TelegramBotContext> options) : DbContext(options)
{
    public DbSet<ChatTracker> ChatTrackers => Set<ChatTracker>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Column names match the source (MySQL) chat_tracker table so the one-time snapshot
        // copies over column-for-column. ChatId is the Telegram-supplied chat id, so it's
        // ValueGeneratedNever.
        modelBuilder.Entity<ChatTracker>(entity =>
        {
            entity.ToTable("chat_tracker");
            entity.HasKey(e => e.ChatId);
            entity.Property(e => e.ChatId).HasColumnName("chat_id").ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.WaterReminder).HasColumnName("water_reminder");
            entity.Property(e => e.Active).HasColumnName("active");
        });
    }
}
