using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Coordinates artifact replacement or retirement with every cooperating reader and writer.
/// </summary>
public sealed class BuildArtifactWriteLease : IDisposable
{
    private FileLease? m_lease;

    private BuildArtifactWriteLease(FileLease lease) => m_lease = lease;

    /// <summary>
    /// Waits for all readers before allowing mutation of one immutable artifact directory.
    /// </summary>
    /// <param name="directory">
    /// The absolute artifact location controlled by the publisher.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels this waiter without disturbing existing readers.
    /// </param>
    /// <returns>
    /// Exclusive protection; retain it through replacement and release it before acquiring a reading lease.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The directory is empty or relative.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Other owners retain the artifact for more than thirty seconds.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Waiting was canceled.
    /// </exception>
    public static async ValueTask<BuildArtifactWriteLease> AcquireAsync(
        string directory,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        if (!Path.IsPathFullyQualified(directory))
            throw new ArgumentException("Artifact mutation requires an absolute owner path.", nameof(directory));
        return new(await FileLease.AcquireAsync(BuildArtifactReadLease.GetLeasePath(directory),
            TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false));
    }

    /// <summary>
    /// Ends exclusive access without removing the persistent coordination file.
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref m_lease, null)?.Dispose();
}
