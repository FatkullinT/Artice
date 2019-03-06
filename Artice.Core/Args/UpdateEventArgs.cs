using System;
using Artice.Core.Bots;
using Artice.Core.Models;

namespace Artice.Core.Args
{
    public class UpdateEventArgs : EventArgs
    {
        public IncomingMessage[] Messages { get; private set; }

        public IChatBot ChatBot { get; private set; }

        public UpdateEventArgs(IncomingMessage[] messages, IChatBot chatBot)
        {
            ChatBot = chatBot;
            Messages = messages;
        }
    }
}