namespace Artice.Core.Models
{
    public abstract class Message
    {
        public string Text { get; set; }

        public Attachment[] Attachments { get; set; }
    }
}