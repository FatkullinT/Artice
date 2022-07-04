using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class StickerImage
    {
        [JsonProperty("url")]
        public string Url { get; internal set; }

        [JsonProperty("width")]
        public long Width { get; internal set; }

        [JsonProperty("height")]
        public long Height { get; internal set; }
    }
}