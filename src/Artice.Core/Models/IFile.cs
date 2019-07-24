using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models
{
	public interface IFile
	{
		Task<string> GetNameAsync(CancellationToken cancellationToken = default);

		Task<int> GetFileSizeAsync(CancellationToken cancellationToken = default);

		Task<string> GetMimeTypeAsync(CancellationToken cancellationToken = default);

        Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default);

		string Serialize();
	}
}