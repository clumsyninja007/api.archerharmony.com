using api.archerharmony.com.DbContext;
using api.archerharmony.com.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace api.archerharmony.com.Controllers
{
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {
        private readonly IUpdateService _updateService;
        private readonly string _token;
        private readonly ILogger<UpdateController> _logger;
        private readonly TelegramBotContext _context;

        public UpdateController(IUpdateService updateService, IOptions<BotConfiguration> botConfig, ILogger<UpdateController> logger, TelegramBotContext context)
        {
            _updateService = updateService;
            _token = botConfig.Value.BotToken;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Test");
        }

        [HttpPost("{token}")]
        public async Task<IActionResult> Post(string token, [FromBody]Update update)
        {
            if (token != _token)
            {
                return Unauthorized("Invalid bot token");
            }

            _logger.LogInformation(JsonSerializer.Serialize(update));

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
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
                                        InvalidCommand(chatId);
                                        return Ok();
                            }

                            var chat = await _context.ChatTracker.FindAsync(chatId);

                            if (chat.WaterReminder != value)
                            {
                                chat.WaterReminder = value;
                                await _context.SaveChangesAsync();
                                await _updateService.SendMessageAsync(chatId, $"Water notifications {msgText}");
                            }
                            else
                            {
                                await _updateService.SendMessageAsync(chatId, $"Water notifications already {msgText}");
                            }

                            break;
                        }
                        case "/echo":
                            await _updateService.EchoAsync(update);
                            break;
                        default:
                            InvalidCommand(chatId);
                            break;
                    }
                }
            }
            
            return Ok();
        }

        [HttpPost("{token}/water-reminder")]
        public async Task<IActionResult> Post(string token)
        {
            if (token != _token)
            {
                return Unauthorized("Invalid bot token");
            }

            var reminderList = await _context.ChatTracker
                .Where(c => c.Active)
                .Where(c => c.WaterReminder)
                .ToListAsync();

            reminderList.ForEach(r => _updateService.WaterReminderAsync(r.ChatId));

            return Ok();
        }

        public async void InvalidCommand(ChatId chatId)
        {
            await _updateService.SendMessageAsync(chatId, "Invalid command");
        }
    }
}
