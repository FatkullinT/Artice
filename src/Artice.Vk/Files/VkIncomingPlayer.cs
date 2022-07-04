using System;

namespace Artice.Vk.Files
{
    public class VkIncomingPlayer : VkIncomingFile
    {
        public VkIncomingPlayer(string fileType, long fileId, long ownerId, string accessKey)
            : base(fileType, fileId, ownerId, accessKey)
        { }

        public VkIncomingPlayer()
        { }

        public Uri PlayerUri { get; set; }
    }
}