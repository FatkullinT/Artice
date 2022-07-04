using System;
using System.Threading;
using System.Threading.Tasks;
using Artice.Vk.Models;

namespace Artice.Vk.HttpClients
{
    public interface IVkLongPollingHttpClient
    {
        Task<LongPollingResponse> GetAsync(
            string server,
            string key,
            string cursor,
            CancellationToken cancellationToken = default);
    }
}