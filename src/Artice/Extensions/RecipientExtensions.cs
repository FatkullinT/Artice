using System;
using Artice.Context;
using Artice.Core.Models;

namespace Artice.Extensions
{
    public static class RecipientExtensions
    {
        public static OutgoingMessage CreateOutgoingMessage(this Recipient recipient, string text)
        {
            var message = new OutgoingMessage() { Text = text };
            switch (recipient.RecipientType)
            {
                case RecipientType.Chat:
                    {
                        message.Chat = new Chat() { Id = recipient.RecipientId };
                        break;
                    }
                case RecipientType.User:
                    {
                        message.To = new User() { Id = recipient.RecipientId };
                        break;
                    }
                default:
                    {
                        throw new Exception("Unknown RecipientType");
                    }
            }
            return message;
        }
    }
}