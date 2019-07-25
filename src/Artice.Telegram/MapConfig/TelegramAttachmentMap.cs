using System.Collections.Generic;
using System.IO;
using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Models.Enums;
using AutoMapper;
using Message = Artice.Telegram.Models.Message;
using System;
using Artice.Telegram.Files;

namespace Artice.Telegram.MapConfig
{
    public class TelegramAttachmentMap : ITypeConverter<Models.Message, Attachment[]>
    {
        private readonly Func<ITelegramHttpClient> _clientConstructor;

        public TelegramAttachmentMap(Func<ITelegramHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public TelegramAttachmentMap()
        {
        }

        public Attachment[] Convert(Message source, Attachment[] destination, ResolutionContext context)
        {
            var result = new List<Attachment>();
            switch (source.Type)
            {
                case MessageType.VoiceMessage:
                {
                    result.Add(new Audio()
                    {
                        File = CreateTelegramFile(source.Voice.FileId, source.Voice.MimeType)
                    });
                    break;
                }
                case MessageType.AudioMessage:
                {
                    result.Add(new Audio()
                    {
                        File = CreateTelegramFile(source.Audio.FileId, source.Audio.MimeType)
                    });
                    break;
                }
                case MessageType.VideoMessage:
                {
                    result.Add(new Video()
                    {
                        File = CreateTelegramFile(source.Video.FileId, source.Video.MimeType)
                    });
                    break;
                }
                case MessageType.PhotoMessage:
                {
                    var maxSize = source.Photo.Max(p => p.Height * p.Width);
                    var maxSizePhoto = source.Photo.FirstOrDefault(photo => photo.Height * photo.Width == maxSize);
                    if (maxSizePhoto != null)
                    {
                        result.Add(new Image()
                        {
                            File = CreateTelegramFile(maxSizePhoto.FileId)
                        });
                    }
                    break;
                }
                case MessageType.DocumentMessage:
                {
                    result.Add(new Document()
                    {
                        File = CreateTelegramFile(source.Document.FileId, source.Document.MimeType, source.Document.FileName),
                        Extention = Path.GetExtension(source.Document.FileName)
                    });
                    break;
                }
                case MessageType.StickerMessage:
                {
                    result.Add(new Sticker()
                    {
                        StickerId = source.Sticker.Emoji,
                        File = CreateTelegramFile(source.Sticker.FileId)
                    });
                    break;
                }
            }
            return result.ToArray();
        }

        private IFile CreateTelegramFile(string fileId, string mimeType = null, string fileName = null)
        {
            return new TelegramFile(_clientConstructor)
            {
                FileId = fileId,
                MimeType = mimeType,
                Name = fileName
            };
        }
    }
}