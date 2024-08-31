using api.archerharmony.com.Services;
using Telegram.Bot.Types;

namespace api.archerharmony.com.Features.TelegramBot.Shared;

public record SendInvalidCommandMessage(ChatId ChatId) : ICommand;

public class SendInvalidCommandMessageHandler(IUpdateService updateService) : ICommandHandler<SendInvalidCommandMessage>
{
    public Task ExecuteAsync(SendInvalidCommandMessage command, CancellationToken ct)
    {
        return updateService.SendMessageAsync(command.ChatId, "Invalid command");
    }
}