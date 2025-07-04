using System.Text.Json;
using Telegram.Bot.Types;

namespace api.archerharmony.com.Features.TelegramBot.PostMessage;

public record Request
{
    public string Token { get; init; } = null!;

    [FromBody] public string UpdateJson { get; init; } = null!;

    /// <summary>
    /// Helper property to get the actual Update object
    /// </summary>
    /// <remarks>
    /// FastEndpoint generator was failing to deserialize due to an ambiguous reference to InlineKeyboardMarkup
    /// </remarks>
    public Update Update => JsonSerializer.Deserialize<Update>(UpdateJson)!;
}