using System.IO;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace api.archerharmony.com.Features.TelegramBot.PostMessage;

public interface IService
{
    Task EchoAsync(Message message, MessageEntity commandProps);
    Task SendMessageAsync(ChatId chatId, string message);
}

[RegisterService<IService>(LifeTime.Scoped)]
public class Service(
    ILogger<Service> logger,
    TelegramBotClient client) : IService
{
    public async Task EchoAsync(Message message, MessageEntity commandProps)
    {
        logger.LogInformation("Received Message from {ChatId}", message.Chat.Id);

        switch (message.Type)
        {
            case MessageType.Text:
                if (string.IsNullOrWhiteSpace(message.Text))
                {
                    await client.SendMessage(message.Chat.Id, "Message type not supported");
                    break;
                }
                
                // Echo each Message
                await client.SendMessage(
                    message.Chat.Id,
                    message.Text[(commandProps.Offset + commandProps.Length)..].Trim());
                break;
            case MessageType.Photo:
            {
                // Download Photo
                var fileId = message.Photo?.LastOrDefault()?.FileId;

                if (string.IsNullOrEmpty(fileId))
                {
                    await client.SendMessage(message.Chat.Id, "File not found");
                    break;
                }
                
                var file = await client.GetFile(fileId);

                if (string.IsNullOrEmpty(file.FilePath))
                {
                    await client.SendMessage(message.Chat.Id, "File not found");
                    break;
                }
                
                var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                await using (var saveImageStream = File.Open(filename, FileMode.Create))
                {
                    await client.DownloadFile(file.FilePath, saveImageStream);
                }

                await client.SendMessage(message.Chat.Id, "Thx for the Pics");
                break;
            }
            default:
                await client.SendMessage(message.Chat.Id, "Message type not supported");
                break;
        }
    }

    public async Task SendMessageAsync(ChatId chatId, string message)
    {
        await client.SendMessage(chatId, message);
    }
}