using System;
using System.IO;

namespace Inno.Content;

/// <summary>
/// Pins immutable entry bytes while independent streams obtain their own retirement protection.
/// </summary>
public sealed class ContentReadLease : IDisposable
{
    private readonly object m_sync = new();
    private Func<Stream>? m_openRead;
    private Action? m_release;

    /// <summary>
    /// Transfers a provider pin and independent-stream factory into a caller-owned lease.
    /// </summary>
    /// <param name="entry">
    /// Immutable metadata identifying the pinned bytes.
    /// </param>
    /// <param name="openRead">
    /// Opens a stream with its own provider pin; stream disposal must release that pin.
    /// </param>
    /// <param name="release">
    /// Releases this lease's provider pin exactly once, independently of any open stream.
    /// </param>
    public ContentReadLease(
        ContentEntry entry,
        Func<Stream> openRead,
        Action release
    ) {
        this.entry = entry ?? throw new ArgumentNullException(nameof(entry));
        m_openRead = openRead ?? throw new ArgumentNullException(nameof(openRead));
        m_release = release ?? throw new ArgumentNullException(nameof(release));
    }

    /// <summary>
    /// Gets the immutable metadata, which does not retain a source or extension generation.
    /// </summary>
    public ContentEntry entry { get; }

    /// <summary>
    /// Opens a new independently pinned stream without transferring this lease's ownership.
    /// </summary>
    /// <returns>
    /// A caller-owned stream; disposing this lease or its store does not invalidate the stream.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// This lease has already released its pin.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The provider did not return a readable stream.
    /// </exception>
    public Stream OpenRead()
    {
        lock (m_sync)
        {
            Func<Stream> openRead = m_openRead ?? throw new ObjectDisposedException(nameof(ContentReadLease));
            Stream input = openRead()
                ?? throw new InvalidOperationException("The content provider returned a null stream.");
            if (!input.CanRead)
            {
                input.Dispose();
                throw new InvalidOperationException("The content provider returned an unreadable stream.");
            }
            return input;
        }
    }

    /// <summary>
    /// Releases this lease's pin once; existing streams retain their independent pins.
    /// </summary>
    public void Dispose()
    {
        Action? release;
        lock (m_sync)
        {
            release = m_release;
            m_release = null;
            m_openRead = null;
        }
        release?.Invoke();
    }
}
