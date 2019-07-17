using System.Collections.Generic;

namespace Artice.Core.Models
{
    public class GalleryWidget
    {
        public GalleryWidget()
        {
            Elements = new List<GalleryElement>();
        }

        public List<GalleryElement> Elements { get; set; }
    }
}