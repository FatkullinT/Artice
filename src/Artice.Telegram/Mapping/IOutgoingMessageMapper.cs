using Artice.Core.Models;

namespace Artice.Telegram.Mapping
{
    public interface IOutgoingMessageMapper
    {
        Artice.Telegram.Models.ReplyMarkups.InlineKeyboardMarkup Map(InlineKeyboard src);
    }
}