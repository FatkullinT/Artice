namespace Artice.Telegram
{
	public static class Consts
	{
		public const string ChannelId = "telegram";
		public const string BaseUrl = "https://api.telegram.org";
		public const string ApiPath = "/bot";
		public const string FilePath = "/file/bot";
		public const int SendingPerSecondLimit = 28;
        public const int LongPoolingTimeout = 25;
    }
}