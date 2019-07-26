namespace Artice.Telegram
{
	public class TelegramProviderConfiguration
	{
		public string AccessToken { get; set; }

        public UpdatesReceivingMethod UpdatesReceivingMethod { get; set; } = UpdatesReceivingMethod.Webhook;
    }
}