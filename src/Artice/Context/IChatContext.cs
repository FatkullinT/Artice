using System;
using Artice.Context.Args;

namespace Artice.Context
{
	/// <summary>
	/// Context of dialog with concrete user or concrete chat.
	/// </summary>
	public interface IChatContext
	{
		/// <summary>
		/// Group or user which is communicating.
		/// </summary>
		Recipient Recipient { get; }

		/// <summary>
		/// Context making time.
		/// </summary>
		DateTime CreatedAt { get; }

		/// <summary>
		/// Time of last context request.
		/// </summary>
		DateTime LastRequestAt { get; }

		/// <summary>
		/// On context delete event.
		/// </summary>
		event EventHandler<ContextDeleteEventArgs> OnDelete;

		/// <summary>
		/// Get saved additional info about chatting.
		/// </summary>
		T Get<T>(string key);

		/// <summary>
		/// Set additional info about chatting to save between messages.
		/// </summary>
		void Set(string key, object value);
	}
}