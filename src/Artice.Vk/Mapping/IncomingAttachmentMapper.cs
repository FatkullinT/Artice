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
                        File = CreateVkFile(maxPhoto.Src, src.Photo.Id, src.Photo.OwnerId)
                    };

                case AttachmentType.Video:
                    return new Video()
                    {
                        File = GetVideoFile(src.Video)
                    };

                case AttachmentType.Audio:
                    return new Audio()
                    {
                        File = CreateVkFile(src.Audio.Url, src.Audio.Id, src.Audio.OwnerId)
                    };

                case AttachmentType.Document:
                    return new Document()
                    {
                        File = CreateVkFile(src.Document.Url, src.Document.Id, src.Document.OwnerId, src.Document.Title),
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
                        File = CreateVkFile(maxSticker.Url, src.Sticker.Id)
                    };

                default:
                    return null;
            }
        }

        private IFile GetVideoFile(Models.Video src)
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

                return CreateVkFile(fileUrl, src.Id, src.OwnerId);
            }

            if (src.Player != null)
                new VkIncomingPlayer(src.Id, src.OwnerId) { PlayerUri = new Uri(src.Player) };

            return new VkIncomingFile(src.Id, src.OwnerId);
        }

        private VkIncomingWebFile CreateVkFile(string uri, long fileId, long ownerId = 0, string fileName = null)
        {
            return new VkIncomingWebFile(_clientConstructor, fileId, ownerId)
            {
                Uri = new Uri(uri),
                FileName = fileName ?? Path.GetFileName(uri)
            };
        }
    }
}