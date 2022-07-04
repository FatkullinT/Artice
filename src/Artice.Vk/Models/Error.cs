using Newtonsoft.Json;

namespace Artice.Vk.Models
{
    public class Error
    {
        [JsonProperty("error_code", Required = Required.Default)]
        public int Code { get; set; }

        [JsonProperty("error_msg", Required = Required.Default)]
        public string Message { get; set; }
    }
}