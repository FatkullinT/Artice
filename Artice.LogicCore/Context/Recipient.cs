namespace Artice.LogicCore.Context
{
    public struct Recipient
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
    }
}