using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Keyboard
    {
        [JsonProperty("one_time")]
        public bool OneTime { get; set; }

        [JsonProperty("buttons")]
        public KeyboardButton[][] Buttons { get; set; }
    }
}