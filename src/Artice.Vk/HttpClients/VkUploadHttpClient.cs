using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Exceptions;
using Artice.Core.Models.Files;
using Artice.Vk.Models;
using Newtonsoft.Json;

namespace Artice.Vk.HttpClients
{
    public class VkUploadHttpClient : IVkUploadHttpClient
    {
        private readonly HttpClient _httpClient;

        public static HttpClient ConfigureClient(HttpClient client)
        {
            return client;
        }

        public VkUploadHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UploadResponse> PostAsync(string uploadUrl, string fieldName, IFile file, CancellationToken cancellationToken)
        {
            var httpContent = new MultipartFormDataContent();
            using (var contentStream = await file.OpenReadStreamAsync(cancellationToken))
            {
                httpContent.Add(new StreamContent(contentStream), fieldName, await file.GetNameAsync(cancellationToken));
                HttpResponseMessage response = await _httpClient.PostAsync(uploadUrl, httpContent, cancellationToken);
                await ThrowIfNotSuccess(response);

                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    string text = reader.ReadToEnd();
                    var uploadResponse = JsonConvert.DeserializeObject<UploadResponse>(text);
                    return uploadResponse;
                }
            }
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
    }
}