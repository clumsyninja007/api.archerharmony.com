namespace api.archerharmony.com.Features.TelegramBot.SendWaterReminders;

public record Request
{
    public string Token { get; init; } = null!;
}