using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models;

namespace Artice.Telegram.Files
{
	internal class TelegramFile: IFile
	{
		private Func<ITelegramHttpClient> _clientConstructor;

		public TelegramFile(Func<ITelegramHttpClient> clientConstructor)
		{
			_clientConstructor = clientConstructor;
		}

		public string FileId { get; private set; }

		public string Name { get; private set; }

		public string MimeType { get; private set; }

		public int FileSize { get; private set; }

		public string FilePath { get; private set; }


		public Task<string> GetNameAsync()
		{
			if (Name == null)
			{
				_clientConstructor().


			}

			return Name;
			
		}

		public Task<int> GetFileSizeAsync()
		{
			throw new NotImplementedException();
		}

		public Task<string> GetMimeTypeAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Uri> GetFileUrlAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Uri> GetPlayerUrlAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadStreamAsync()
		{
			throw new NotImplementedException();
		}

		public string Serialize()
		{
			throw new NotImplementedException();
		}

		private async Task<FileInfo> GetFileInfo(CancellationToken cancellationToken)
		{
			var parameters = new Dictionary<string, object>
			{
				{"file_id", FileId}
			};
			Models.File fileInfo;

			string methodPath = string.Concat(Consts.ApiPath, _configuration.AccessToken, "/", "getFile");

			var result =
				await _clientConstructor()
					.GetAsync<Models.File>(methodPath, parameters, cancellationToken);
			fileInfo = result.ResultObject;

			FilePath = fileInfo.FilePath;
			Name = Name ?? Path.GetFileName(FilePath);
		}

		private async Task<Stream> GetFileStream(CancellationToken cancellationToken)
		{
			var downloadedFile = await _client.DownloadFile(file.Url, cancellationToken);
			file.MimeType = downloadedFile.MimeType;
			file.Content = downloadedFile.Content;
		}
	}
}