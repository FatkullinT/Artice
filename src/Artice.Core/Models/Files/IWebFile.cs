using System;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public interface IWebFile : IFile
    {
        Task<Uri> GetFileUriAsync();
    }
}