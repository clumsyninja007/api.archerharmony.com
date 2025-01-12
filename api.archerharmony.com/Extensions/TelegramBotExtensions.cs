using System.Net.Http;
using api.archerharmony.com.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MihaZupan;
using Telegram.Bot;

namespace api.archerharmony.com.Extensions;

public static class TelegramBotExtensions
{
    public static WebApplicationBuilder AddTelegramBotClient(this WebApplicationBuilder builder)
    {
        var botToken = builder.GetSecretOrEnvVar("TelegramBot__BotConfiguration__BotToken");
        if (string.IsNullOrEmpty(botToken))
        {
            throw new Exception("TelegramBot__BotConfiguration__BotToken is required");
        }

        var botConfig = new BotConfiguration
        {
            BotToken = botToken
        };

        var client = botConfig.BuildBotClient();
        builder.Services.AddSingleton(client);
        return builder;
    }

    private static TelegramBotClient BuildBotClient(this BotConfiguration botConfig)
    {
        TelegramBotClient client;
        if (string.IsNullOrEmpty(botConfig.Socks5Host))
        {
            client = new TelegramBotClient(botConfig.BotToken);
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
            
            client = new TelegramBotClient(new TelegramBotClientOptions(botConfig.BotToken), httpClient);
        }
        
        return client;
    }
}