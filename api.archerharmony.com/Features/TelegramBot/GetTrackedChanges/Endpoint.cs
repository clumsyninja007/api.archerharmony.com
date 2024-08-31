using api.archerharmony.com.Models.Telegram;

namespace api.archerharmony.com.Features.TelegramBot.GetTrackedChanges;

public class Endpoint(TelegramBotContext context) : EndpointWithoutRequest<List<ChatTracker>>
{
    public override void Configure()
    {
        Get("update");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = await context.ChatTracker.ToListAsync(ct);
    }
}