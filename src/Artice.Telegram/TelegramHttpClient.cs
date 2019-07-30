using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Exceptions;
using Artice.Core.Models.Files;
using Artice.Telegram.Files;
using Artice.Telegram.Models;
using Newtonsoft.Json;

namespace Artice.Telegram
{
    public class TelegramHttpClient : ITelegramHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly TelegramProviderConfiguration _configuration;

        public TelegramHttpClient(HttpClient httpClient, TelegramProviderConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            if (configuration.UpdatesReceivingMethod == UpdatesReceivingMethod.LongPolling)
                _httpClient.Timeout = TimeSpan.FromSeconds(Consts.LongPoolingTimeout * 2);
        }

        public static HttpClient ConfigureClient(HttpClient client)
        {
            client.BaseAddress = new Uri(Consts.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip,deflate");
            return client;
        }


        public async Task<ApiResponse<T>> GetAsync<T>(string method, Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
        {
            var uri = parameters == null || !parameters.Any()
                ? GetMethodPath(method)
                : string.Concat(GetMethodPath(method), "?", string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")));
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            await ThrowIfNotSuccess(response);

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());

            ThrowIfNotSuccess(apiResponse);

            return apiResponse;
        }

        public async Task<ApiResponse<T>> PostFilesAsync<T>(string method,
            Dictionary<string, string> parameters,
            IEnumerable<KeyValuePair<string, IFile>> files,
            CancellationToken cancellationToken = default)
        {
            var httpContent = new MultipartFormDataContent();

            foreach (var parameter in parameters)
            {
                httpContent.Add(
                    new StringContent(parameter.Value), parameter.Key);
            }

            using (var filesEnumerator = files.GetEnumerator())
            {
                filesEnumerator.Reset();
                return await PostFilesAsync<T>(method, httpContent, filesEnumerator, cancellationToken);
            }
        }

        private async Task<ApiResponse<T>> PostFilesAsync<T>(
            string method,
            MultipartFormDataContent httpContent,
            IEnumerator<KeyValuePair<string, IFile>> filesEnumerator,
            CancellationToken cancellationToken)
        {
            if (filesEnumerator.MoveNext())
            {
                var file = filesEnumerator.Current.Value;
                var field = filesEnumerator.Current.Key;

                if (file is OutgoingMultiTypeFile multiTypeFile)
                {
                    file = multiTypeFile.GetIfExist<TelegramIncomingFile>() ?? multiTypeFile.Prefer<IWebFile>();
                }

                if (file is TelegramIncomingFile telegramIncomingFile)
                {
                    httpContent.Add(new StringContent(telegramIncomingFile.FileId), field);
                    return await PostFilesAsync<T>(method, httpContent, filesEnumerator, cancellationToken);
                }

                if (file is IWebFile webFile)
                {
                    httpContent.Add(new StringContent((await webFile.GetFileUriAsync()).AbsolutePath), field);
                    return await PostFilesAsync<T>(method, httpContent, filesEnumerator, cancellationToken);
                }

                using (var contentStream = await file.OpenReadStreamAsync(cancellationToken))
                {
                    httpContent.Add(new StreamContent(contentStream), field, await file.GetNameAsync(cancellationToken));
                    return await PostFilesAsync<T>(method, httpContent, filesEnumerator, cancellationToken);
                }
            }

            return await PostAsync<T>(method, httpContent, cancellationToken);
        }

        public Task<ApiResponse<T>> PostAsync<T>(string method, Dictionary<string, object> parameters,
            CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(parameters);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
            return PostAsync<T>(method, httpContent, cancellationToken);
        }

        private async Task<ApiResponse<T>> PostAsync<T>(string method, HttpContent httpContent,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(GetMethodPath(method), httpContent, cancellationToken);

            await ThrowIfNotSuccess(response);

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());

            ThrowIfNotSuccess(apiResponse);

            return apiResponse;
        }

        public async Task<HttpResponseMessage> GetFileResponseAsync(string fileInnerPath,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(
                GetFileOuterPath(fileInnerPath),
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            await ThrowIfNotSuccess(response);

            return response;
        }

        private string GetFileOuterPath(string fileInnerPath)
        {
            return string.Concat(Consts.FilePath, _configuration.AccessToken, "/", fileInnerPath);
        }

        private string GetMethodPath(string methodName)
        {
            return string.Concat(Consts.ApiPath, _configuration.AccessToken, "/", methodName);
        }

        private async Task ThrowIfNotSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var requestUrl = response.RequestMessage.RequestUri.ToString();
            var requestMethod = response.RequestMessage.Method.ToString();
            var innerMessage = await response.Content.ReadAsStringAsync();

            var messageBuilder = new StringBuilder("The request finished with an error.");
            messageBuilder.AppendLine($" Request: {requestMethod} {requestUrl}");

            if (innerMessage.Length > 0 && innerMessage.Length < 512)
                messageBuilder.AppendLine($" ErrorDetails: {innerMessage}");

            throw new ApiRequestException(messageBuilder.ToString())
            {
                BotApiIdentifier = Consts.TelegramId,
                StatusCode = response.StatusCode
            };
        }

        private void ThrowIfNotSuccess<T>(ApiResponse<T> response)
        {
            if (response.Ok)
                return;

            throw new ApiRequestException($"Api returned error.\n\r  ErrorDetails: {response.Message}")
            {
                BotApiIdentifier = Consts.TelegramId,
                StatusCode = HttpStatusCode.OK,
                ErrorCode = response.Code.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}