using Telegram.Bot;
using Telegram.Bot.Types;

namespace api.archerharmony.com.Features.TelegramBot.Shared;

public record SendInvalidCommandMessage(ChatId ChatId) : ICommand;

public class SendInvalidCommandMessageHandler(TelegramBotClient client) : ICommandHandler<SendInvalidCommandMessage>
{
    public Task ExecuteAsync(SendInvalidCommandMessage command, CancellationToken ct)
    {
        return client.SendMessage(command.ChatId, "Invalid command", cancellationToken: ct);
    }
}