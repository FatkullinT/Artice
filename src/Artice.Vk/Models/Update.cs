using Artice.Vk.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Artice.Vk.Models
{
    public class Update
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UpdateType Type { get; internal set; }

        [JsonProperty("object")]
        public Message Object { get; internal set; }

        [JsonProperty("group_id")]
        public long GroupId { get; internal set; }
    }
}