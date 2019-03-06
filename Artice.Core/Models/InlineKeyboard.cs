using System.Collections.Generic;

namespace Artice.Core.Models
{
    public class InlineKeyboard
    {
        public InlineKeyboard()
        {
            Buttons = new List<KeyboardButton>();
        }

        public List<KeyboardButton> Buttons { get; }
    }
}