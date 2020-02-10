using Artice.Vk.Configuration;

namespace Artice.Vk.AspNetCore
{
	public interface IVkProviderConfigurator
	{
        IVkProviderConfigurator SetGroupCredentials(string groupId, string accessToken);

        IVkProviderConfigurator UseUpdatesReceivingMethod(UpdatesReceivingMethod method);

        IVkProviderConfigurator SetWebhookVerifyToken(string verifyToken);
    }
}