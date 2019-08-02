using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Artice.Core.Models.Files;

namespace Artice.Telegram.Files
{
    public class TelegramIncomingFile : IIncomingFile
    {
        private readonly Func<ITelegramHttpClient> _clientConstructor;

        public TelegramIncomingFile(Func<ITelegramHttpClient> clientConstructor)
        {
            _clientConstructor = clientConstructor;
        }

        public string FileId { get; set; }

        public string Name { get; set; }

        public string MimeType { get; set; }

        public int? FileSize { get; private set; }

        public string FilePath { get; private set; }


        public async Task<string> GetNameAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(Name))
                await FillFileInfo(cancellationToken);
            

            return Name;
        }

        public async Task<long> GetFileSizeAsync(CancellationToken cancellationToken)
        {
            if (!FileSize.HasValue)
                await FillFileInfo(cancellationToken);
            

            return FileSize ?? 0;
        }

        public async Task<string> GetMimeTypeAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(MimeType))
            {
                using (var response = await GetFileResponseAsync(cancellationToken))
                {
                    MimeType = response.Content.Headers.ContentType.MediaType;
                }
            }

            return MimeType;
        }

        public async Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(FilePath))
                await FillFileInfo(cancellationToken);
            

            var response = await GetFileResponseAsync(cancellationToken);

            MimeType = response.Content.Headers.ContentType.MediaType;

            return new ResponseMessageReadStream(
                await response.Content.ReadAsStreamAsync(), response);
        }

        private async Task FillFileInfo(CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>
            {
                {"file_id", FileId}
            };

            var result =
                await _clientConstructor()
                    .GetAsync<Models.File>("getFile", parameters, cancellationToken);
            var fileInfo = result.ResultObject;

            FilePath = fileInfo.FilePath;
            Name = Name ?? Path.GetFileName(FilePath);
            FileSize = fileInfo.FileSize;
        }

        private Task<HttpResponseMessage> GetFileResponseAsync(CancellationToken cancellationToken)
        {
            return _clientConstructor().GetFileResponseAsync(FilePath, cancellationToken);
        }
    }
}