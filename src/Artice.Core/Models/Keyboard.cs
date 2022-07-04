using System.Collections.Generic;
using Artice.Core.Models.Enums;

namespace Artice.Core.Models
{
    public class Keyboard
    {
        public Keyboard()
        {
            Buttons = new List<KeyboardButton>();
        }

        public List<KeyboardButton> Buttons { get; }

        public KeyboardType Type { get; set; }
    }
}