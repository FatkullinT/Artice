using System.IO;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public interface ILocalFile : IFile
    {
        Task<string> GetFilePath();
    }
}