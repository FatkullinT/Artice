using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Artice.Core.Models.Files
{
    public class ResponseMessageReadStream : Stream
    {
        private readonly Stream _inner;
        private readonly IDisposable _externalDisposable;


        public ResponseMessageReadStream(Stream innerStream, IDisposable externalDisposable)
        {
            _inner = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
            _externalDisposable = externalDisposable ?? throw new ArgumentNullException(nameof(externalDisposable)); ;

        }

        public override bool CanRead => _inner.CanRead;

        public override bool CanSeek => _inner.CanSeek;

        public override bool CanWrite => false;

        public override long Length => _inner.Length;

        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }


        public override long Seek(long offset, SeekOrigin origin)
        {
            return _inner.Seek(offset, origin);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _inner.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(
          byte[] buffer,
          int offset,
          int count,
          CancellationToken cancellationToken)
        {
            return _inner.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override Task WriteAsync(
          byte[] buffer,
          int offset,
          int count,
          CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            _inner.Dispose();
            _externalDisposable.Dispose();
        }
    }
}