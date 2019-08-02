using System;

namespace Artice.Vk.Files
{
    public class VkIncomingPlayer : VkIncomingFile
    {
        public VkIncomingPlayer(long fileId, long ownerId = 0) : base(fileId, ownerId)
        { }

        public VkIncomingPlayer()
        { }

        public Uri PlayerUri { get; set; }
    }
}