using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Artice.Vk.Models
{
    public class LongPoolServerInfo
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("ts")]
        public string Cursor { get; set; }
    }
}