namespace Artice.Core.Models
{
    public class Sticker : Attachment
    {
        public string ChannelId { get; set; }

        public string StickerId { get; set; }

        public string CollectionId { get; set; }
    }
}