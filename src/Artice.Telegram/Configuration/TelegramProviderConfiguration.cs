namespace Artice.Telegram.Configuration
{
	public class TelegramProviderConfiguration
	{
		public string AccessToken { get; set; }

        public UpdatesReceivingMethod UpdatesReceivingMethod { get; set; } = UpdatesReceivingMethod.Webhook;
    }
}