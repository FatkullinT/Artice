using System;

namespace Artice.Core.Logger
{
	public interface ILogger
	{
		void Log(LogLevel logLevel, Exception ex, string message, params object[] args);
	}
}