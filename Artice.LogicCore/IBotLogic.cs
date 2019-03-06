using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artice.Core.Bots;
using Artice.Core.Models;
using Artice.LogicCore.Context;

namespace Artice.LogicCore
{
    public interface IBotLogic : IDisposable
    {
        IEnumerable<SchedullerTask> Tasks { get; }

        void Initialize(ContextStorage contextStorage);

        Task<OutgoingMessage> Answer(IChatBot chatBot, IncomingMessage message, ChatContext context);

        void OnError(IChatBot chatBot, IncomingMessage message, Exception exception);
    }
}