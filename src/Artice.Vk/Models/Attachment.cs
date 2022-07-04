using Artice.Vk.Models.Enum;
using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Attachment
    {
        [JsonProperty("type")]
        public AttachmentType Type { get; internal set; }

        [JsonProperty(AttachmentTypeNames.Photo)]
        public Photo Photo { get; internal set; }

        [JsonProperty(AttachmentTypeNames.Video)]
        public Video Video { get; internal set; }

        [JsonProperty(AttachmentTypeNames.Audio)]
        public Audio Audio { get; internal set; }

        [JsonProperty(AttachmentTypeNames.AudioMessage)]
        public AudioMessage AudioMessage { get; internal set; }

        [JsonProperty(AttachmentTypeNames.Document)]
        public Document Document { get; internal set; }

        [JsonProperty(AttachmentTypeNames.Sticker)]
        public Sticker Sticker { get; internal set; }
    }
}