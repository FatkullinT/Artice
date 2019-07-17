using System;
using System.IO;
using Artice.Core.Models.Enum;

namespace Artice.Core.Models
{
    public class FileReference
    {
        public FileReference(Stream content)
        {
            Content = content;
        }

        public FileReference(Uri url)
        {
            Url = url;
        }

        public FileReference(string id)
        {
            Id = id;
        }

        public FileReference()
        {
        }

        public string Id { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        //[ScriptIgnore]
        public Stream Content { get; set; }

        public Uri Url { get; set; }

        public Uri PlayerUrl { get; set; }

        //[ScriptIgnore]
        public FileReferenceType ReferenceType
        {
            get
            {
                if (Content != null)
                {
                    return FileReferenceType.Content;
                }
                if (Url != null)
                {
                    return FileReferenceType.Url;
                }
                if (!string.IsNullOrEmpty(Id))
                {
                    return FileReferenceType.Id;
                }
                if (PlayerUrl != null)
                {
                    return FileReferenceType.PlayerUrl;
                }
                return FileReferenceType.Unknown;
            }
        }
    }
}