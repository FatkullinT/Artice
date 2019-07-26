namespace Artice.Telegram.AspNetCore
{
	internal class TelegramProviderConfigurator : ITelegramProviderConfigurator
	{
		public TelegramProviderConfiguration Configuration { get; } = new TelegramProviderConfiguration();

		public ITelegramProviderConfigurator SetAccessToken(string accessToken)
		{
			Configuration.AccessToken = accessToken;
			return this;
		}

        public ITelegramProviderConfigurator UseUpdatesReceivingMethod(UpdatesReceivingMethod method)
        {
            Configuration.UpdatesReceivingMethod = method;
            return this;
        }
    }
}