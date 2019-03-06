using System;
using System.Collections.Generic;

namespace Artice.Core.Bots
{
	public interface IBotStorage : IDisposable
	{
		T GetBot<T>() where T : class, IChatBot;
		IChatBot GetBot(string botName);
		IEnumerable<IChatBot> Bots { get; }
	}
}