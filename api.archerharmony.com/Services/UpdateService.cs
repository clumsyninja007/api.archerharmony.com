using Microsoft.Extensions.Logging;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace api.archerharmony.com.Services;

public class UpdateService : IUpdateService
{
    private readonly IBotService _botService;
    private readonly ILogger<UpdateService> _logger;

    public UpdateService(IBotService botService, ILogger<UpdateService> logger)
    {
        _botService = botService;
        _logger = logger;
    }

    public async Task EchoAsync(Message message, MessageEntity commandProps)
    {
        _logger.LogInformation("Received Message from {ChatId}", message.Chat.Id);

        switch (message.Type)
        {
            case MessageType.Text:
                if (string.IsNullOrWhiteSpace(message.Text))
                {
                    await _botService.Client.SendMessage(message.Chat.Id, "Message type not supported");
                    break;
                }
                
                // Echo each Message
                await _botService.Client.SendMessage(
                    message.Chat.Id,
                    message.Text[(commandProps.Offset + commandProps.Length)..].Trim());
                break;
            case MessageType.Photo:
            {
                // Download Photo
                var fileId = message.Photo?.LastOrDefault()?.FileId;

                if (string.IsNullOrEmpty(fileId))
                {
                    await _botService.Client.SendMessage(message.Chat.Id, "File not found");
                    break;
                }
                
                var file = await _botService.Client.GetFile(fileId);

                if (string.IsNullOrEmpty(file.FilePath))
                {
                    await _botService.Client.SendMessage(message.Chat.Id, "File not found");
                    break;
                }
                
                var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                await using (var saveImageStream = File.Open(filename, FileMode.Create))
                {
                    await _botService.Client.DownloadFile(file.FilePath, saveImageStream);
                }

                await _botService.Client.SendMessage(message.Chat.Id, "Thx for the Pics");
                break;
            }
            default:
                await _botService.Client.SendMessage(message.Chat.Id, "Message type not supported");
                break;
        }
    }

    public async Task WaterReminderAsync(ChatId chatId)
    {
        await _botService.Client.SendMessage(chatId, "Drink some water");
    }

    public async Task SendMessageAsync(ChatId chatId, string message)
    {
        await _botService.Client.SendMessage(chatId, message);
    }
}