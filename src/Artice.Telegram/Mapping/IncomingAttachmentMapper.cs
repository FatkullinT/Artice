using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Telegram.Files;
using Artice.Telegram.Models.Enums;

namespace Artice.Telegram.Mapping
{
    public class IncomingAttachmentMapper : IIncomingAttachmentMapper
    {
        private readonly Func<ITelegramHttpClient> _clientConstructor;

        public IncomingAttachmentMapper(Func<ITelegramHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public IEnumerable<Attachment> Map(Telegram.Models.Message src)
        {
            switch (src?.Type)
            {
                case MessageType.PhotoMessage:
                    var maxSize = src.Photo.Max(p => p.Height * p.Width);
                    var maxSizePhoto = src.Photo.FirstOrDefault(photo => photo.Height * photo.Width == maxSize);
                    if (maxSizePhoto != null)
                    {
                        yield return new Image()
                        {
                            File = CreateTelegramFile(maxSizePhoto.FileId)
                        };
                    }
                    break;

                case MessageType.AudioMessage:
                    yield return new Audio()
                    {
                        File = CreateTelegramFile(src.Audio.FileId, src.Audio.MimeType)
                    };
                    break;

                case MessageType.VideoMessage:
                    yield return new Video()
                    {
                        File = CreateTelegramFile(src.Video.FileId, src.Video.MimeType)
                    };
                    break;

                case MessageType.VoiceMessage:
                    yield return new Audio()
                    {
                        File = CreateTelegramFile(src.Voice.FileId, src.Voice.MimeType)
                    };
                    break;

                case MessageType.DocumentMessage:
                    yield return new Document()
                    {
                        File = CreateTelegramFile(src.Document.FileId, src.Document.MimeType, src.Document.FileName),
                        Extention = Path.GetExtension(src.Document.FileName)
                    };
                    break;

                case MessageType.StickerMessage:
                    yield return new Sticker()
                    {
                        StickerId = src.Sticker.Emoji,
                        File = CreateTelegramFile(src.Sticker.FileId)
                    };
                    break;
            }
        }

        private IIncomingFile CreateTelegramFile(string fileId, string mimeType = null, string fileName = null)
        {
            return new TelegramIncomingFile(_clientConstructor)
            {
                FileId = fileId,
                MimeType = mimeType,
                Name = fileName
            };
        }
    }
}