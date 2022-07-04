using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artice.Vk.Models;
using Newtonsoft.Json;

namespace Artice.Vk.HttpClients
{
    public class VkLongPollingHttpClient : IVkLongPollingHttpClient
    {
        private readonly HttpClient _httpClient;

        public static HttpClient ConfigureClient(HttpClient client)
        {
            client.Timeout = TimeSpan.FromSeconds(Consts.LongPoolingTimeout * 2);
            return client;
        }

        public VkLongPollingHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LongPollingResponse> GetAsync(string server, string key, string cursor, CancellationToken cancellationToken = default)
        {
            var url = $"{server}?act=a_check&key={key}&ts={cursor}&wait={Consts.LongPoolingTimeout}";
            var response = await _httpClient.GetAsync(url, cancellationToken);
            return JsonConvert.DeserializeObject<LongPollingResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}