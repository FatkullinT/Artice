using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class UploadResponse
    {
        [JsonProperty("server")]
        public long Server { get; internal set; }

        [JsonProperty("photo")]
        public string Photo { get; internal set; }

        [JsonProperty("hash")]
        public string Hash { get; internal set; }
    }
}