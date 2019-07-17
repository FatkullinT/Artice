using System.IO;

namespace Artice.Telegram.Models
{
	public class DownloadedFile
	{
		public Stream Content { get; set; }

		public string MimeType { get; set; }
	}
}