using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Core.IncomingMessages
{
    public interface IInterrogator<TUpdate>
    {
        Task<UpdatesResponse<TUpdate>> GetUpdatesAsync(string cursor, CancellationToken cancellationToken);
    }
}