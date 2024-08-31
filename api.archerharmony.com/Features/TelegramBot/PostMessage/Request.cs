using Telegram.Bot.Types;

namespace api.archerharmony.com.Features.TelegramBot.PostMessage;

public record Request
{
    public string Token { get; init; }
    
    [FromBody]
    public Update Update { get; init; }
}