using System;
using System.Collections.Generic;
using Artice.LogicCore.Context.Args;

namespace Artice.LogicCore.Context
{
    public class ChatContext
    {
        public Recipient Recipient { get; }

        public DateTime CreatedAt { get; }

        public DateTime LastRequestAt { get; private set; }

        public event EventHandler<ContextDeleteEventArgs> Delete;

        private readonly Dictionary<string, object> _parameters;

        public ChatContext(Recipient recipient)
        {
            _parameters = new Dictionary<string, object>();
            Recipient = recipient;
            CreatedAt = DateTime.UtcNow;
            LastRequestAt = DateTime.UtcNow;
        }

        public void UpdateLastRequestTime()
        {
            LastRequestAt = DateTime.UtcNow;
        }

        public T Get<T>(string key)
        {
            if (_parameters.ContainsKey(key) && _parameters[key] is T)
            {
                return (T)_parameters[key];
            }
            return default(T);
        }

        public void Set(string key, object value)
        {
            if (value == null)
            {
                if (_parameters.ContainsKey(key))
                {
                    _parameters.Remove(key);
                }
            }
            else
            {
                if (_parameters.ContainsKey(key))
                {
                    _parameters[key] = value;
                }
                else
                {
                    _parameters.Add(key, value);
                }
            }
        }

        internal void OnDelete(ContextDeleteEventArgs e)
        {
            Delete?.Invoke(this, e);
        }
    }
}