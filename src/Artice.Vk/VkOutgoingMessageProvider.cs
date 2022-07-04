using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Core.OutgoingMessages;
using Artice.Vk.Extensions;
using Artice.Vk.Files;
using Artice.Vk.HttpClients;
using Artice.Vk.Mapping;
using Artice.Vk.Models;
using Newtonsoft.Json;
using Attachment = Artice.Core.Models.Attachment;
using Sticker = Artice.Core.Models.Sticker;

namespace Artice.Vk
{
    public class VkOutgoingMessageProvider : IOutgoingMessageProvider
    {
        private readonly Func<IVkHttpClient> _clientConstructor;
        private readonly Func<IVkUploadHttpClient> _uploadClientConstructor;
        private readonly IOutgoingMessageMapper _mapper;
        private readonly IIncomingAttachmentMapper _attachmentMapper;

        public string ChannelId => Consts.ChannelId;

        public VkOutgoingMessageProvider(
            Func<IVkHttpClient> clientConstructor,
            Func<IVkUploadHttpClient> uploadClientConstructor,
            IOutgoingMessageMapper mapper,
            IIncomingAttachmentMapper attachmentMapper)
        {
            _clientConstructor = clientConstructor;
            _mapper = mapper;
            _attachmentMapper = attachmentMapper;
            _uploadClientConstructor = uploadClientConstructor;
        }

        public async Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            var recipientId = message.Group != null ? message.Group.Id : message.To.Id;
            var additionalParameters = new Dictionary<string, string>
            {
                {"random_id", GetRandomId()},
                {"peer_id", recipientId}
            };

            if (message.Attachments != null && message.Attachments.Any())
                additionalParameters.Add("attachment", await PrepareAttachments(message.Attachments, recipientId, cancellationToken));

            if (message.Keyboard != null)
                additionalParameters.Add("keyboard", JsonConvert.SerializeObject(_mapper.Map(message.Keyboard)));

            if (message.Group == null)
                additionalParameters.Add("user_id", message.To.Id);

            if (!string.IsNullOrEmpty(message.Text))
                additionalParameters.Add("message", message.Text);

            await _clientConstructor().PostAsync<int>("messages.send", additionalParameters, cancellationToken: cancellationToken);
        }

        private async Task<string> PrepareAttachments(Attachment[] attachments, string peerId, CancellationToken cancellationToken)
        {
            UploadServerInfo uploadServerInfo = null;
            var keys = new List<string>(attachments.Length);

            foreach (var attachment in attachments)
            {
                if (attachment is Sticker)
                    continue;

                var file = attachment.File;

                if (file is OutgoingMultiTypeFile multiTypeFile)
                    file = multiTypeFile.Prefer<VkIncomingFile>();
                

                if (file is VkIncomingFile vkIncomingFile)
                {
                    keys.Add(CreateFileKey(vkIncomingFile));
                    continue;
                }

                if (attachment is Image)
                {
                    if (uploadServerInfo == null)
                    {
                        var parameters = new Dictionary<string, object>() { { "peer_id", peerId } };
                        uploadServerInfo = (await _clientConstructor()
                            .GetAsync<UploadServerInfo>("photos.getMessagesUploadServer", parameters,
                                cancellationToken)).Response;
                    }

                    var uploadResponse = await _uploadClientConstructor()
                        .PostAsync(uploadServerInfo.UploadUrl, "photo", file, cancellationToken);

                    var saveParameters = new Dictionary<string, string>()
                    {
                        {"server", uploadResponse.Server.ToString(CultureInfo.InvariantCulture)},
                        {"photo", uploadResponse.Photo},
                        {"hash", uploadResponse.Hash}
                    };

                    var resultPhotos = (await _clientConstructor()
                        .PostAsync<Photo[]>("photos.saveMessagesPhoto", saveParameters, cancellationToken: cancellationToken)).Response;

                    foreach (var resultPhoto in resultPhotos)
                    {
                        var resultImage = _attachmentMapper.Map(resultPhoto);
                        attachment.File = new OutgoingMultiTypeFile(attachment.File, resultImage.File);

                        keys.Add(CreateFileKey((VkIncomingFile)resultImage.File));
                    }
                }
            }

            return string.Join(",", keys);
        }

        private static string CreateFileKey(VkIncomingFile vkIncomingFile)
        {
            return vkIncomingFile.AccessKey != null
                ? $"{vkIncomingFile.FileType}{vkIncomingFile.OwnerId}_{vkIncomingFile.FileId}_{vkIncomingFile.AccessKey}"
                : $"{vkIncomingFile.FileType}{vkIncomingFile.OwnerId}_{vkIncomingFile.FileId}";
        }


        private string GetRandomId()
        {
            return DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
        }
    }
}