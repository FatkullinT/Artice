using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class ChatPhoto
    {
        [JsonProperty("photo_50")]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100")]
        public string Photo100 { get; set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; set; }
    }
}