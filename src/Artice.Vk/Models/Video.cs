using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SmartHead.FIFA.ChatBots.Vkontakte.Converters;

namespace Artice.Vk.Models
{
    public class Video
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("description")]
        public string Description { get; internal set; }

        [JsonProperty("duration")]
        public long Duration { get; internal set; }

        [JsonProperty("photo_130")]
        public string Photo130 { get; internal set; }

        [JsonProperty("photo_320")]
        public string Photo320 { get; internal set; }

        [JsonProperty("photo_640")]
        public string Photo640 { get; internal set; }

        [JsonProperty("photo_800")]
        public string Photo800 { get; internal set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeFromSecondsConverter))]
        public DateTime Date { get; internal set; }

        [JsonProperty("adding_date")]
        [JsonConverter(typeof(DateTimeFromSecondsConverter))]
        public DateTime AddingDate { get; internal set; }

        [JsonProperty("views")]
        public long Views { get; internal set; }

        [JsonProperty("comments")]
        public long Comments { get; internal set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; internal set; }

        [JsonProperty("player")]
        public string Player { get; internal set; }

        [JsonProperty("processing")]
        public bool Processing { get; internal set; }

        [JsonProperty("live")]
        public bool Live { get; internal set; }

        [JsonProperty("files")]
        public Dictionary<string, string> Files { get; internal set; }
    }
}