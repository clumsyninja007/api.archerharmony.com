﻿namespace api.archerharmony.com.Entities.Entities.Telegram;

public partial class ChatTracker
{
    public long ChatId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool WaterReminder { get; set; }

    public bool Active { get; set; }
}
