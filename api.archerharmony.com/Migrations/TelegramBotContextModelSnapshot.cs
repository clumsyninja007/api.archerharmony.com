﻿// <auto-generated />

using api.archerharmony.com.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace api.archerharmony.com.Migrations
{
    [DbContext(typeof(TelegramBotContext))]
    partial class TelegramBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("api.archerharmony.com.Models.Telegram.ChatTracker", b =>
                {
                    b.Property<long>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("chat_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnName("first_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnName("last_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("WaterReminder")
                        .HasColumnName("water_reminder")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("ChatId");

                    b.ToTable("chat_tracker");
                });
#pragma warning restore 612, 618
        }
    }
}
