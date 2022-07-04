using Artice.Vk.Configuration;

namespace Artice.Vk.AspNetCore
{
	internal class VkProviderConfigurator : IVkProviderConfigurator
	{
		public VkProviderConfiguration Configuration { get; } = new VkProviderConfiguration();

		public IVkProviderConfigurator SetGroupCredentials(string groupId, string accessToken)
		{
			Configuration.AccessToken = accessToken;
            Configuration.GroupId = groupId;
			return this;
		}

        public IVkProviderConfigurator UseUpdatesReceivingMethod(UpdatesReceivingMethod method)
        {
            Configuration.UpdatesReceivingMethod = method;
            return this;
        }

        public IVkProviderConfigurator SetWebhookVerifyToken(string verifyToken)
        {
            Configuration.WebhookVerifyToken = verifyToken;
            return this;
        }
    }
}