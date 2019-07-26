using System;
using System.Collections.Generic;
using Artice.Context.Args;

namespace Artice.Context
{
	/// <summary>
	/// Context of dialog with concrete user or concrete chat.
	/// </summary>
	public class ChatContext : IChatContext
	{
		private readonly Dictionary<string, object> _parameters;

		public ChatContext(Recipient recipient)
		{
			_parameters = new Dictionary<string, object>();
			Recipient = recipient;
			CreatedAt = DateTime.UtcNow;
			LastRequestAt = DateTime.UtcNow;
		}

		/// <summary>
		/// Group or user which is communicating.
		/// </summary>
		public Recipient Recipient { get; }

		/// <summary>
		/// Context making time.
		/// </summary>
		public DateTime CreatedAt { get; }

		/// <summary>
		/// Time of last context request.
		/// </summary>
		public DateTime LastRequestAt { get; private set; }

		/// <summary>
		/// On context delete event.
		/// </summary>
		public event EventHandler<ContextDeleteEventArgs> OnDelete;

		/// <summary>
		/// Get saved additional info about chatting.
		/// </summary>
		public T Get<T>(string key)
		{
			if (_parameters.ContainsKey(key) && _parameters[key] is T)
			{
				return (T)_parameters[key];
			}
			return default;
		}

		/// <summary>
		/// Set additional info about chatting to save between messages.
		/// </summary>
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

		internal void UpdateLastRequestTime()
		{
			LastRequestAt = DateTime.UtcNow;
		}

		internal void OnDeleteInvoke(ContextDeleteEventArgs e)
		{
			OnDelete?.Invoke(this, e);
		}
	}
}