using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Place
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("latitude")]
        public double Latitude { get; internal set; }

        [JsonProperty("longitude")]
        public double Longitude { get; internal set; }

        [JsonProperty("created")]
        public long Created { get; internal set; }

        [JsonProperty("icon")]
        public string Icon { get; internal set; }

        [JsonProperty("country")]
        public string Country { get; internal set; }

        [JsonProperty("city")]
        public string City { get; internal set; }
    }
}