using System.Net.Http;
using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;

namespace api.archerharmony.com.Services;

public class BotService : IBotService
{
    public BotService(IOptions<BotConfiguration> botConfig)
    {
        var config = botConfig.Value;

        if (string.IsNullOrEmpty(config.Socks5Host))
        {
            Client = new TelegramBotClient(config.BotToken);
        }
        else
        {
            var proxy = new HttpToSocks5Proxy(
                config.Socks5Host, 
                config.Socks5Port
            );

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = true
            };
            var httpClient = new HttpClient(httpClientHandler);
            
            Client = new TelegramBotClient(new TelegramBotClientOptions(config.BotToken), httpClient);
        }
    }

    public TelegramBotClient Client { get; }
}