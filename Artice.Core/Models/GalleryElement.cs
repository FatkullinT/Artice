namespace Artice.Core.Models
{
    public class GalleryElement
    {
        public int Order { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public InlineKeyboard InlineKeyboard { get; set; }
    }
}