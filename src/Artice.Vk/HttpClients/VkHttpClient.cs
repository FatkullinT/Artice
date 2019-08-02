using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Exceptions;
using Artice.Core.Models.Files;
using Artice.Vk.Configuration;
using Artice.Vk.Models;
using Newtonsoft.Json;

namespace Artice.Vk.HttpClients
{
    public class VkHttpClient : IVkHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly VkProviderConfiguration _configuration;

        public VkHttpClient(HttpClient httpClient, VkProviderConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public static HttpClient ConfigureClient(HttpClient client)
        {
            client.BaseAddress = new Uri(Consts.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string method, Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
        {
            var uri = parameters == null || !parameters.Any()
                ? GetMethodPath(method)
                : string.Concat(GetMethodPath(method), "&", string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            var response = await GetAsync(uri, cancellationToken);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());

            ThrowIfNotSuccess(apiResponse);

            return apiResponse;
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(uri, cancellationToken);

            await ThrowIfNotSuccess(response);

            return response;
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string method,
            Dictionary<string, string> parameters,
            IEnumerable<KeyValuePair<string, IFile>> files = null,
            CancellationToken cancellationToken = default)
        {
            var httpContent = new MultipartFormDataContent();

            foreach (var parameter in parameters)
            {
                httpContent.Add(
                    new StringContent(parameter.Value), parameter.Key);
            }

            if (files == null)
                return await PostAsync<T>(method, httpContent, cancellationToken);

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

                using (var contentStream = await file.OpenReadStreamAsync(cancellationToken))
                {
                    httpContent.Add(new StreamContent(contentStream), field, await file.GetNameAsync(cancellationToken));
                    return await PostFilesAsync<T>(method, httpContent, filesEnumerator, cancellationToken);
                }
            }

            return await PostAsync<T>(method, httpContent, cancellationToken);
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


        private string GetMethodPath(string methodName)
        {
            return $"/method/{methodName}?v={Consts.ApiVersion}&access_token={_configuration.AccessToken}";
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
                BotApiIdentifier = Consts.ChannelId,
                StatusCode = response.StatusCode
            };
        }

        private void ThrowIfNotSuccess<T>(ApiResponse<T> response)
        {
            if (response.Error == null)
                return;

            throw new ApiRequestException($"Api returned error.\n\r  ErrorDetails: {response.Error.Message}")
            {
                BotApiIdentifier = Consts.ChannelId,
                StatusCode = HttpStatusCode.OK,
                ErrorCode = response.Error.Code.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}