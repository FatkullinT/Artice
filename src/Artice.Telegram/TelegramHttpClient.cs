using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Artice.Telegram.Models;
using Newtonsoft.Json;

namespace Artice.Telegram
{
	public class TelegramHttpClient : ITelegramHttpClient
	{
		private readonly HttpClient _httpClient;

		public TelegramHttpClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public static HttpClient ConfigureClient(HttpClient client)
		{
			client.BaseAddress = new Uri(Consts.BaseUrl);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip,deflate");
			return client;
		}

		public async Task<ApiResponse<T>> GetAsync<T>(string methodPath, Dictionary<string, object> parameters = null,
			CancellationToken cancellationToken = default)
		{
			var uri = parameters == null || !parameters.Any()
				? methodPath
				: string.Concat(methodPath, "?", string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}")));
			var response = await _httpClient.GetAsync(uri, cancellationToken);
			return JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());
		}

		public async Task<ApiResponse<T>> PostAsync<T>(string methodPath, Dictionary<string, object> parameters,
			CancellationToken cancellationToken = default)
		{
			var payload = JsonConvert.SerializeObject(parameters);
			var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync(methodPath, httpContent, cancellationToken);
			return JsonConvert.DeserializeObject<ApiResponse<T>>(await response.Content.ReadAsStringAsync());
		}

		public async Task<DownloadedFile> DownloadFile(Uri fileUrl,
			CancellationToken cancellationToken = default)
		{
			var downloadedFile = new DownloadedFile()
			{
				Content = new MemoryStream()
			};

			using (
				var response =
					await
						_httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
			)
			{
				await response.Content.CopyToAsync(downloadedFile.Content);
				downloadedFile.Content.Position = 0;
				downloadedFile.MimeType = response.Content.Headers.ContentType.MediaType;
			}

			return downloadedFile;
		}
	}
}