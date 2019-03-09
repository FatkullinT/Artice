using System;
using Artice.Core.Bots;
using Artice.Core.Models;

namespace Artice.Core.Args
{
    public class UpdateEventArgs : EventArgs
    {
        public IncomingMessage[] Messages { get; private set; }

        public IOutgoingMessageProvider OutgoingMessageProvider { get; private set; }

        public UpdateEventArgs(IncomingMessage[] messages, IOutgoingMessageProvider outgoingMessageProvider)
        {
            OutgoingMessageProvider = outgoingMessageProvider;
            Messages = messages;
        }
    }
}