using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Core.OutgoingMessages;
using Artice.Telegram.Extensions;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;
using Artice.Telegram.Models.ReplyMarkups;
using Newtonsoft.Json;
using Message = Artice.Telegram.Models.Message;

namespace Artice.Telegram
{
    public class TelegramOutgoingMessageProvider : IOutgoingMessageProvider
    {


        private readonly IOutgoingMessageMapper _mapper;
        private readonly IIncomingAttachmentMapper _attachmentMapper;
        private readonly Func<ITelegramHttpClient> _clientConstructor;

        public string ChannelId => Consts.ChannelId;

        public TelegramOutgoingMessageProvider(
            IOutgoingMessageMapper mapper,
            IIncomingAttachmentMapper attachmentMapper,
            Func<ITelegramHttpClient> clientConstructor)

        {
            _mapper = mapper;
            _attachmentMapper = attachmentMapper;
            _clientConstructor = clientConstructor;
        }

        public async Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            var clientId = message.Group != null ? message.Group.Id : message.To.Id;

            if (message.Attachments != null && message.Attachments.Any())
            {
                var needSendMessage = !string.IsNullOrEmpty(message.Text) || message.Keyboard != null;
                foreach (var attachment in message.Attachments)
                {
                    var result = await SendAttachmentMessageAsync(
                        clientId,
                        needSendMessage ? message.Text : null,
                        attachment,
                        replyMarkup: needSendMessage ? _mapper.Map(message.Keyboard) : null,
                        parseMode: ParseMode.Markdown,
                        cancellationToken: cancellationToken);

                    var newAttachment = _attachmentMapper.Map(result.ResultObject).FirstOrDefault();

                    if (newAttachment != null)
                        attachment.File = new OutgoingMultiTypeFile(attachment.File, newAttachment.File);

                    needSendMessage = false;
                }
            }
            else
            {
                await SendTextMessageAsync(
                    clientId,
                    message.Text,
                    replyMarkup: _mapper.Map(message.Keyboard),
                    parseMode: ParseMode.Markdown,
                    cancellationToken: cancellationToken);
            }


            //return sendResult.Ok || sendResult.Code != 429;
        }

        private Task<ApiResponse<Message>> SendTextMessageAsync(string chatId, string text, bool disableWebPagePreview = false,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null,
            ParseMode parseMode = ParseMode.Default,
            CancellationToken cancellationToken = default)
        {
            var additionalParameters = new Dictionary<string, object>();

            if (disableWebPagePreview)
                additionalParameters.Add("disable_web_page_preview", true);

            if (parseMode != ParseMode.Default)
                additionalParameters.Add("parse_mode", parseMode.ToModeString());

            additionalParameters.Add("chat_id", chatId);

            if (disableNotification)
                additionalParameters.Add("disable_notification", true);

            if (replyMarkup != null)
                additionalParameters.Add("reply_markup", replyMarkup);

            if (replyToMessageId != 0)
                additionalParameters.Add("reply_to_message_id", replyToMessageId);

            additionalParameters.Add("text", text);

            return _clientConstructor().PostAsync<Message>("sendMessage", additionalParameters, cancellationToken);
        }

        private async Task<ApiResponse<Message>> SendAttachmentMessageAsync(
            string chatId,
            string messageText,
            Attachment attachment,
            bool disableNotification = false,
            int replyToMessageId = 0,
            IReplyMarkup replyMarkup = null,
            ParseMode parseMode = ParseMode.Default,
            CancellationToken cancellationToken = default)
        {
            var sendingParams = attachment.GetSendingParams();

            var additionalParameters = new Dictionary<string, string>();

            if (parseMode != ParseMode.Default)
                additionalParameters.Add("parse_mode", parseMode.ToModeString());

            additionalParameters.Add("chat_id", chatId);

            if (disableNotification)
                additionalParameters.Add("disable_notification", bool.TrueString);

            if (replyMarkup != null)
                additionalParameters.Add("reply_markup", JsonConvert.SerializeObject(replyMarkup));

            if (replyToMessageId != 0)
                additionalParameters.Add("reply_to_message_id", replyToMessageId.ToString(CultureInfo.InvariantCulture));

            if (messageText != null)
                additionalParameters.Add("caption", messageText);

            var files = new[]
            {
                new KeyValuePair<string, IFile>(sendingParams.ContentFieldName, attachment.File)
            };

            return await _clientConstructor().PostFilesAsync<Artice.Telegram.Models.Message>(sendingParams.Method, additionalParameters, files, cancellationToken);
        }
    }
}