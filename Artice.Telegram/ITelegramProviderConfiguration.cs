using System.Net.Http;

namespace Artice.Telegram
{
	public interface ITelegramProviderConfiguration
	{
		ITelegramProviderConfiguration SetAccessToken(string accessToken);

		ITelegramProviderConfiguration UseHttpMessageHandler(HttpMessageHandler messageHandler);
	}
}