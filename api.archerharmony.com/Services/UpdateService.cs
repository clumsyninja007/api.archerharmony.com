using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace api.archerharmony.com.Services
{
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
            _logger.LogInformation("Received Message from {0}", message.Chat.Id);

            switch (message.Type)
            {
                case MessageType.Text:
                    // Echo each Message
                    await _botService.Client.SendTextMessageAsync(
                        message.Chat.Id,
                        message.Text.Substring(commandProps.Offset + commandProps.Length).Trim());
                    break;
                case MessageType.Photo:
                    {
                        // Download Photo
                        var fileId = message.Photo.LastOrDefault()?.FileId;
                        var file = await _botService.Client.GetFileAsync(fileId);

                        var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                        await using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                        {
                            await _botService.Client.DownloadFileAsync(file.FilePath, saveImageStream);
                        }

                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
                        break;
                    }
                default:
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Message type not supported");
                    break;
            }
        }

        public async Task WaterReminderAsync(ChatId chatId)
        {
            await _botService.Client.SendTextMessageAsync(chatId, "Drink some water");
        }

        public async Task SendMessageAsync(ChatId chatId, string message)
        {
            await _botService.Client.SendTextMessageAsync(chatId, message);
        }
    }
}
