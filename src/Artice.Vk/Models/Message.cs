using System;
using Newtonsoft.Json;
using SmartHead.FIFA.ChatBots.Vkontakte.Converters;

namespace Artice.Vk.Models
{
    public class Message
    {
        [JsonProperty("conversation_message_id")]
        public long Id { get; internal set; }

        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeFromSecondsConverter))]
        public DateTime Date { get; internal set; }

        [JsonProperty("out")]
        public bool Out { get; internal set; }

        [JsonProperty("from_id")]
        public long FromId { get; internal set; }

        [JsonProperty("peer_id")]
        public long ChatId { get; internal set; }
        
        [JsonProperty("text")]
        public string Text { get; internal set; }

        [JsonProperty("fwd_messages")]
        public Message[] ForwardMessages { get; internal set; }

        [JsonProperty("important")]
        public bool Important { get; internal set; }

        [JsonProperty("random_id")]
        public long? RandomId { get; internal set; }

        [JsonProperty("geo")]
        public Location Geo { get; internal set; }

        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; internal set; }
        
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; internal set; }

        [JsonProperty("payload")]
        public string Payload { get; internal set; }
    }
}