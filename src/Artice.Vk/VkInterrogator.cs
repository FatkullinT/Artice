using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Exceptions;
using Artice.Core.IncomingMessages;
using Artice.Core.Models;
using Artice.Vk.Configuration;
using Artice.Vk.HttpClients;
using Artice.Vk.Models;
using Artice.Vk.Models.Enum;

namespace Artice.Vk
{
    public class VkInterrogator : IInterrogator<Update>
    {
        private readonly Func<IVkHttpClient> _clientConstructor;
        private readonly VkProviderConfiguration _config;
        private readonly Func<IVkLongPollingHttpClient> _longPollingClientConstructor;

        private const string ServerName = "Server";
        private const string KeyName = "Key";
        private const string CursorName = "Cursor";

        public VkInterrogator(
            Func<IVkLongPollingHttpClient> longPollingClientConstructor,
            Func<IVkHttpClient> clientConstructor,
            VkProviderConfiguration config)
        {
            _longPollingClientConstructor = longPollingClientConstructor;
            _clientConstructor = clientConstructor;
            _config = config;
        }

        public async Task<UpdatesResponse<Update>> GetUpdatesAsync(Dictionary<string, string> contextData, CancellationToken cancellationToken)
        {
            if (contextData == null)
            {
                contextData = await InitContextData(cancellationToken);
            }

            var response = await SendLongPoolRequest(contextData, cancellationToken);

            return await ProcessResponse(response, contextData, cancellationToken);
        }

        private async Task<UpdatesResponse<Update>> ProcessResponse(
            LongPollingResponse response,
            Dictionary<string, string> contextData,
            CancellationToken cancellationToken)
        {
            switch (response.Failed)
            {
                case LongPollingFail.None:
                    return GetUpdatesResponse(response, contextData);

                case LongPollingFail.HistoryOutdated:
                    return await SendRequestWithNewCursor(response, contextData, cancellationToken);

                case LongPollingFail.KeyExpired:
                case LongPollingFail.ServerInfoLost:
                    return await SendRequestWithNewServerInfo(response, contextData, cancellationToken);
                default:
                    throw new ArticeExecutionException($"Unknown value of \"failed\" parameter: {response.Failed}");
            }
        }

        private async Task<UpdatesResponse<Update>> SendRequestWithNewServerInfo(
            LongPollingResponse response,
            Dictionary<string, string> contextData,
            CancellationToken cancellationToken)
        {
            var serverInfo = await GetLongPollServerInfo(cancellationToken);
            contextData[CursorName] = serverInfo.Cursor;
            contextData[KeyName] = serverInfo.Key;
            contextData[ServerName] = serverInfo.Server;
            var newResponse = await SendLongPoolRequest(contextData, cancellationToken);
            return await ProcessResponse(newResponse, contextData, cancellationToken);
        }

        private async Task<UpdatesResponse<Update>> SendRequestWithNewCursor(
            LongPollingResponse response,
            Dictionary<string, string> contextData,
            CancellationToken cancellationToken)
        {
            contextData[CursorName] = response.Cursor;
            var newResponse = await SendLongPoolRequest(contextData, cancellationToken);
            return await ProcessResponse(newResponse, contextData, cancellationToken);
        }

        private UpdatesResponse<Update> GetUpdatesResponse(
            LongPollingResponse response, 
            Dictionary<string, string> contextData)
        {
            contextData[CursorName] = response.Cursor;
            return new UpdatesResponse<Update>()
            {
                Updates = response.Updates,
                ContextData = contextData
            };
        }

        private Task<LongPollingResponse> SendLongPoolRequest(Dictionary<string, string> contextData, CancellationToken cancellationToken)
        {
             return _longPollingClientConstructor().GetAsync(
                contextData[ServerName],
                contextData[KeyName],
                contextData[CursorName],
                cancellationToken);
        }

        private async Task<Dictionary<string, string>> InitContextData(CancellationToken cancellationToken)
        {
            var serverInfo = await GetLongPollServerInfo(cancellationToken);
            return new Dictionary<string, string>()
            {
                {CursorName, serverInfo.Cursor},
                {ServerName, serverInfo.Server},
                {KeyName, serverInfo.Key}
            };
        }

        private async Task<LongPoolServerInfo> GetLongPollServerInfo(CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"group_id", _config.GroupId}
            };

            var response = await _clientConstructor().GetAsync<LongPoolServerInfo>("groups.getLongPollServer", parameters, cancellationToken);
            return response.Response;
        }
    }

}