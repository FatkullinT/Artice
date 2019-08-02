using System;
using System.IO;
using System.Linq;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Vk.Files;
using Artice.Vk.HttpClients;
using Artice.Vk.Models.Enum;

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
                    var photoMaxSize = src.Photo.Sizes.Max(p => p.Height * p.Width);
                    var maxPhoto = src.Photo.Sizes.First(photo => photo.Height * photo.Width == photoMaxSize);
                    return new Image()
                    {
                        File = CreateVkFile(maxPhoto.Url, src.Photo.Id, src.Photo.OwnerId, src.Photo.AccessKey)
                    };

                case AttachmentType.Video:
                    return new Video()
                    {
                        File = GetVideoFile(src.Video, ConvertFileName(src.Video.Title, 75))
                    };

                case AttachmentType.Audio:
                    string fileName = null;

                    if (!string.IsNullOrWhiteSpace(src.Audio.Artist) && !string.IsNullOrWhiteSpace(src.Audio.Title))
                        fileName = $"{ConvertFileName(src.Audio.Artist, 50)}-{ConvertFileName(src.Audio.Title, 50)}";

                    return new Audio()
                    {
                        File = CreateVkFile(src.Audio.Url, src.Audio.Id, src.Audio.OwnerId, src.Audio.AccessKey, fileName)
                    };

                case AttachmentType.Document:
                    return new Document()
                    {
                        File = CreateVkFile(src.Document.Url, src.Document.Id, src.Document.OwnerId, src.Document.AccessKey, src.Document.Title),
                        Extention = src.Document.Extention != null ? $".{src.Document.Extention.Trim('.')}" : null
                    };

                case AttachmentType.Sticker:
                    var stickerMaxSize = src.Sticker.Images.Max(p => p.Height * p.Width);
                    var maxSticker = src.Sticker.Images.First(photo => photo.Height * photo.Width == stickerMaxSize);
                    return new Sticker()
                    {
                        ChannelId = Consts.ChannelId,
                        StickerId = src.Sticker.Id.ToString(),
                        CollectionId = src.Sticker.ProductId.ToString(),
                        File = CreateVkFile(maxSticker.Url, src.Sticker.Id, 0, null)
                    };

                default:
                    return null;
            }
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

        private IFile GetVideoFile(Models.Video src, string fileName)
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

                return CreateVkFile(fileUrl, src.Id, src.OwnerId, src.AccessKey, fileName);
            }

            if (src.Player != null)
                return new VkIncomingPlayer(src.Id, src.OwnerId, src.AccessKey) { PlayerUri = new Uri(src.Player), FileName = fileName };

            return new VkIncomingFile(src.Id, src.OwnerId, src.AccessKey) {FileName = fileName };
        }

        private VkIncomingWebFile CreateVkFile(string url, long fileId, long ownerId, string accessKey, string fileName = null)
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

            return new VkIncomingWebFile(_clientConstructor, fileId, ownerId, accessKey)
            {
                Uri = uri,
                FileName = fileName
            };
        }
    }
}