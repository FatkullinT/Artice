using System.Collections.Generic;
using System.IO;
using System.Linq;
using Artice.Core.Models;
using Artice.Telegram.Models.Enums;
using AutoMapper;
using Message = Artice.Telegram.Models.Message;

namespace Artice.Telegram.MapConfig
{
    public class TelegramAttachmentMap : ITypeConverter<Models.Message, Attachment[]>
    {
        public Attachment[] Convert(Message source, Attachment[] destination, ResolutionContext context)
        {
            var result = new List<Attachment>();
            switch (source.Type)
            {
                case MessageType.VoiceMessage:
                {
                    result.Add(new Audio()
                    {
                        File = new FileReference(source.Voice.FileId)
                    });
                    break;
                }
                case MessageType.AudioMessage:
                {
                    result.Add(new Audio()
                    {
                        File = new FileReference(source.Audio.FileId)
                    });
                    break;
                }
                case MessageType.VideoMessage:
                {
                    result.Add(new Video()
                    {
                        File = new FileReference(source.Video.FileId)
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
                            File = new FileReference(maxSizePhoto.FileId)
                        });
                    }
                    break;
                }
                case MessageType.DocumentMessage:
                {
                    result.Add(new Document()
                    {
                        File = new FileReference(source.Document.FileId) { FileName = source.Document.FileName },
                        Extention = Path.GetExtension(source.Document.FileName)
                    });
                    break;
                }
                case MessageType.StickerMessage:
                {
                    result.Add(new Sticker()
                    {
                        StickerId = source.Sticker.Emoji,
                        File = new FileReference(source.Sticker.FileId)
                    });
                    break;
                }
            }
            return result.ToArray();
        }
    }
}