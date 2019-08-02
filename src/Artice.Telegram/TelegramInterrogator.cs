using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Artice.Telegram.Models;

namespace Artice.Telegram
{
    public class TelegramInterrogator : IInterrogator<Update>
    {
        private readonly Func<ITelegramHttpClient> _clientConstructor;
        private const string CursorKeyName = "Cursor";
        public TelegramInterrogator(Func<ITelegramHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public async Task<UpdatesResponse<Update>> GetUpdatesAsync(Dictionary<string, string> contextData, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"timeout", Consts.LongPoolingTimeout}
            };

            if (contextData != null)
                parameters.Add("offset", contextData[CursorKeyName]);

            var response = await _clientConstructor().GetAsync<Update[]>("getUpdates", parameters, cancellationToken);

            var nextCursor = response.ResultObject.Any() ? response.ResultObject.Max(update => update.Id) + 1 : 0;
            var nextCursorStr = nextCursor > 0 ? nextCursor.ToString(CultureInfo.InvariantCulture) : null;

            if (contextData == null)
            {
                contextData = new Dictionary<string, string>()
                {
                    {CursorKeyName, nextCursorStr}
                };
            }
            else
            {
                contextData[CursorKeyName] = nextCursorStr;
            }

            return new UpdatesResponse<Update>()
            {
                ContextData = contextData,
                Updates = response.ResultObject
            };
        }
    }
}