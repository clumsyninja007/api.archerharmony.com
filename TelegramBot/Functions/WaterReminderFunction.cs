using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TelegramBot.Data;

namespace TelegramBot.Functions;

// Replaces the old external cron. Fires 9am / 1pm / 6pm in the app's configured time zone
// (set WEBSITE_TIME_ZONE; NCRONTAB is evaluated in that zone).
public class WaterReminderFunction(
    ITelegramBotClient bot,
    TelegramBotContext context,
    ILogger<WaterReminderFunction> logger)
{
    [Function("WaterReminder")]
    public async Task Run([TimerTrigger("0 0 9,13,18 * * *")] TimerInfo timer, CancellationToken ct)
    {
        var chatIds = await context.ChatTrackers
            .Where(c => c.Active && c.WaterReminder)
            .Select(c => c.ChatId)
            .ToListAsync(ct);

        foreach (var chatId in chatIds)
        {
            await bot.SendMessage(chatId, "Drink some water", cancellationToken: ct);
        }

        logger.LogInformation("Sent {Count} water reminders", chatIds.Count);
    }
}
