using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public class OutgoingLocalFile : IOutgoingFile, ILocalFile
    {
        public string FilePath { get; }

        public OutgoingLocalFile(string filePath)
        {
            FilePath = filePath;
        }

        public Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Path.GetFileName(FilePath));
        }

        public Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((Stream)File.OpenRead(FilePath));
        }

        public Task<string> GetFilePath()
        {
            return Task.FromResult(FilePath);
        }
    }
}