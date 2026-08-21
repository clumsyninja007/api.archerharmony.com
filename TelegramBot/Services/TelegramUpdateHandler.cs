using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Data;

namespace TelegramBot.Services;

public interface ITelegramUpdateHandler
{
    Task HandleAsync(Update update, CancellationToken ct);
}

public class TelegramUpdateHandler(
    ITelegramBotClient bot,
    TelegramBotContext context,
    IHttpClientFactory httpClientFactory,
    ILogger<TelegramUpdateHandler> logger) : ITelegramUpdateHandler
{
    public async Task HandleAsync(Update update, CancellationToken ct)
    {
        if (update.Type != UpdateType.Message || update.Message is null)
        {
            return;
        }

        var message = update.Message;
        long chatId = message.Chat.Id;

        // Only react to messages that carry a bot-command entity. Strip any "@botname" suffix
        // Telegram appends (always in groups, sometimes in 1:1) so "/joke" and "/joke@ArchBot"
        // both match.
        var command = message.EntityValues?.FirstOrDefault()?.Split('@')[0];
        var commandProps = message.Entities?.FirstOrDefault();
        if (command is null || commandProps is null)
        {
            return;
        }

        switch (command)
        {
            case "/water":
                await HandleWaterAsync(chatId, message, commandProps, ct);
                break;
            case "/echo":
                await EchoAsync(chatId, message, commandProps, ct);
                break;
            case "/joke":
                await SendJokeAsync(chatId, ct);
                break;
            default:
                await bot.SendMessage(chatId, "Invalid command", cancellationToken: ct);
                break;
        }
    }

    private async Task HandleWaterAsync(long chatId, Message message, MessageEntity commandProps, CancellationToken ct)
    {
        var argument = message.Text?[(commandProps.Offset + commandProps.Length)..].Trim().ToLowerInvariant();

        var (value, label) = argument switch
        {
            "on" => ((bool?)true, "enabled"),
            "off" => ((bool?)false, "disabled"),
            _ => (null, "")
        };

        if (value is null)
        {
            await bot.SendMessage(chatId, "Usage: /water on|off", cancellationToken: ct);
            return;
        }

        var chat = await context.ChatTrackers.FirstOrDefaultAsync(c => c.ChatId == chatId, ct);
        if (chat is null)
        {
            await bot.SendMessage(chatId, "Chat not found", cancellationToken: ct);
            return;
        }

        if (chat.WaterReminder != value.Value)
        {
            chat.WaterReminder = value.Value;
            await context.SaveChangesAsync(ct);
            await bot.SendMessage(chatId, $"Water notifications {label}", cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(chatId, $"Water notifications already {label}", cancellationToken: ct);
        }
    }

    private async Task EchoAsync(long chatId, Message message, MessageEntity commandProps, CancellationToken ct)
    {
        logger.LogInformation("Echo from {ChatId}", chatId);

        switch (message.Type)
        {
            case MessageType.Text when !string.IsNullOrWhiteSpace(message.Text):
                var echo = message.Text[(commandProps.Offset + commandProps.Length)..].Trim();
                await bot.SendMessage(chatId, string.IsNullOrEmpty(echo) ? "(nothing to echo)" : echo, cancellationToken: ct);
                break;
            case MessageType.Photo:
                await bot.SendMessage(chatId, "Thx for the Pics", cancellationToken: ct);
                break;
            default:
                await bot.SendMessage(chatId, "Message type not supported", cancellationToken: ct);
                break;
        }
    }

    private async Task SendJokeAsync(long chatId, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://icanhazdadjoke.com/");
        request.Headers.Add("Accept", "text/plain");

        var client = httpClientFactory.CreateClient();
        var response = await client.SendAsync(request, ct);

        if (response.IsSuccessStatusCode)
        {
            var joke = await response.Content.ReadAsStringAsync(ct);
            await bot.SendMessage(chatId, joke, cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(chatId, "Error getting joke :(", cancellationToken: ct);
        }
    }
}
