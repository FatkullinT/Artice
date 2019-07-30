using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public class OutgoingFileByUri : IOutgoingFile, IWebFile
    {
        public Uri FileUri { get; }

        public OutgoingFileByUri(Uri fileUri)
        {
            FileUri = fileUri;
        }

        public Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Path.GetFileName(FileUri.AbsolutePath));
        }

        public async Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(
                    FileUri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                return new ResponseMessageReadStream(
                    await response.Content.ReadAsStreamAsync(), response);
            }
        }

        public Task<Uri> GetFileUriAsync()
        {
            return Task.FromResult(FileUri);
        }
    }
}