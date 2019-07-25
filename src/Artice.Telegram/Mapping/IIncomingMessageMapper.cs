using Artice.Core.Models;

namespace Artice.Telegram.Mapping
{
    public interface IIncomingMessageMapper
    {
        IncomingMessage Map(Telegram.Models.Message message);
        IncomingMessage Map(Telegram.Models.CallbackQuery callbackQuery);
    }
}