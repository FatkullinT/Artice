using System;
using System.Threading.Tasks;
using Artice.Core.Bots;
using Artice.Core.Models;
using Artice.LogicCore.Context;

namespace Artice.LogicCore
{
    public interface ILogic : IDisposable
    {
        Task<OutgoingMessage> Answer(IOutgoingMessageProvider outgoingMessageProvider, IncomingMessage message, ChatContext context);
	}
}