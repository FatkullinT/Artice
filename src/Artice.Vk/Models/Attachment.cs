using Artice.Vk.Models.Enum;
using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Attachment
    {
        [JsonProperty("type")]
        public AttachmentType Type { get; internal set; }

        [JsonProperty("photo")]
        public Photo Photo { get; internal set; }

        [JsonProperty("video")]
        public Video Video { get; internal set; }

        [JsonProperty("audio")]
        public Audio Audio { get; internal set; }

        [JsonProperty("doc")]
        public Document Document { get; internal set; }

        [JsonProperty("sticker")]
        public Sticker Sticker { get; internal set; }
    }
}