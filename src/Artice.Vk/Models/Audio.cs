using System;
using Newtonsoft.Json;
using SmartHead.FIFA.ChatBots.Vkontakte.Converters;

namespace Artice.Vk.Models
{
    public class Audio
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("artist")]
        public string Artist { get; internal set; }

        [JsonProperty("duration")]
        public long Duration { get; internal set; }

        [JsonProperty("url")]
        public string Url { get; internal set; }

        [JsonProperty("lyrics_id")]
        public long LyricsId { get; internal set; }

        [JsonProperty("album_id")]
        public long AlbumId { get; internal set; }

        [JsonProperty("genre_id")]
        public long GenreId { get; internal set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeFromSecondsConverter))]
        public DateTime Date { get; internal set; }

        [JsonProperty("no_search")]
        public bool NoSearch { get; internal set; }

        [JsonProperty("is_hq")]
        public bool IsHq { get; internal set; }
    }
}