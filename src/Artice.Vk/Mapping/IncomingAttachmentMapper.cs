using System;
using System.IO;
using System.Linq;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Vk.Files;
using Artice.Vk.HttpClients;
using Artice.Vk.Models;
using Artice.Vk.Models.Enum;
using Attachment = Artice.Core.Models.Attachment;
using Audio = Artice.Core.Models.Audio;
using Document = Artice.Core.Models.Document;
using Sticker = Artice.Core.Models.Sticker;
using Video = Artice.Core.Models.Video;

namespace Artice.Vk.Mapping
{
    public class IncomingAttachmentMapper : IIncomingAttachmentMapper
    {
        private readonly Func<IVkHttpClient> _clientConstructor;

        public IncomingAttachmentMapper(Func<IVkHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public Attachment Map(Models.Attachment src)
        {
            switch (src.Type)
            {
                case AttachmentType.Photo:
                    return Map(src.Photo);
                case AttachmentType.Video:
                    return Map(src.Video);
                case AttachmentType.Audio:
                    return Map(src.Audio);
                case AttachmentType.AudioMessage:
                    return Map(src.AudioMessage);
                case AttachmentType.Document:
                    return Map(src.Document);
                case AttachmentType.Sticker:
                    return Map(src.Sticker);
                default:
                    return null;
            }
        }

        public Sticker Map(Models.Sticker src)
        {
            var stickerMaxSize = src.Images.Max(p => p.Height * p.Width);
            var maxSticker = src.Images.First(photo => photo.Height * photo.Width == stickerMaxSize);
            return new Sticker()
            {
                ChannelId = Consts.ChannelId,
                StickerId = src.Id.ToString(),
                CollectionId = src.ProductId.ToString(),
                File = new FileByUri(new Uri(maxSticker.Url))
            };
        }

        public Document Map(Models.Document src)
        {
            return new Document()
            {
                File = CreateVkFile(AttachmentTypeNames.Document, src.Url, src.Id, src.OwnerId, src.AccessKey, src.Title),
                Extension = src.Extension != null ? $".{src.Extension.Trim('.')}" : null
            };
        }

        public Audio Map(Models.AudioMessage src)
        {
            return new Audio()
            {
                File = new FileByUri(new Uri(src.LinkMp3))
            };
        }

        public Audio Map(Models.Audio src)
        {
            string fileName = null;

            if (!string.IsNullOrWhiteSpace(src.Artist) && !string.IsNullOrWhiteSpace(src.Title))
                fileName = $"{ConvertFileName(src.Artist, 50)}-{ConvertFileName(src.Title, 50)}";

            return new Audio()
            {
                File = CreateVkFile(AttachmentTypeNames.Audio, src.Url, src.Id, src.OwnerId, src.AccessKey, fileName)
            };
        }

        public Video Map(Models.Video src)
        {
            return new Video()
            {
                File = GetVideoFile(src, AttachmentTypeNames.Video, ConvertFileName(src.Title, 75))
            };
        }

        public Image Map(Models.Photo src)
        {
            var photoMaxSize = src.Sizes.Max(p => p.Height * p.Width);
            var maxPhoto = src.Sizes.First(photo => photo.Height * photo.Width == photoMaxSize);
            return new Image()
            {
                File = CreateVkFile(AttachmentTypeNames.Photo, maxPhoto.Url, src.Id, src.OwnerId, src.AccessKey)
            };
        }

        private string ConvertFileName(string src, int maxLength)
        {
            if (string.IsNullOrEmpty(src))
                return null;

            return (src.Length > maxLength ? src.Substring(0, maxLength) : src)
                .Replace(' ', '_')
                .Replace('/', '_')
                .Replace('\\', '_')
                .Replace(':', '_');
        }

        private IFile GetVideoFile(Models.Video src, string fileType, string fileName)
        {
            if (src.Files != null && src.Files.Any())
            {
                var maxSize = src.Files.Keys.Max(key =>
                {
                    if (int.TryParse(key.Split('_').Last(), out int size))
                        return size;

                    return 0;
                });

                string fileUrl = null;

                if (maxSize > 0)
                {
                    var keySuffix = $"_{maxSize}";
                    fileUrl = src.Files.FirstOrDefault(pair => pair.Key.EndsWith(keySuffix)).Value;
                }

                if (string.IsNullOrEmpty(fileUrl))
                    fileUrl = src.Files.First().Value;

                return CreateVkFile(fileType, fileUrl, src.Id, src.OwnerId, src.AccessKey, fileName);
            }

            if (src.Player != null)
                return new VkIncomingPlayer(fileType, src.Id, src.OwnerId, src.AccessKey) { PlayerUri = new Uri(src.Player), FileName = fileName };

            return new VkIncomingFile(fileType, src.Id, src.OwnerId, src.AccessKey) {FileName = fileName };
        }

        private VkIncomingWebFile CreateVkFile(string fileType,string url, long fileId, long ownerId, string accessKey, string fileName = null)
        {
            var uri = new Uri(url);

            if (fileName == null)
            {
                fileName = Path.GetFileName(uri.AbsolutePath);
            }
            else if (!Path.HasExtension(fileName))
            {
                fileName = $"{fileName}{Path.GetExtension(uri.AbsolutePath)}";
            }

            return new VkIncomingWebFile(_clientConstructor, fileType, fileId, ownerId, accessKey)
            {
                Uri = uri,
                FileName = fileName
            };
        }
    }
}