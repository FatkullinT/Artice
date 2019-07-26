using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            return JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string method, Dictionary<string, object> parameters,
            CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(parameters);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(GetMethodPath(method), httpContent, cancellationToken);
            return JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());
        }

        public Task<HttpResponseMessage> GetFileResponseAsync(string fileInnerPath,
            CancellationToken cancellationToken = default)
        {
            return _httpClient.GetAsync(
                GetFileOuterPath(fileInnerPath),
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
        }

        private string GetFileOuterPath(string fileInnerPath)
        {
            return string.Concat(Consts.FilePath, _configuration.AccessToken, "/", fileInnerPath);
        }

        private string GetMethodPath(string methodName)
        {
            return string.Concat(Consts.ApiPath, _configuration.AccessToken, "/", methodName);
        }
    }
}