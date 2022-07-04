using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Sticker
    {
        [JsonProperty("sticker_id")]
        public long Id { get; internal set; }

        [JsonProperty("product_id")]
        public long ProductId { get; internal set; }

        [JsonProperty("images")]
        public StickerImage[] Images { get; internal set; }

        [JsonProperty("images_with_background ")]
        public StickerImage[] ImagesWithBackground { get; internal set; }
    }
}