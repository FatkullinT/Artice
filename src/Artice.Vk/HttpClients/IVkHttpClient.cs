using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models.Files;
using Artice.Vk.Models;

namespace Artice.Vk.HttpClients
{
    public interface IVkHttpClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string method, Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> GetAsync(string uri, CancellationToken cancellationToken = default);

        Task<ApiResponse<T>> PostAsync<T>(string method,
            Dictionary<string, string> parameters,
            IEnumerable<KeyValuePair<string, IFile>> files = null,
            CancellationToken cancellationToken = default);
    }
}