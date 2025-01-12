using Telegram.Bot;
using Telegram.Bot.Types;

namespace api.archerharmony.com.Features.TelegramBot.SendWaterReminders;

public interface IService
{
    Task WaterReminderAsync(ChatId chatId);
}

[RegisterService<IService>(LifeTime.Scoped)]
public class Service(TelegramBotClient client) : IService
{
    public async Task WaterReminderAsync(ChatId chatId)
    {
        await client.SendMessage(chatId, "Drink some water");
    }
}