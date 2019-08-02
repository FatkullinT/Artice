using Artice.Vk.Models.Enum;
using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class LongPollingResponse
    {
        [JsonProperty("ts")]
        public string Cursor { get; set; }

        [JsonProperty("updates")]
        public Update[] Updates { get; set; }

        [JsonProperty("failed")]
        public LongPollingFail Failed { get; set; }
    }
}