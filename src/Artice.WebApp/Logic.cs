using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Artice.Context;
using Artice.Core.Models;
using Artice.Core.OutgoingMessages;
using Artice.Extensions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Win32.SafeHandles;

namespace Artice.WebApp
{
	public class Logic : ILogic
	{
		public void Dispose()
		{

		}

		public IEnumerable<Task<OutgoingMessage>> Answer(IOutgoingMessageProvider outgoingMessageProvider, IncomingMessage message, IChatContext context)
		{
			if (string.Equals("/start", message.Text))
			{
				yield return Task.FromResult(message.GetResponse("Бот запущен."));
			}
			else if (string.Equals("/help", message.Text))
			{
				yield return Task.FromResult(message.GetResponse("Этот бот не умеет ничего и он очень счастлив, чего и вам желает."));
				yield return Task.FromResult(message.GetResponse("А ты так можешь?"));
			}
			else if (message.Attachments.Any())
			{
				var count = 0;
				foreach (var attachment in message.Attachments)
				{
					var savedFileMessage = SaveFile(attachment, message, outgoingMessageProvider);
					yield return savedFileMessage;
					count++;
				}

				yield return Task.FromResult(message.GetResponse($"Сохранено {count} файлов"));
			}
			else
			{
				yield return Task.FromResult(message.GetResponse($"Получено сообщение: {message.Text}"));
			}
		}

		private async Task<OutgoingMessage> SaveFile(Attachment attachment, IncomingMessage message, IOutgoingMessageProvider outgoingMessageProvider)
		{
			var file = attachment.File;
			await outgoingMessageProvider.GetFileContentAsync(file);
			using (var stream = File.Create($"c:\\temp\\{file.FileName}"))
			{
				file.Content.Seek(0, SeekOrigin.Begin);
				file.Content.CopyTo(stream);
			}

			return message.GetResponse(
				$"Сохранен файл типа {attachment.GetType()} с именем {ReflectMarkdown(attachment.File.FileName)}");
		}

		string ReflectMarkdown(string message)
		{
			return message
				.Replace("_", "\\_")
				.Replace("*", "\\*")
				.Replace("[", "\\[")
				.Replace("'", "\\'");
		}
	}
}