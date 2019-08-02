using Artice.Vk.Models.Enum;
using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Action
    {
        [JsonProperty("type")]
        public ActionType Type { get; set; }

        [JsonProperty("member_id")]
        public long MemberId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("photo")]
        public ChatPhoto Photo { get; set; }
    }
}