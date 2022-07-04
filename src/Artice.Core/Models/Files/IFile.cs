using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public interface IFile
    {
        Task<string> GetNameAsync(CancellationToken cancellationToken = default);

        Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default);
    }
}