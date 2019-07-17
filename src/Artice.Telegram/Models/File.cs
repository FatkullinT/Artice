using Newtonsoft.Json;

namespace Artice.Telegram.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class File
    {
        /// <summary>
        /// Unique identifier for this file
        /// </summary>
        [JsonProperty(PropertyName = "file_id", Required = Required.Always)]
        public string FileId { get; set; }

        /// <summary>
        /// Optional. FileReference size, if known
        /// </summary>
        [JsonProperty(PropertyName = "file_size", Required = Required.Default)]
        public int FileSize { get; set; }

        /// <summary>
        /// FileReference path. 
        /// </summary>
        [JsonProperty(PropertyName = "file_path", Required = Required.Default)]
        public string FilePath { get; set; }
    }
}
