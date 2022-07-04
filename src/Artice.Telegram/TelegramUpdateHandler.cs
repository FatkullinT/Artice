using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Artice.Telegram.Mapping;
using Artice.Telegram.Models;
using Artice.Telegram.Models.Enums;

namespace Artice.Telegram
{
    public class TelegramUpdateHandler : IIncomingUpdateHandler<Update>
    {
        private readonly IIncomingMessageMapper _mapper;
        private readonly Func<ITelegramHttpClient> _clientConstructor;

        public TelegramUpdateHandler(IIncomingMessageMapper mapper, Func<ITelegramHttpClient> clientConstructor)
        {
            _mapper = mapper;
            _clientConstructor = clientConstructor;
        }

        public async Task<IncomingMessage> HandleAsync(Update update, CancellationToken cancellationToken = default)
        {
            switch (update.Type)
            {
                case UpdateType.MessageUpdate:
                    return _mapper.Map(update.Message);

                case UpdateType.CallbackQueryUpdate:
                    await AnswerCallbackQueryAsync(update.CallbackQuery, cancellationToken);
                    return _mapper.Map(update.CallbackQuery);

                default:
                    return null;
            }
        }

        private Task<ApiResponse<bool>> AnswerCallbackQueryAsync(
            CallbackQuery callbackQuery,
            CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>
            {
                {"callback_query_id", callbackQuery.Id},
                {"show_alert", false},
            };

            return _clientConstructor().PostAsync<bool>("answerCallbackQuery", parameters, cancellationToken);
        }
    }
}