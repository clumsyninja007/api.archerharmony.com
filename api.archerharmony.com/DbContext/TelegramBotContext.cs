using api.archerharmony.com.Models.Telegram;
using Microsoft.EntityFrameworkCore;

namespace api.archerharmony.com.DbContext
{
    public class TelegramBotContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TelegramBotContext()
        {
        }

        public TelegramBotContext(DbContextOptions<TelegramBotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatTracker> ChatTracker { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatTracker>(entity =>
            {
                entity.HasKey(e => e.ChatId);

                entity.ToTable("chat_tracker");

                entity.Property(e => e.ChatId)
                    .HasColumnName("chat_id");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name");

                entity.Property(e => e.WaterReminder)
                    .HasColumnName("water_reminder");

                entity.Property(e => e.Active)
                    .HasColumnName("active");
            });
        }
    }
}
