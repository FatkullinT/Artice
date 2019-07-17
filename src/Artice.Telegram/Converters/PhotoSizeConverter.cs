using System;
using Artice.Telegram.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Artice.Telegram.Converters
{
    internal class PhotoSizeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => JObject.FromObject(value).WriteTo(writer);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            if (!jObject.HasValues) return null;

            var photoSize = new PhotoSize();
            serializer.Populate(jObject.CreateReader(), photoSize);
            return photoSize;
        }

        public override bool CanConvert(Type objectType)
            => (typeof(PhotoSize) == objectType);

    }
}
