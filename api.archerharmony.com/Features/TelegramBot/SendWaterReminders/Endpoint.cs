using api.archerharmony.com.Services;
using Microsoft.Extensions.Options;

namespace api.archerharmony.com.Features.TelegramBot.SendWaterReminders;

public class Endpoint(
    IOptions<BotConfiguration> botConfig,
    TelegramBotContext context,
    IUpdateService updateService)
    : Endpoint<Request>
{
    public override void Configure()
    {
        Post("update/{token}/water-reminder");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (req.Token != botConfig.Value.BotToken)
        {
            await SendUnauthorizedAsync(ct);
        }

        var reminderList = await context.ChatTracker
            .Where(c => c.Active)
            .Where(c => c.WaterReminder)
            .ToListAsync(ct);

        reminderList.ForEach(r => updateService.WaterReminderAsync(r.ChatId));
    }
}