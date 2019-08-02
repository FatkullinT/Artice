using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Artice.Vk.Models
{
    public class Payload
    {
        [JsonProperty("command")]
        public string Command { get; internal set; }
    }
}