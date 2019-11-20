using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;

namespace api.archerharmony.com.Services
{
    public class BotService : IBotService
    {
        public BotService(IOptions<BotConfiguration> botConfig)
        {
            var config = botConfig.Value;
            // use proxy if configured in appsettings.*.json
            Client = string.IsNullOrEmpty(config.Socks5Host)
                ? new TelegramBotClient(config.BotToken)
                : new TelegramBotClient(
                    config.BotToken,
                    new HttpToSocks5Proxy(config.Socks5Host, config.Socks5Port));
        }

        public TelegramBotClient Client { get; }
    }
}
