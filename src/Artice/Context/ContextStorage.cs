using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Artice.Context.Args;

namespace Artice.Context
{
	public class ContextStorage : IContextStorage
	{
        private readonly Dictionary<Recipient, ChatContext> _contexts;

        private readonly Timer _cleanTimer;

        private readonly TimeSpan _contextLifeTime;

        public IEnumerable<ChatContext> Contexts => _contexts.Values;

        public ContextStorage()
        {
            _contexts = new Dictionary<Recipient, ChatContext>();
			_contextLifeTime = new TimeSpan(0, 10, 0);
			_cleanTimer = new Timer(180000);
            _cleanTimer.Elapsed += Clean;
            _cleanTimer.Start();
        }

        private void Clean(object sender, ElapsedEventArgs e)
        {
            List<ChatContext> contextRemoveList =
                _contexts.Values.Where(c => DateTime.UtcNow - c.LastRequestAt > _contextLifeTime)
                    .ToList();
            foreach (var context in contextRemoveList)
            {
                _contexts.Remove(context.Recipient);
                context.OnDeleteInvoke(new ContextDeleteEventArgs(ContextDeleteReason.Timeout));
            }
        }

        public IChatContext Get(Recipient recipient)
        {
            ChatContext context;
            if (_contexts.ContainsKey(recipient))
            {
                context = _contexts[recipient];
                context.UpdateLastRequestTime();
            }
            else
            {
                context = new ChatContext(recipient);
                _contexts.Add(recipient, context);

            }
            return context;
        }

        public IChatContext Get(string botName, RecipientType recipientType, string recipientId)
        {
            return Get(new Recipient(botName, recipientId, recipientType));
        }

        public void Dispose()
        {
            foreach (var context in _contexts.Values)
            {
                context.OnDeleteInvoke(new ContextDeleteEventArgs(ContextDeleteReason.StorageDispose));
            }
            _contexts.Clear();
            _cleanTimer.Stop();
        }
    }
}