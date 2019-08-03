using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models.Files;

namespace Artice.Vk.Files
{
    public class VkIncomingFile : IIncomingFile
    {
        public string FileType { get; set; }

        public long FileId { get; set; }

        public long OwnerId { get; set; }

        public string AccessKey { get; set; }

        public string FileName { get; set; }

        public VkIncomingFile(string fileType, long fileId, long ownerId, string accessKey)
        {
            FileType = fileType;
            FileId = fileId;
            OwnerId = ownerId;
            AccessKey = accessKey;
        }

        public VkIncomingFile() : this(null, 0, 0, null)
        { }

        public virtual Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(FileName ?? $"{GetType().Name}_{OwnerId}_{FileId}");
        }

        public virtual Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Stream.Null);
        }

        public virtual Task<long> GetFileSizeAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((long)0);
        }

        public virtual Task<string> GetMimeTypeAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(string.Empty);
        }
    }
}