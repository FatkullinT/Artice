﻿using Newtonsoft.Json;

namespace Artice.Telegram.Models
{
    /// <summary>
    /// This object represents a sticker.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Sticker : File
    {
        /// <summary>
        /// Sticker width
        /// </summary>
        [JsonProperty(PropertyName = "width", Required = Required.Always)]
        public string Width { get; set; }

        /// <summary>
        /// Sticker height
        /// </summary>
        [JsonProperty(PropertyName = "height", Required = Required.Always)]
        public string Height { get; set; }

        /// <summary>
        /// Sticker thumbnail in .webp or .jpg format
        /// </summary>
        [JsonProperty(PropertyName = "thumb", Required = Required.Default)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Emoji associated with the sticker
        /// </summary>
        [JsonProperty(PropertyName = "emoji", Required = Required.Default)]
        public string Emoji { get; set; }

        /// <summary>
        /// Sticker set name
        /// </summary>
        [JsonProperty(PropertyName = "set_name", Required = Required.Default)]
        public string SetName { get; set; }

        /// <summary>
        /// Is animated sticker
        /// </summary>
        [JsonProperty(PropertyName = "is_animated", Required = Required.Default)]
        public bool IsAnimated { get; set; }
    }
}
