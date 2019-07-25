using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;
using Artice.Core.Models.Enum;
using Artice.Core.OutgoingMessages;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;
using Artice.Telegram.Models.ReplyMarkups;
using Message = Artice.Telegram.Models.Message;

namespace Artice.Telegram
{
	public class TelegramOutgoingMessageProvider : IOutgoingMessageProvider
	{


		private readonly IOutgoingMessageMapper _mapper;
        private readonly Func<ITelegramHttpClient> _clientConstructor;


        protected int SendLimitPerSecond => Consts.SendingPerSecondLimit;


		public string MessengerId => Consts.TelegramId;

		public TelegramOutgoingMessageProvider(
			IOutgoingMessageMapper mapper,
			Func<ITelegramHttpClient> clientConstructor)

        {
            _mapper = mapper;
            _clientConstructor = clientConstructor;
        }

		public async Task SendMessageAsync(OutgoingMessage message, CancellationToken cancellationToken = new CancellationToken())
		{

			var sendResult =
				await
					SendTextMessageAsync(message.Chat != null ? message.Chat.Id : message.To.Id,   message.Text,
						replyMarkup: _mapper.Map(message.InlineKeyboard),
						parseMode: ParseMode.Markdown,
						cancellationToken: cancellationToken);
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

			return SendMessageAsync(MessageType.TextMessage, chatId, text, disableNotification, replyToMessageId,
				replyMarkup,
				additionalParameters, cancellationToken);
		}

		private Task<ApiResponse<Message>> SendMessageAsync(MessageType type, string chatId, object content,
			bool disableNotification = false,
			int replyToMessageId = 0,
			IReplyMarkup replyMarkup = null,
			Dictionary<string, object> additionalParameters = null,
			CancellationToken cancellationToken = default)
		{
			if (additionalParameters == null)
				additionalParameters = new Dictionary<string, object>();

			var typeInfo = type.ToKeyValue();

			additionalParameters.Add("chat_id", chatId);

			if (disableNotification)
				additionalParameters.Add("disable_notification", true);

			if (replyMarkup != null)
				additionalParameters.Add("reply_markup", replyMarkup);

			if (replyToMessageId != 0)
				additionalParameters.Add("reply_to_message_id", replyToMessageId);

			if (!string.IsNullOrEmpty(typeInfo.Value))
				additionalParameters.Add(typeInfo.Value, content);

			return _clientConstructor().PostAsync<Message>(typeInfo.Key, additionalParameters, cancellationToken);
			
		}

		private Task<ApiResponse<bool>> AnswerCallbackQueryAsync(string callbackQueryId, string text = null,
			bool showAlert = false,
			string url = null,
			int cacheTime = 0,
			CancellationToken cancellationToken = default)
		{
			var parameters = new Dictionary<string, object>
			{
				{"callback_query_id", callbackQueryId},
				{"show_alert", showAlert},
			};

			if (!string.IsNullOrEmpty(text))
				parameters.Add("text", text);

			if (!string.IsNullOrEmpty(url))
				parameters.Add("url", url);

			if (cacheTime != 0)
				parameters.Add("cache_time", cacheTime);

			return _clientConstructor().PostAsync<bool>("answerCallbackQuery", parameters, cancellationToken);
			
		}
	}
}