using System.Linq;
using Artice.Core.Models;

namespace Artice.Telegram.Mapping
{
    public class OutgoingMessageMapper : IOutgoingMessageMapper
    {
        public Artice.Telegram.Models.ReplyMarkups.InlineKeyboardMarkup Map(Keyboard src)
        {
            if (src == null)
                return null;

            return new Artice.Telegram.Models.ReplyMarkups.InlineKeyboardMarkup()
            {
                InlineKeyboard = src.Buttons
                    .GroupBy(key => key.RowOrder)
                    .OrderBy(keyRow => keyRow.Key)
                    .Select(keyRow => keyRow
                        .OrderBy(key => key.ColumnOrder)
                        .Select(Map)
                        .ToArray())
                    .ToArray()
            };
        }

        private Artice.Telegram.Models.InlineKeyboardButton Map(KeyboardButton src)
        {
            if (src == null)
                return null;

            return new Artice.Telegram.Models.InlineKeyboardButton()
            {
                CallbackData = src.CallbackData,
                //Url = src.Url,
                ButtonText = src.ButtonText
            };
        }
    }
}