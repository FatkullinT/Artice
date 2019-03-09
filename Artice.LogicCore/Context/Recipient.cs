using System;

namespace Artice.LogicCore.Context
{
	public class Recipient
	{
		public readonly string BotName;

		public readonly string RecipientId;

		public readonly RecipientType RecipientType;

		public Recipient(string botName, string recipientId, RecipientType type)
		{
			BotName = botName;
			RecipientId = recipientId;
			RecipientType = type;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Recipient other))
				return false;

			return string.Equals(other.BotName, BotName, StringComparison.InvariantCulture)
				   && string.Equals(other.RecipientId, RecipientId, StringComparison.InvariantCulture)
				   && other.RecipientId == RecipientId;
		}

		public override int GetHashCode()
		{
			return (BotName, RecipientId, RecipientType).GetHashCode();
		}
	}
}