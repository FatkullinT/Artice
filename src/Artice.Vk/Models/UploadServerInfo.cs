using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Artice.Vk.Models
{
    public class UploadServerInfo
    {
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }

        [JsonProperty("album_id")]
        public long AlbumId { get; set; }

        [JsonProperty("group_id")]
        public long GroupId { get; set; }
    }
}