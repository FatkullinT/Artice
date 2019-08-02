using System;

namespace Artice.Vk.Files
{
    public class VkIncomingPlayer : VkIncomingFile
    {
        public VkIncomingPlayer(long fileId, long ownerId, string accessKey) : base(fileId, ownerId, accessKey)
        { }

        public VkIncomingPlayer()
        { }

        public Uri PlayerUri { get; set; }
    }
}