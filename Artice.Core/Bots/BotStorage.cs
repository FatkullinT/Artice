using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Artice.Core.Bots
{
	internal class BotStorage : IBotStorage
	{
		private readonly IServiceScope _serviceScope;

		public BotStorage(IServiceProvider serviceProvider, IEnumerable<Func<IServiceProvider, IChatBot>> providerFactories)
		{
			_serviceScope = serviceProvider.CreateScope();

				
			foreach (var provider in providerFactories.Select(func => func(_serviceScope.ServiceProvider)))
			{
				AddBot(provider);
			}
		}

		private readonly Dictionary<Type, IChatBot> _chatBots = new Dictionary<Type, IChatBot>();

        private void AddBot(IChatBot bot)
        {
            Type botType = bot.GetType();
            if (_chatBots.ContainsKey(botType))
            {
                _chatBots[botType] = bot;
            }
            else
            {
                _chatBots.Add(botType, bot);
            }
        }

        public T GetBot<T>() where T : class, IChatBot
        {
	        if (_chatBots.TryGetValue(typeof(T), out var bot))
            {
                return (T)bot;
            }
            return null;
        }

        public IChatBot GetBot(string botName)
        {
            return _chatBots.Values.FirstOrDefault(bot => string.Equals(botName, bot.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<IChatBot> Bots => _chatBots.Values;

        public void Dispose()
        {
			foreach (var bot in _chatBots.Values)
            {
                bot.Dispose();
            }
			_serviceScope.Dispose();
			_chatBots.Clear();
        }
    }
}