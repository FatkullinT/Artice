using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Artice.Context;
using Artice.Core.Models;
using Artice.Core.Models.Files;
using Artice.Core.OutgoingMessages;
using Artice.Extensions;

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
            else if (string.Equals("/inlineButtons", message.Text, StringComparison.CurrentCultureIgnoreCase))
            {
                yield return GetInlineKeyboard(message);
            }
            else if (string.Equals("/testPhoto", message.Text, StringComparison.CurrentCultureIgnoreCase))
            {
                var attachmentMessage = message.GetResponse("test photo response");
                attachmentMessage.Attachments = new Attachment[]
                {
                    new Image()
                    {
                        File = new OutgoingLocalFile("C:\\Temp\\file8.jpg")
                    },
                };
                yield return Task.FromResult(attachmentMessage);
            }
            else if (string.Equals("/doublePhotoSend", message.Text, StringComparison.CurrentCultureIgnoreCase))
            {
                var attachmentMessage = message.GetResponse("test photo - 1");
                attachmentMessage.Attachments = new Attachment[]
                {
                    new Image()
                    {
                        File = new OutgoingLocalFile("C:\\Temp\\file8.jpg")
                    },
                };
                yield return Task.FromResult(attachmentMessage);
                attachmentMessage.Text = "test photo - 2";
                yield return Task.FromResult(attachmentMessage);
            }

            else if (string.Equals("/testPhotoWithKeyboard", message.Text, StringComparison.CurrentCultureIgnoreCase))
            {
                var attachmentMessage = message.GetResponse("test photo response");
                attachmentMessage.Keyboard = new Keyboard()
                {
                    Buttons =
                    {
                        new KeyboardButton() {ButtonText = "1-1", CallbackData = "1/1", RowOrder = 1, ColumnOrder = 1},
                        new KeyboardButton() {ButtonText = "1-2", CallbackData = "1/2", RowOrder = 1, ColumnOrder = 2},
                        new KeyboardButton() {ButtonText = "2-1", CallbackData = "2/1", RowOrder = 2, ColumnOrder = 1},
                        new KeyboardButton() {ButtonText = "2-3", CallbackData = "2/3", RowOrder = 2, ColumnOrder = 3},
                        new KeyboardButton() {ButtonText = "4-1", CallbackData = "4/1", RowOrder = 4, ColumnOrder = 1}
                    }
                };
                attachmentMessage.Attachments = new Attachment[]
                {
                    new Image()
                    {
                        File = new OutgoingLocalFile("C:\\Temp\\file8.jpg")
                    },
                };
                yield return Task.FromResult(attachmentMessage);
            }
            else if (message.Attachments.Any())
            {
                var count = 0;
                foreach (var attachment in message.Attachments)
                {
                    var savedFileMessage = SaveFile(attachment, message);
                    yield return savedFileMessage;
                    count++;
                }

                yield return Task.FromResult(message.GetResponse($"Сохранено {count} файлов"));
                var fileResp = message.GetResponse($"Попытка вернуть те же файлы:");
                fileResp.Attachments = message.Attachments;
                yield return Task.FromResult(fileResp);
            }
            else if (!string.IsNullOrEmpty(message.CallbackData))
            {
                yield return Task.FromResult(message.GetResponse($"Ты нажал на кнопку {message.CallbackData}"));
            }
            else
            {
                yield return Task.FromResult(message.GetResponse($"Получено сообщение: {message.Text}"));
            }
        }

        private Task<OutgoingMessage> GetInlineKeyboard(IncomingMessage message)
        {
            var response = message.GetResponse("Тестовые инлайн-кнопки:");
            response.Keyboard = new Keyboard()
            {
                Buttons =
                {
                    new KeyboardButton() {ButtonText = "1-1", CallbackData = "1/1", RowOrder = 1, ColumnOrder = 1},
                    new KeyboardButton() {ButtonText = "1-2", CallbackData = "1/2", RowOrder = 1, ColumnOrder = 2},
                    new KeyboardButton() {ButtonText = "2-1", CallbackData = "2/1", RowOrder = 2, ColumnOrder = 1},
                    new KeyboardButton() {ButtonText = "2-3", CallbackData = "2/3", RowOrder = 2, ColumnOrder = 3},
                    new KeyboardButton() {ButtonText = "4-1", CallbackData = "4/1", RowOrder = 4, ColumnOrder = 1}
                }
            };

            return Task.FromResult(response);
        }




        private async Task<OutgoingMessage> SaveFile(Attachment attachment, IncomingMessage message)
        {
            var file = attachment.File;
            using (var stream = File.Create($"c:\\temp\\{await file.GetNameAsync()}"))
            {
                using (var srcStream = await file.OpenReadStreamAsync())
                {
                    await srcStream.CopyToAsync(stream);
                }
            }

            return message.GetResponse(
                $"Сохранен файл типа {attachment.GetType()} с именем {ReflectMarkdown(await file.GetNameAsync())}");
        }



        string ReflectMarkdown(string message)
        {
            return message;
            //.Replace("_", "\\_")
            //.Replace("*", "\\*")
            //.Replace("[", "\\[")
            //.Replace("'", "\\'");
        }
    }
}