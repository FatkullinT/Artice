using System;
using Microsoft.Extensions.Logging;
using ILogger = Artice.Core.Logger.ILogger;
using LogLevel = Artice.Core.Logger.LogLevel;

namespace Artice.Core.AspNetCore
{
	internal class ArticeLogger : ILogger
	{
		private readonly ILogger<ArticeLogger> _logger;

		public ArticeLogger(ILogger<ArticeLogger> logger)
		{
			_logger = logger;
		}

		public void Log(LogLevel logLevel, Exception ex, string message, params object[] args)
		{
			_logger.Log((Microsoft.Extensions.Logging.LogLevel)logLevel, 0, ex, message, args);
		}
	}
}