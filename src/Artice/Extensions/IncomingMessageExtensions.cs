using Artice.Core.Models;

namespace Artice.Extensions
{
    public static class IncomingMessageExtensions
    {
        public static OutgoingMessage GetResponse(this IncomingMessage incomingMessage, string responseText)
        {
            return new OutgoingMessage()
            {
                Chat = incomingMessage.Chat,
                To = incomingMessage.From,
                Text = responseText
            };
        }
    }
}