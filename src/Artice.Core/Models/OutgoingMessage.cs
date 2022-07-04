namespace Artice.Core.Models
{
	public class OutgoingMessage : Message
	{
		public User To { get; set; }

		public Group Group { get; set; }

		public Keyboard Keyboard { get; set; }
    }
}