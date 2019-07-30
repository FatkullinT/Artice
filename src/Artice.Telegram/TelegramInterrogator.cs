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

        public TelegramInterrogator(Func<ITelegramHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public async Task<UpdatesResponse<Update>> GetUpdatesAsync(string cursor, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"timeout", Consts.LongPoolingTimeout}
            };

            if (!string.IsNullOrEmpty(cursor))
                parameters.Add("offset", cursor);

            var response = await _clientConstructor().GetAsync<Update[]>("getUpdates", parameters, cancellationToken);

            var nextCursor = response.ResultObject.Any() ? response.ResultObject.Max(update => update.Id) + 1 : 0;

            return new UpdatesResponse<Update>()
            {
                Cursor = nextCursor > 0 ? nextCursor.ToString(CultureInfo.InvariantCulture) : null,
                Updates = response.ResultObject
            };
        }
    }
}