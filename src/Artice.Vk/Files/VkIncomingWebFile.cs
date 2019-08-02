using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models.Files;
using Artice.Vk.HttpClients;

namespace Artice.Vk.Files
{
    public class VkIncomingWebFile : VkIncomingFile, IWebFile
    {
        public VkIncomingWebFile(Func<IVkHttpClient> clientConstructor, long fileId, long ownerId, string accessKey) 
            : base(fileId, ownerId, accessKey)
        {
            _clientConstructor = clientConstructor;
            FileSize = -1;
        }


        private readonly Func<IVkHttpClient> _clientConstructor;

        public VkIncomingWebFile(Func<IVkHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
            FileSize = -1;
        }

        public Uri Uri { get; set; }

        public string MimeType { get; set; }

        public long FileSize { get; set; }

        public Task<Uri> GetFileUriAsync()
        {
            return Task.FromResult(Uri);
        }

        public override async Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            var response = await GetFileResponseAsync(cancellationToken);

            MimeType = response.Content.Headers.ContentType.MediaType;

            var stream = new ResponseMessageReadStream(
                await response.Content.ReadAsStreamAsync(), response);

            FileSize =
                response.Content.Headers.ContentLength
                ?? response.Content.Headers.ContentLength
                ?? stream.Length;

            return stream;
        }

        public override async Task<long> GetFileSizeAsync(CancellationToken cancellationToken = default)
        {
            if (FileSize < 0)
            {
                using (var response = await GetFileResponseAsync(cancellationToken))
                {
                    FileSize =
                        response.Content.Headers.ContentLength
                        ?? response.Content.Headers.ContentLength
                        ?? (await response.Content.ReadAsStreamAsync()).Length;
                    MimeType = response.Content.Headers.ContentType.MediaType;
                }
            }

            return FileSize;
        }

        public override async Task<string> GetMimeTypeAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(MimeType))
            {
                using (var response = await GetFileResponseAsync(cancellationToken))
                {
                    MimeType = response.Content.Headers.ContentType.MediaType;
                    FileSize =
                        response.Content.Headers.ContentLength
                        ?? response.Content.Headers.ContentLength
                        ?? (await response.Content.ReadAsStreamAsync()).Length;
                }
            }

            return MimeType;
        }

        private Task<HttpResponseMessage> GetFileResponseAsync(CancellationToken cancellationToken)
        {
            return _clientConstructor().GetAsync(Uri.AbsoluteUri, cancellationToken);
        }
    }
}