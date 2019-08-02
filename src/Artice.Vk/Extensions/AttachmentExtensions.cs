using System;
using System.Collections.Generic;
using Artice.Core.Models;

namespace Artice.Vk.Extensions
{
    internal static class AttachmentExtensions
    {

        private static readonly Dictionary<Type, string> AttachmentSendParams =
            new Dictionary<Type, string>()
            {
                {
                    typeof(Image),
                    "photo"
                },
                {
                    typeof(Audio),
                    "audio"
                },
                {
                    typeof(Video),
                    "video"
                },
                {
                    typeof(Document),
                    "doc"
                }
            };

        internal static string GetAttachmentType(this Attachment attachment)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            var type = attachment.GetType();

            if (AttachmentSendParams.TryGetValue(type, out var typeName))
                return typeName;

            throw new NotSupportedException($"Not supported attachment type {type.FullName}");
        }
    }
}
