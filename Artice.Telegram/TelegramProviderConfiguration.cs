using System.Net.Http;

namespace Artice.Telegram
{
	public class TelegramProviderConfiguration : ITelegramProviderConfiguration
	{
		public string AccessToken { get; private set; }

		public HttpMessageHandler HttpMessageHandler { get; private set; }

		public ITelegramProviderConfiguration SetAccessToken(string accessToken)
		{
			AccessToken = accessToken;
			return this;
		}

		public ITelegramProviderConfiguration UseHttpMessageHandler(HttpMessageHandler messageHandler)
		{
			HttpMessageHandler = messageHandler;
			return this;
		}
	}
}