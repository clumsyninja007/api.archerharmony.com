using Telegram.Bot;

namespace api.archerharmony.com.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
