using System;
using System.IO;
using System.Security.Cryptography;
using Inno.Content;
using Inno.Core.IO;

namespace Inno.Adapter.Content.FileSystem;

internal sealed class ContentCacheGeneration
{
    private readonly object m_sync = new();
    private readonly string m_root;
    private FileLease? m_lease;
    private int m_pins = 1;

    internal ContentCacheGeneration(
        string root,
        FileLease lease
    ) {
        m_root = root;
        m_lease = lease;
    }

    internal ContentReadLease Acquire(ContentEntry entry)
    {
        lock (m_sync)
        {
            ObjectDisposedException.ThrowIf(m_lease is null, this);
            m_pins = checked(m_pins + 1);
            return new ContentReadLease(entry, () => OpenPinned(entry), ReleasePin);
        }
    }

    internal void ReleasePin()
    {
        lock (m_sync)
        {
            if (--m_pins == 0)
            {
                m_lease!.Dispose();
                m_lease = null;
            }
        }
    }

    private Stream OpenPinned(ContentEntry entry)
    {
        lock (m_sync)
        {
            ObjectDisposedException.ThrowIf(m_lease is null, this);
            m_pins = checked(m_pins + 1);
        }
        FileStream? input = null;
        try
        {
            input = new FileStream(ContentCacheValidator.Resolve(m_root, entry.key),
                FileMode.Open, FileAccess.Read, FileShare.Read);
            if (input.Length != entry.length
                || !StringComparer.Ordinal.Equals(Convert.ToHexString(SHA256.HashData(input)), entry.contentHash))
                throw new InvalidDataException($"Cached content '{entry.key}' changed after preparation.");
            input.Position = 0;
            return new GenerationReadStream(input, ReleasePin);
        }
        catch
        {
            try
            {
                input?.Dispose();
            }
            finally
            {
                ReleasePin();
            }
            throw;
        }
    }

    private sealed class GenerationReadStream : Stream
    {
        private readonly FileStream m_input;
        private Action? m_release;

        internal GenerationReadStream(
            FileStream input,
            Action release
        ) {
            m_input = input;
            m_release = release;
        }

        /// <inheritdoc />
        public override bool CanRead => m_release is not null;
        /// <inheritdoc />
        public override bool CanSeek => m_release is not null;
        /// <inheritdoc />
        public override bool CanWrite => false;
        /// <inheritdoc />
        public override long Length => m_input.Length;
        /// <inheritdoc />
        public override long Position
        {
            get => m_input.Position;
            set => m_input.Position = value;
        }

        /// <inheritdoc />
        public override int Read(
            byte[] buffer,
            int offset,
            int count
        ) => m_input.Read(buffer, offset, count);
        /// <inheritdoc />
        public override int Read(Span<byte> buffer) => m_input.Read(buffer);
        /// <inheritdoc />
        public override int ReadByte() => m_input.ReadByte();
        /// <inheritdoc />
        public override long Seek(
            long offset,
            SeekOrigin origin
        ) => m_input.Seek(offset, origin);
        /// <inheritdoc />
        public override void Flush() => throw new NotSupportedException();
        /// <inheritdoc />
        public override void SetLength(long value) => throw new NotSupportedException();
        /// <inheritdoc />
        public override void Write(
            byte[] buffer,
            int offset,
            int count
        ) => throw new NotSupportedException();

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Action? release = System.Threading.Interlocked.Exchange(ref m_release, null);
                if (release is not null)
                {
                    try
                    {
                        m_input.Dispose();
                    }
                    finally
                    {
                        release();
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
