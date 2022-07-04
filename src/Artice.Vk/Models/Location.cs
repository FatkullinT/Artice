using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Location
    {
        [JsonProperty("type")]
        public string Type { get; internal set; }

        [JsonProperty("coordinates")]
        public string Coordinates { get; internal set; }

        [JsonProperty("place")]
        public Place Place { get; internal set; }

    }
}