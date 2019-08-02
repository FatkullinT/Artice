using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Size
    {
        [JsonProperty("url")]
        public string Url { get; internal set; }

        [JsonProperty("width")]
        public long Width { get; internal set; }

        [JsonProperty("height")]
        public long Height { get; internal set; }

        [JsonProperty("type")]
        public string Type { get; internal set; }
    }
}