using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Telegram.Models;
using System.Net.Http;
using Artice.Core.Models.Files;

namespace Artice.Telegram
{
    public interface ITelegramHttpClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string methodPath, Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<T>> PostAsync<T>(string methodPath, Dictionary<string, object> parameters,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> GetFileResponseAsync(string fileInnerPath,
            CancellationToken cancellationToken = default);

        Task<ApiResponse<T>> PostFilesAsync<T>(string method, 
            Dictionary<string, string> parameters,
            IEnumerable<KeyValuePair<string, IFile>> files,
            CancellationToken cancellationToken = default);
    }
}