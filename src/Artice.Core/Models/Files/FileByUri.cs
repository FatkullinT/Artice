using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public class FileByUri : IOutgoingFile, IIncomingFile, IWebFile
    {
        public Uri FileUri { get; }

        public long FileSize { get; private set; } = -1;

        public string MimeType { get; private set; }

        public FileByUri(Uri fileUri)
        {
            FileUri = fileUri;
        }

        public Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Path.GetFileName(FileUri.AbsolutePath));
        }

        public async Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            var response = await GetFileResponseAsync(cancellationToken);
            MimeType = response.Content.Headers.ContentType.MediaType;
            FileSize = response.Content.Headers.ContentLength ?? default;

            return new ResponseMessageReadStream(
                    await response.Content.ReadAsStreamAsync(), response);
            
        }

        public Task<Uri> GetFileUriAsync()
        {
            return Task.FromResult(FileUri);
        }

        public async Task<long> GetFileSizeAsync(CancellationToken cancellationToken = default)
        {
            if (FileSize < 0)
            {
                using (var response = await GetFileResponseAsync(cancellationToken))
                {
                    await GetFileResponseAsync(cancellationToken);
                    MimeType = response.Content.Headers.ContentType.MediaType;

                    if (response.Content.Headers.ContentLength.HasValue)
                    {
                        FileSize = response.Content.Headers.ContentLength.Value;
                    }
                    else
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            FileSize = stream.Length;
                        }
                    }
                     
                }
            }

            return FileSize;
        }

        public async Task<string> GetMimeTypeAsync(CancellationToken cancellationToken = default)
        {
            if (MimeType == null)
            {
                using (var response = await GetFileResponseAsync(cancellationToken))
                {
                    await GetFileResponseAsync(cancellationToken);
                    MimeType = response.Content.Headers.ContentType.MediaType;
                    FileSize = response.Content.Headers.ContentLength ?? FileSize;
                }
            }

            return MimeType;
        }

        private async Task<HttpResponseMessage> GetFileResponseAsync(CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(
                    FileUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);
            }
        }
    }
}