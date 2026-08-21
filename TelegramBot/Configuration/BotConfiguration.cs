namespace TelegramBot.Configuration;

public record BotConfiguration
{
    public required string BotToken { get; init; }
}
