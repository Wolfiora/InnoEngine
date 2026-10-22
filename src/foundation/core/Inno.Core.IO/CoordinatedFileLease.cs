using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Core.IO;

/// <summary>
/// Holds a persistent owner gate throughout waiting for and using a retireable leaf lease.
/// </summary>
public sealed class CoordinatedFileLease : IDisposable
{
    private readonly FileLease m_owner;
    private FileLease? m_leaf;

    private CoordinatedFileLease(
        FileLease owner,
        FileLease leaf
    ) {
        m_owner = owner;
        m_leaf = leaf;
    }

    /// <summary>
    /// Acquires owner reading rights before opening or waiting for a leaf lease.
    /// </summary>
    /// <param name="coordinationPath">
    /// The persistent owner gate, never removed by retirement.
    /// </param>
    /// <param name="leafPath">
    /// A distinct leaf path within the same owner boundary.
    /// </param>
    /// <param name="timeout">
    /// The complete wait budget, or infinite.
    /// </param>
    /// <param name="shared">
    /// Whether the leaf permits simultaneous readers.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation while waiting for either gate.
    /// </param>
    /// <returns>
    /// Ownership retaining both gates until disposal.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// A path is relative, the leaf escapes its owner, or the budget is invalid.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// The complete wait budget expires.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels before ownership is returned.
    /// </exception>
    public static async ValueTask<CoordinatedFileLease> AcquireAsync(
        string coordinationPath,
        string leafPath,
        TimeSpan timeout,
        bool shared = false,
        CancellationToken cancellationToken = default
    ) {
        if (!Path.IsPathFullyQualified(coordinationPath) || !Path.IsPathFullyQualified(leafPath))
            throw new ArgumentException("Coordinated leases require absolute owner and leaf paths.");
        string ownerPath = Path.GetFullPath(coordinationPath);
        string leaf = Path.GetFullPath(leafPath);
        string root = Path.GetDirectoryName(ownerPath)!;
        PathBoundary.RequireUnlinkedPath(root, leaf);
        if (string.Equals(ownerPath, leaf, OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            throw new ArgumentException("The leaf must differ from its persistent owner gate.", nameof(leafPath));
        if (timeout < TimeSpan.Zero && timeout != Timeout.InfiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(timeout));
        Stopwatch elapsed = Stopwatch.StartNew();
        if (!File.Exists(ownerPath))
            using (await FileLease.AcquireAsync(ownerPath, Remaining(), cancellationToken).ConfigureAwait(false)) { }
        FileLease owner = await FileLease.AcquireSharedAsync(ownerPath, Remaining(), cancellationToken).ConfigureAwait(false);
        try
        {
            if (shared && !File.Exists(leaf))
                using (await FileLease.AcquireAsync(leaf, Remaining(), cancellationToken).ConfigureAwait(false)) { }
            FileLease acquired = shared
                ? await FileLease.AcquireSharedAsync(leaf, Remaining(), cancellationToken).ConfigureAwait(false)
                : await FileLease.AcquireAsync(leaf, Remaining(), cancellationToken).ConfigureAwait(false);
            return new(owner, acquired);
        }
        catch
        {
            owner.Dispose();
            throw;
        }

        TimeSpan Remaining() => timeout == Timeout.InfiniteTimeSpan ? timeout
            : timeout > elapsed.Elapsed ? timeout - elapsed.Elapsed : TimeSpan.Zero;
    }

    /// <summary>
    /// Releases the leaf before its persistent owner gate; neither file is deleted.
    /// </summary>
    public void Dispose()
    {
        FileLease? leaf = Interlocked.Exchange(ref m_leaf, null);
        if (leaf is null)
            return;
        try
        {
            leaf.Dispose();
        }
        finally
        {
            m_owner.Dispose();
        }
    }
}
