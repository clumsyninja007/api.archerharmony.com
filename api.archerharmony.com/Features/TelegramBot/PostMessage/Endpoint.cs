using System.Net.Http;
using api.archerharmony.com.Features.TelegramBot.Shared;
using api.archerharmony.com.Services;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace api.archerharmony.com.Features.TelegramBot.PostMessage;

public class Endpoint(
    IOptions<BotConfiguration> botConfig,
    TelegramBotContext context,
    IUpdateService updateService,
    IHttpClientFactory clientFactory)
    : Endpoint<Request>
{
    public override void Configure()
    {
        Post("update/{token}");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (req.Token != botConfig.Value.BotToken)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        if (req.Update.Type == UpdateType.Message)
        {
            var message = req.Update.Message;
            ChatId chatId = message.Chat.Id;

            // if the message is a command
            if (message.EntityValues != null && message.EntityValues.Any())
            {
                var command = message.EntityValues.First();
                var commandProps = message.Entities.First();

                switch (command)
                {
                    case "/water":
                    {
                        var commandText = message.Text.Substring(commandProps.Offset + commandProps.Length).Trim().ToLower();
                        bool value;
                        string msgText;

                        switch (commandText)
                        {
                            case "on":
                                value = true;
                                msgText = "enabled";
                                break;
                            case "off":
                                value = false;
                                msgText = "disabled";
                                break;
                            default:
                                await new SendInvalidCommandMessage(chatId).ExecuteAsync(ct);
                                return;
                        }

                        var chat = await context.ChatTracker
                            .Where(c => c.ChatId == chatId.Identifier)
                            .FirstOrDefaultAsync(ct);

                        if (chat == null)
                        {
                            ThrowError("Chat not found");
                            return;
                        }

                        if (chat.WaterReminder != value)
                        {
                            chat.WaterReminder = value;
                            await context.SaveChangesAsync(ct);
                            await updateService.SendMessageAsync(chatId, $"Water notifications {msgText}");
                        }
                        else
                        {
                            await updateService.SendMessageAsync(chatId, $"Water notifications already {msgText}");
                        }

                        break;
                    }
                    case "/echo":
                        await updateService.EchoAsync(message, commandProps);
                        break;
                    case "/joke":
                    {
                        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, "https://icanhazdadjoke.com/");
                        request.Headers.Add("Accept", "text/plain");

                        var client = clientFactory.CreateClient();

                        var response = await client.SendAsync(request, ct);

                        if (response.IsSuccessStatusCode)
                        {
                            var joke = await response.Content.ReadAsStringAsync(ct);

                            await updateService.SendMessageAsync(chatId, joke);
                        }
                        else
                        {
                            await updateService.SendMessageAsync(chatId, "Error getting joke :(");
                        }

                        client.Dispose();

                        break;
                    }
                    default:
                        await new SendInvalidCommandMessage(chatId).ExecuteAsync(ct);
                        break;
                }
            }
        }
    }
}