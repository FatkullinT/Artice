using System;
using Artice.Vk.Models.Enum;
using Newtonsoft.Json;
using SmartHead.FIFA.ChatBots.Vkontakte.Converters;

namespace Artice.Vk.Models
{
    public class Document
    {
        [JsonProperty("id")]
        public long Id { get; internal set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("size")]
        public long Size { get; internal set; }

        [JsonProperty("ext")]
        public string Extention { get; internal set; }

        [JsonProperty("url")]
        public string Url { get; internal set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeFromSecondsConverter))]
        public DateTime Date { get; internal set; }

        [JsonProperty("type")]
        public DocumentType Type { get; internal set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; internal set; }
    }
}