using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace api.archerharmony.com.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Message message, MessageEntity commandProps);
        Task WaterReminderAsync(ChatId chatId);
        Task SendMessageAsync(ChatId chatId, string message);
    }
}
