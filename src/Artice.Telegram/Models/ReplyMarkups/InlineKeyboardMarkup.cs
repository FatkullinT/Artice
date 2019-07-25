using Newtonsoft.Json;

namespace Artice.Telegram.Models.ReplyMarkups
{
    /// <summary>
    /// This object represents an inline keyboard that appears right next to the <see cref="Message"/> it belongs to.
    /// </summary>
    /// <remarks>
    /// Inline keyboards are currently being tested and are not available in channels yet. For now, feel free to use them in one-on-one chats or groups.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineKeyboardMarkup : IReplyMarkup
    {
        /// <summary>
        /// Array of <see cref="InlineKeyboardButton"/> rows, each represented by an Array of <see cref="InlineKeyboardButton"/>.
        /// </summary>
        [JsonProperty("inline_keyboard", Required = Required.Always)]
        public InlineKeyboardButton[][] InlineKeyboard { get; set; }
    }
}
