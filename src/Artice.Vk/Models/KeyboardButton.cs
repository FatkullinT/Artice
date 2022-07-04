using Artice.Vk.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Artice.Vk.Models
{
    public class KeyboardButton
    {
        [JsonProperty("color")]
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyboardButtonColor Color { get; internal set; }

        [JsonProperty("action")]
        public KeyboardButtonAction Action { get; internal set; }
    }
}