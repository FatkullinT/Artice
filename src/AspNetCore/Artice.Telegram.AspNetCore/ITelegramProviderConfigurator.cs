using Artice.Telegram.Configuration;

namespace Artice.Telegram.AspNetCore
{
	public interface ITelegramProviderConfigurator
	{
		ITelegramProviderConfigurator SetAccessToken(string accessToken);

        ITelegramProviderConfigurator UseUpdatesReceivingMethod(UpdatesReceivingMethod method);
    }
}