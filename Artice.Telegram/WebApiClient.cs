using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Artice.Telegram
{
    internal class WebApiClient : HttpClient
    {
        public WebApiClient(string baseUrl, HttpMessageHandler handler) : base(handler)
		{
            BaseAddress = new Uri(baseUrl);
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip,deflate");
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string methodName, Dictionary<string, object> parameters = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uri = parameters == null || !parameters.Any()
                ? methodName
                : string.Concat(methodName, "?", string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")));
            var response = await GetAsync(uri, cancellationToken);
            return JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string methodName, Dictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            var payload = JsonConvert.SerializeObject(parameters);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = PostAsync(methodName, httpContent, cancellationToken);
            var res = response.GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<ApiResponse<T>>(await res.Content.ReadAsStringAsync());
        }

        public class ApiResponse<TResponse>
        {
            /// <summary>
            /// Gets a value indicating whether the request was successful.
            /// </summary>
            [JsonProperty("ok", Required = Required.Always)]
            public bool Ok { get; set; }

            /// <summary>
            /// Gets the result object.
            /// </summary>
            [JsonProperty("result", Required = Required.Default)]
            public TResponse ResultObject { get; set; }

            /// <summary>
            /// Gets the error message.
            /// </summary>
            [JsonProperty("description", Required = Required.Default)]
            public string Message { get; set; }

            /// <summary>
            /// Gets the error code.
            /// </summary>
            [JsonProperty("error_code", Required = Required.Default)]
            public int Code { get; set; }
        }
    }
}