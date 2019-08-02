using Artice.Vk.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Artice.Vk.Models
{
    public class KeyboardButtonAction
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyboardButtonType Type { get; internal set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; internal set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public string Payload { get; internal set; }

        [JsonProperty("app_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long AppId { get; internal set; }

        [JsonProperty("owner_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long OwnerId { get; internal set; }

        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash { get; internal set; }
    }
}