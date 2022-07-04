using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
	public interface IIncomingFile : IFile
	{
		Task<long> GetFileSizeAsync(CancellationToken cancellationToken = default);

		Task<string> GetMimeTypeAsync(CancellationToken cancellationToken = default);
    }
}