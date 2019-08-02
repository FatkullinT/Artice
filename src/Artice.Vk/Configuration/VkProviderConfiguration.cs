namespace Artice.Vk.Configuration
{
	public class VkProviderConfiguration
	{
		public string AccessToken { get; set; }

        public string GroupId { get; set; }

        public string WebhookVerifyToken { get; set; }

        public UpdatesReceivingMethod UpdatesReceivingMethod { get; set; } = UpdatesReceivingMethod.Webhook;
    }
}