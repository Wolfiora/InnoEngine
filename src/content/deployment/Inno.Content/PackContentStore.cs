using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Inno.Content;

/// <summary>
/// Owns one validated archive with independent entry cursors and synchronized shared-stream positioning.
/// </summary>
public sealed class PackContentStore : IRuntimeContentStore
{
    private readonly object m_sync = new();
    private readonly ZipArchive m_archive;
    private readonly Dictionary<ContentKey, ZipArchiveEntry> m_entries;
    private int m_pins = 1;
    private bool m_closed;

    internal PackContentStore(
        ZipArchive archive,
        ContentPackDescriptor descriptor,
        ContentPackIndex index,
        Dictionary<ContentKey, ZipArchiveEntry> entries
    ) {
        m_archive = archive;
        m_entries = entries;
        this.descriptor = descriptor;
        this.index = index;
    }

    /// <inheritdoc />
    public ContentPackDescriptor descriptor { get; }

    /// <inheritdoc />
    public ContentPackIndex index { get; }

    /// <inheritdoc />
    public ContentReadLease Acquire(ContentKey key)
    {
        lock (m_sync)
        {
            ObjectDisposedException.ThrowIf(m_closed, this);
            if (!index.TryGetEntry(key, out ContentEntry? entry))
                throw new FileNotFoundException($"Content '{key}' does not exist in pack '{descriptor.contentHash}'.");
            m_pins = checked(m_pins + 1);
            return new ContentReadLease(entry!, () => OpenPinned(key), ReleasePin);
        }
    }

    /// <summary>
    /// Stops issuing leases and closes the archive after every existing lease and stream releases its pin.
    /// </summary>
    public void Dispose()
    {
        lock (m_sync)
        {
            if (m_closed)
                return;
            m_closed = true;
            ReleasePin();
        }
    }

    private Stream OpenPinned(ContentKey key)
    {
        lock (m_sync)
        {
            Stream input = m_entries[key].Open();
            m_pins = checked(m_pins + 1);
            return new EntryReadStream(this, input);
        }
    }

    private void ReleasePin()
    {
        lock (m_sync)
        {
            if (--m_pins == 0)
                m_archive.Dispose();
        }
    }

    private sealed class EntryReadStream : Stream
    {
        private readonly PackContentStore m_owner;
        private Stream? m_input;

        internal EntryReadStream(
            PackContentStore owner,
            Stream input
        ) {
            m_owner = owner;
            m_input = input;
        }

        /// <inheritdoc />
        public override bool CanRead => m_input is not null;
        /// <inheritdoc />
        public override bool CanSeek => false;
        /// <inheritdoc />
        public override bool CanWrite => false;
        /// <inheritdoc />
        public override long Length => throw new NotSupportedException();
        /// <inheritdoc />
        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override int Read(
            byte[] buffer,
            int offset,
            int count
        ) => Read(buffer.AsSpan(offset, count));

        /// <inheritdoc />
        public override int Read(Span<byte> buffer)
        {
            lock (m_owner.m_sync)
                return (m_input ?? throw new ObjectDisposedException(nameof(EntryReadStream))).Read(buffer);
        }

        /// <inheritdoc />
        public override int ReadByte()
        {
            lock (m_owner.m_sync)
                return (m_input ?? throw new ObjectDisposedException(nameof(EntryReadStream))).ReadByte();
        }

        /// <inheritdoc />
        public override void Flush() => throw new NotSupportedException();
        /// <inheritdoc />
        public override long Seek(
            long offset,
            SeekOrigin origin
        ) => throw new NotSupportedException();
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
                lock (m_owner.m_sync)
                {
                    if (m_input is not null)
                    {
                        try
                        {
                            m_input.Dispose();
                        }
                        finally
                        {
                            m_input = null;
                            m_owner.ReleasePin();
                        }
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
