using System;
using Newtonsoft.Json;
using SmartHead.FIFA.ChatBots.Vkontakte.Converters;

namespace Artice.Vk.Models
{
    public class Photo
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("album_id")]
        public long AlbumId { get; internal set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; internal set; }

        [JsonProperty("user_id")]
        public long UserId { get; internal set; }

        [JsonProperty("text")]
        public string Text { get; internal set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeFromSecondsConverter))]
        public DateTime Date { get; internal set; }

        [JsonProperty("sizes")]
        public Size[] Sizes { get; internal set; }

        [JsonProperty("width")]
        public long Width { get; internal set; }

        [JsonProperty("height")]
        public long Height { get; internal set; }
    }
}