using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Pins a verified immutable build artifact against cooperating repair and retirement operations.
/// </summary>
public sealed class BuildArtifactReadLease : IDisposable
{
    private CoordinatedFileLease? m_lease;

    private BuildArtifactReadLease(CoordinatedFileLease lease) => m_lease = lease;

    /// <summary>
    /// Acquires reading ownership and verifies the complete artifact before exposing it to a consumer.
    /// </summary>
    /// <param name="ownerDirectory">
    /// The exact publisher root retaining the persistent owner.lock gate.
    /// </param>
    /// <param name="directory">
    /// The absolute artifact directory owned by its publisher; its persistent lease is stored alongside it.
    /// </param>
    /// <param name="fingerprint">
    /// The exact input identity required by the consumer.
    /// </param>
    /// <param name="outputDirectories">
    /// The complete relative output roots declared by the publisher.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels ownership acquisition without releasing another reader's protection.
    /// </param>
    /// <param name="statistics">
    /// Optional borrowed operation receiving output-integrity read statistics; its input cache is not used.
    /// </param>
    /// <returns>
    /// Reading ownership which the caller must retain until all artifact access completes.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The directory is not absolute or a required identity is blank.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The complete artifact does not match the requested identity and output inventory.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Repair or retirement retains exclusive ownership for more than thirty seconds.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Waiting was canceled before ownership could be returned.
    /// </exception>
    public static async ValueTask<BuildArtifactReadLease> AcquireAsync(
        string directory,
        string ownerDirectory,
        string fingerprint,
        IReadOnlyList<string> outputDirectories,
        CancellationToken cancellationToken = default,
        NativeBuildContext? statistics = null
    ) => await TryAcquireAsync(directory, ownerDirectory, fingerprint, outputDirectories, statistics, cancellationToken).ConfigureAwait(false)
        ?? throw new InvalidDataException($"Build artifact '{directory}' is missing, damaged or has a different identity.");

    /// <summary>
    /// Ends artifact access and allows repair or retirement; repeated disposal has no effect.
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref m_lease, null)?.Dispose();

    internal static async ValueTask<BuildArtifactReadLease?> TryAcquireAsync(
        string directory,
        string ownerDirectory,
        string fingerprint,
        IReadOnlyList<string> outputDirectories,
        NativeBuildContext? context,
        CancellationToken cancellationToken
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        ArgumentException.ThrowIfNullOrWhiteSpace(fingerprint);
        ArgumentNullException.ThrowIfNull(outputDirectories);
        if (!Path.IsPathFullyQualified(directory))
            throw new ArgumentException("Artifact reading requires an absolute owner path.", nameof(directory));
        string path = GetLeasePath(directory);
        BuildCacheCleanupPlan.RequireChild(ownerDirectory, directory);
        CoordinatedFileLease lease = await CoordinatedFileLease.AcquireAsync(Path.Combine(ownerDirectory, "owner.lock"),
            path, TimeSpan.FromSeconds(30), shared: true, cancellationToken: cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!BuildArtifactManifest.IsComplete(directory, fingerprint, outputDirectories, context))
            {
                lease.Dispose();
                return null;
            }
            return new(lease);
        }
        catch
        {
            lease.Dispose();
            throw;
        }
    }

    internal static string GetLeasePath(string directory)
        => Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory)) + ".read.lock";
}
