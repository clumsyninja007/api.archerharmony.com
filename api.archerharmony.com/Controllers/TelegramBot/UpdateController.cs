using api.archerharmony.com.DbContext;
using api.archerharmony.com.Models.Telegram;
using api.archerharmony.com.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace api.archerharmony.com.Controllers.TelegramBot
{
    [Route("[controller]")]
    public class UpdateController : ControllerBase
    {
        private readonly IUpdateService _updateService;
        private readonly string _token;
        private readonly IHttpClientFactory _clientFactory;
        private readonly TelegramBotContext _context;

        public UpdateController(
            IUpdateService updateService,
            IOptions<BotConfiguration> botConfig,
            IHttpClientFactory clientFactory,
            TelegramBotContext context
            )
        {
            _updateService = updateService;
            _token = botConfig.Value.BotToken;
            _clientFactory = clientFactory;
            _context = context;
        }

        [HttpGet]
        public async Task<List<ChatTracker>> GetTrackedChanges()
        {
            return await _context.ChatTracker.ToListAsync();
        }

        [HttpPost("{token}")]
        public async Task<IActionResult> Post(string token, [FromBody]Update update)
        {
            if (token != _token)
            {
                return Unauthorized("Invalid bot token");
            }

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

                                var chat = await _context.ChatTracker.Where(c => c.ChatId == chatId.Identifier).FirstOrDefaultAsync();

                                if (chat == null)
                                {
                                    return BadRequest("Chat not found");
                                }

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
                            await _updateService.EchoAsync(message, commandProps);
                            break;
                        case "/joke":
                            {
                                var request = new HttpRequestMessage(HttpMethod.Get, "https://icanhazdadjoke.com/");
                                request.Headers.Add("Accept", "text/plain");

                                var client = _clientFactory.CreateClient();

                                var response = await client.SendAsync(request);

                                if (response.IsSuccessStatusCode)
                                {
                                    var joke = await response.Content.ReadAsStringAsync();

                                    await _updateService.SendMessageAsync(chatId, joke);
                                }
                                else
                                {
                                    await _updateService.SendMessageAsync(chatId, "Error getting joke :(");
                                }

                                client.Dispose();

                                break;
                            }
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
