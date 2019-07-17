using System;

namespace Artice.Core.Logger
{
	public static class LoggerExtensions
	{
		/// <summary>Formats and writes a debug log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</example>
		public static void LogDebug(
		  this ILogger logger,
		  Exception exception,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Debug, exception, message, args);
		}

		/// <summary>Formats and writes a debug log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
		public static void LogDebug(
		  this ILogger logger,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Debug, message, args);
		}

		/// <summary>Formats and writes a trace log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
		public static void LogTrace(
		  this ILogger logger,
		  Exception exception,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Trace, exception, message, args);
		}

		/// <summary>Formats and writes a trace log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogTrace("Processing request from {Address}", address)</example>
		public static void LogTrace(this ILogger logger, string message, params object[] args)
		{
			logger.Log(LogLevel.Trace, message, args);
		}

		/// <summary>Formats and writes an informational log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
		public static void LogInformation(
		  this ILogger logger,
		  Exception exception,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Information, exception, message, args);
		}

		/// <summary>Formats and writes an informational log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogInformation("Processing request from {Address}", address)</example>
		public static void LogInformation(this ILogger logger, string message, params object[] args)
		{
			logger.Log(LogLevel.Information, message, args);
		}

		/// <summary>Formats and writes a warning log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
		public static void LogWarning(
		  this ILogger logger,
		  Exception exception,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Warning, exception, message, args);
		}

		/// <summary>Formats and writes a warning log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogWarning("Processing request from {Address}", address)</example>
		public static void LogWarning(this ILogger logger, string message, params object[] args)
		{
			logger.Log(LogLevel.Warning, message, args);
		}

		/// <summary>Formats and writes an error log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
		public static void LogError(
		  this ILogger logger,
		  Exception exception,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Error, exception, message, args);
		}

		/// <summary>Formats and writes an error log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogError("Processing request from {Address}", address)</example>
		public static void LogError(this ILogger logger, string message, params object[] args)
		{
			logger.Log(LogLevel.Error, message, args);
		}

		/// <summary>Formats and writes a critical log message.</summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="exception">The exception to log.</param>
		/// <param name="message">Format string of the log message in message template format. Example: <code>"User {User} logged in from {Address}"</code></param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		/// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
		public static void LogCritical(
		  this ILogger logger,
		  Exception exception,
		  string message,
		  params object[] args)
		{
			logger.Log(LogLevel.Critical, exception, message, args);
		}

		/// <summary>
		/// Formats and writes a log message at the specified log level.
		/// </summary>
		/// <param name="logger">The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> to write to.</param>
		/// <param name="logLevel">Entry will be written on this level.</param>
		/// <param name="message">Format string of the log message.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		public static void Log(
		  this ILogger logger,
		  LogLevel logLevel,
		  string message,
		  params object[] args)
		{
			logger.Log(logLevel, (Exception)null, message, args);
		}
	}
}