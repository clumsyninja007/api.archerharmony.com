namespace api.archerharmony.com.Models.Telegram;

public class ChatTracker
{
    public long ChatId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool WaterReminder { get; set; }
    public bool Active { get; set; }
}