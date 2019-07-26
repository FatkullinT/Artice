namespace Artice.Core.Models
{
	public class OutgoingMessage : Message
	{
		public User To { get; set; }

		public Group Group { get; set; }

		public InlineKeyboard InlineKeyboard { get; set; }

		public GalleryWidget Gallery { get; set; }
	}
}