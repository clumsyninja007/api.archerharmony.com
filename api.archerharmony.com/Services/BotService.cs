using System.Net.Http;
using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;

namespace api.archerharmony.com.Services;

public class BotService : IBotService
{
    public BotService(BotConfiguration botConfig)
    {
        if (string.IsNullOrEmpty(botConfig.Socks5Host))
        {
            Client = new TelegramBotClient(botConfig.BotToken);
        }
        else
        {
            var proxy = new HttpToSocks5Proxy(
                botConfig.Socks5Host, 
                botConfig.Socks5Port
            );

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = true
            };
            var httpClient = new HttpClient(httpClientHandler);
            
            Client = new TelegramBotClient(new TelegramBotClientOptions(botConfig.BotToken), httpClient);
        }
    }

    public TelegramBotClient Client { get; }
}