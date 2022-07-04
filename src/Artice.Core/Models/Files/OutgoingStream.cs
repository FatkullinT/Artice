using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public class OutgoingStream : IOutgoingFile
    {
        private readonly string _fileName;
        private readonly Func<Task<Stream>> _createStreamAsyncFunc;

        public OutgoingStream(string fileName, Func<Task<Stream>> streamAsyncFunc)
        {
            _fileName = fileName;
            _createStreamAsyncFunc = streamAsyncFunc;
        }

        public Task<string> GetNameAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_fileName);
        }

        public Task<Stream> OpenReadStreamAsync(CancellationToken cancellationToken = default)
        {
            return _createStreamAsyncFunc();
        }
    }
}