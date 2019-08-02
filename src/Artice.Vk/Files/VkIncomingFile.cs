﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Artice.Core.Models.Files;

namespace Artice.Vk.Files
{
    public class VkIncomingFile : IIncomingFile
    {
        public long FileId { get; set; }

        public long OwnerId { get; set; }

        public VkIncomingFile(long fileId, long ownerId = 0)
        {
        }

        public VkIncomingFile() : this(0, 0)
        { }

        public virtual Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult($"{GetType().Name}_{OwnerId}_{FileId}");
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