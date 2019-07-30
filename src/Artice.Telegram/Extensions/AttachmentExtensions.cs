using System;
using System.Collections.Generic;
using Artice.Core.Models;

namespace Artice.Telegram.Extensions
{
    internal static class AttachmentExtensions
    {

        private static readonly Dictionary<Type, AttachmentSendingParams> AttachmentSendParams =
            new Dictionary<Type, AttachmentSendingParams>()
            {
                {
                    typeof(Image),
                    new AttachmentSendingParams("sendPhoto", "photo")
                },
                {
                    typeof(Audio),
                    new AttachmentSendingParams("sendAudio", "audio")
                },
                {
                    typeof(Video),
                    new AttachmentSendingParams("sendVideo", "video")
                },
                {
                    typeof(Document),
                    new AttachmentSendingParams("sendDocument", "document")
                },
                {
                    typeof(Sticker),
                    new AttachmentSendingParams("sendSticker", "sticker")
                }
            };

        internal static AttachmentSendingParams GetSendingParams(this Attachment attachment)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            var type = attachment.GetType();

            if (AttachmentSendParams.TryGetValue(type, out var parameters))
                return parameters;

            throw new NotSupportedException($"Not supported attachment type {type.FullName}");
        }

        internal struct AttachmentSendingParams
        {
            public AttachmentSendingParams(string method, string contentFieldName)
            {
                Method = method;
                ContentFieldName = contentFieldName;
            }

            public string Method { get; set; }

            public string ContentFieldName { get; set; }
        }
    }
}
