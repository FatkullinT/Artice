using System;
using System.IO;
using System.Threading.Tasks;

namespace Artice.Core.Models
{
	public interface IFile
	{
		Task<string> GetNameAsync();

		Task<int> GetFileSizeAsync();

		Task<string> GetMimeTypeAsync();

		Task<Uri> GetFileUrlAsync();

		Task<Uri> GetPlayerUrlAsync();

		Task<Stream> OpenReadStreamAsync();

		string Serialize();
	}
}