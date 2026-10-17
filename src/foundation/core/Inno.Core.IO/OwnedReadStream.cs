using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Core.IO;

/// <summary>
/// Keeps an independent read stream and its lifetime pin owned together until both retire.
/// </summary>
public sealed class OwnedReadStream : Stream
{
    private readonly object m_sync = new();
    private readonly Stream m_input;
    private IDisposable? m_owner;
    private bool m_inputClosed;

    /// <summary>
    /// Transfers a readable stream and its independent lifetime pin into this wrapper.
    /// </summary>
    /// <param name="input">
    /// The owned readable stream; it is closed before its pin is released.
    /// </param>
    /// <param name="owner">
    /// The owned pin whose repeated disposal must safely retry unfinished retirement.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The stream is not readable.
    /// </exception>
    public OwnedReadStream(
        Stream input,
        IDisposable owner
    ) {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(owner);
        if (!input.CanRead)
            throw new ArgumentException("An owned read stream requires readable input.", nameof(input));
        m_input = input;
        m_owner = owner;
    }

    /// <inheritdoc />
    public override bool CanRead => !m_inputClosed && m_input.CanRead;
    /// <inheritdoc />
    public override bool CanSeek => !m_inputClosed && m_input.CanSeek;
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
    public override ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default
    ) => m_input.ReadAsync(buffer, cancellationToken);
    /// <inheritdoc />
    public override Task<int> ReadAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken
    ) => m_input.ReadAsync(buffer, offset, count, cancellationToken);
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
            lock (m_sync)
            {
                if (!m_inputClosed)
                {
                    m_input.Dispose();
                    m_inputClosed = true;
                }
                m_owner?.Dispose();
                m_owner = null;
            }
        }
        base.Dispose(disposing);
    }
}
