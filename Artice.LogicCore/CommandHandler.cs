using System.Threading.Tasks;
using Artice.Core.Bots;
using Artice.Core.Models;
using Artice.LogicCore.Context;

namespace Artice.LogicCore
{
    public abstract class CommandHandler
    {
        protected readonly ChatContext Context;

        protected readonly IOutgoingMessageProvider OutgoingMessageProvider;

        protected CommandHandler(ChatContext context, IOutgoingMessageProvider outgoingMessageProvider)
        {
            Context = context;
            OutgoingMessageProvider = outgoingMessageProvider;
        }

        public abstract Task<OutgoingMessage> Handle(IncomingMessage incomingMessage);

        /// <summary>
        /// Reset chat context and clean all data created by this command handler
        /// </summary>
        public virtual void Delete()
        {
        }
    }
}