using System.Linq;
using System.Web;
using Artice.Core.Models.Enums;
using Artice.Vk.Models;
using Artice.Vk.Models.Enum;
using Newtonsoft.Json;
using Keyboard = Artice.Core.Models.Keyboard;
using KeyboardButton = Artice.Core.Models.KeyboardButton;

namespace Artice.Vk.Mapping
{
    public class OutgoingMessageMapper : IOutgoingMessageMapper
    {
        public Vk.Models.Keyboard Map(Keyboard src)
        {
            if (src == null)
                return null;

            return new Models.Keyboard()
            {
                OneTime = src.Type != KeyboardType.Constant,

                Buttons = src.Buttons
                    .GroupBy(key => key.RowOrder)
                    .OrderBy(keyRow => keyRow.Key)
                    .Select(keyRow => keyRow
                        .OrderBy(key => key.ColumnOrder)
                        .Select(Map)
                        .ToArray())
                    .ToArray()
            };
        }

        private Vk.Models.KeyboardButton Map(KeyboardButton src)
        {
            if (src == null)
                return null;

            return new Models.KeyboardButton()
            {
                Color = KeyboardButtonColor.Primary,
                Action = new KeyboardButtonAction()
                {
                    Type = KeyboardButtonType.Text,
                    Label = src.ButtonText,
                    Payload = JsonConvert.SerializeObject(new Payload() { Command = src.CallbackData })
                }
            };
        }
    }
}