﻿namespace Artice.Telegram.AspNetCore
{
	public interface ITelegramProviderConfigurator
	{
		ITelegramProviderConfigurator SetAccessToken(string accessToken);
	}
}