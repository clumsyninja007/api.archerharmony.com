using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using TelegramBot.Configuration;
using TelegramBot.Services;

namespace TelegramBot.Functions;

// Telegram webhook. The {token} path segment is the shared secret (set as the webhook URL via
// setWebhook); it must equal the bot token, otherwise the request is rejected.
public class UpdateFunction(
    IOptions<BotConfiguration> botConfig,
    ITelegramUpdateHandler handler,
    ILogger<UpdateFunction> logger)
{
    [Function("Update")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "update/{token}")] HttpRequest req,
        string token,
        CancellationToken ct)
    {
        if (token != botConfig.Value.BotToken)
        {
            return new UnauthorizedResult();
        }

        using var reader = new StreamReader(req.Body);
        var body = await reader.ReadToEndAsync(ct);

        Update? update;
        try
        {
            update = JsonSerializer.Deserialize<Update>(body);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to parse Telegram update");
            return new BadRequestResult();
        }

        if (update is null)
        {
            return new BadRequestResult();
        }

        await handler.HandleAsync(update, ct);
        return new OkResult();
    }
}
