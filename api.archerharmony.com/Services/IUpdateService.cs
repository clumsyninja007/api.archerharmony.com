using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace api.archerharmony.com.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
        Task WaterReminderAsync(ChatId chatId);
        Task SendMessageAsync(ChatId chatId, string message);
    }
}
