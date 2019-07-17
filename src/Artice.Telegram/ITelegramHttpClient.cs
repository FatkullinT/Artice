using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artice.Telegram.Models;

namespace Artice.Telegram
{
	public interface ITelegramHttpClient
	{
		Task<ApiResponse<T>> GetAsync<T>(string methodPath, Dictionary<string, object> parameters = null,
			CancellationToken cancellationToken = default);

		Task<ApiResponse<T>> PostAsync<T>(string methodPath, Dictionary<string, object> parameters,
			CancellationToken cancellationToken = default);

		Task<DownloadedFile> DownloadFile(Uri fileUrl,
			CancellationToken cancellationToken = default);
	}
}