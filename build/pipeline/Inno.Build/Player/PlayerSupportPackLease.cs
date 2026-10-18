using System;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build;

/// <summary>
/// Keeps verified Player compilation and linker inputs alive until their consumer finishes.
/// </summary>
public sealed class PlayerSupportPackLease : IDisposable
{
    private readonly string m_directory;
    private FileLease? m_lease;

    internal PlayerSupportPackLease(
        string directory,
        FileLease lease
    ) {
        m_directory = directory;
        m_lease = lease;
    }

    /// <summary>
    /// Gets the verified absolute directory while this lease owns reading rights.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The consumer has already released its reading rights.
    /// </exception>
    public string directory
    {
        get
        {
            ObjectDisposedException.ThrowIf(m_lease is null, this);
            return m_directory;
        }
    }

    /// <summary>
    /// Releases reading rights after all compilation, copying and linking have completed.
    /// Repeated disposal has no effect.
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref m_lease, null)?.Dispose();
}
