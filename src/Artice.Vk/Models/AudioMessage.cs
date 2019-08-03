using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class AudioMessage
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; internal set; }

        [JsonProperty("duration")]
        public long Duration { get; internal set; }

        [JsonProperty("waveform")]
        public int[] Waveform { get; internal set; }

        [JsonProperty("link_ogg")]
        public string LinkOgg { get; internal set; }

        [JsonProperty("link_mp3")]
        public string LinkMp3 { get; internal set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; internal set; }
    }
}