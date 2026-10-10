using System;
using System.IO;
using System.Threading;

namespace Inno.Core.IO;

/// <summary>
/// Provides rollback-safe installation of complete directory trees.
/// </summary>
public static class AtomicDirectory
{
    /// <summary>
    /// Publishes a complete directory at an unoccupied path without replacing an existing destination.
    /// </summary>
    /// <param name="source">
    /// The complete candidate directory, disjoint from the destination.
    /// </param>
    /// <param name="destination">
    /// The unoccupied publication path on the same filesystem.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation or bounded waits before the rename commits.
    /// </param>
    /// <remarks>
    /// Windows access and sharing failures are retried for at most two seconds to tolerate temporary readers.
    /// Failure preserves the candidate and any existing destination. A committed rename is not canceled afterward.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// A path is blank or the two directory trees overlap.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The source directory does not exist.
    /// </exception>
    /// <exception cref="IOException">
    /// The destination exists, the filesystem cannot rename the tree, or a transient block persists.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The operating system continues denying access after the bounded retry window.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Cancellation is requested before the directory is published.
    /// </exception>
    public static void Publish(
        string source,
        string destination,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentException.ThrowIfNullOrWhiteSpace(source);
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);
        string candidate = Path.GetFullPath(source);
        string target = Path.GetFullPath(destination);
        if (IsContained(Path.GetRelativePath(candidate, target))
            || IsContained(Path.GetRelativePath(target, candidate)))
            throw new ArgumentException("The candidate and destination directory trees must be disjoint.", nameof(destination));
        if (!Directory.Exists(candidate))
            throw new DirectoryNotFoundException($"Directory candidate '{candidate}' does not exist.");
        string? parent = Path.GetDirectoryName(target);
        if (string.IsNullOrEmpty(parent))
            throw new IOException($"Directory path '{target}' has no owning directory.");
        Directory.CreateDirectory(parent);
        FileSystemRename.MoveDirectory(candidate, target, cancellationToken);
    }

    /// <summary>
    /// Installs a disjoint candidate tree and restores the previous destination if installation fails.
    /// </summary>
    /// <param name="source">
    /// The complete candidate directory.
    /// </param>
    /// <param name="destination">
    /// The destination directory.
    /// </param>
    /// <remarks>
    /// The caller must serialize readers and writers while the two directory moves run.
    /// Backup cleanup runs after commit; its failure preserves the installed candidate and remaining backup.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Either path is blank, or the candidate and destination trees overlap.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The candidate directory does not exist.
    /// </exception>
    /// <exception cref="IOException">
    /// Installation fails, or a committed installation's backup cannot be removed.
    /// A cleanup failure explicitly identifies the installed destination and remaining backup.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The operating system denies access before the candidate can be installed.
    /// </exception>
    /// <exception cref="AggregateException">
    /// Installation and restoration both fail; neither tree is deleted to force a rollback.
    /// </exception>
    public static void Install(
        string source,
        string destination
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(source);
        ArgumentException.ThrowIfNullOrWhiteSpace(destination);
        string candidate = Path.GetFullPath(source);
        string target = Path.GetFullPath(destination);
        string relativeTarget = Path.GetRelativePath(candidate, target);
        string relativeCandidate = Path.GetRelativePath(target, candidate);
        if (IsContained(relativeTarget) || IsContained(relativeCandidate))
            throw new ArgumentException("The candidate and destination directory trees must be disjoint.", nameof(destination));
        if (!Directory.Exists(candidate))
            throw new DirectoryNotFoundException($"Atomic directory candidate '{candidate}' does not exist.");
        string? parent = Path.GetDirectoryName(target);
        if (string.IsNullOrEmpty(parent))
            throw new IOException($"Directory path '{target}' has no owning directory.");
        Directory.CreateDirectory(parent);

        string backup = target + ".backup-" + Guid.NewGuid().ToString("N");
        if (Directory.Exists(target))
            FileSystemRename.MoveDirectory(target, backup);
        try
        {
            FileSystemRename.MoveDirectory(candidate, target);
        }
        catch (Exception installationFailure)
        {
            if (Directory.Exists(backup))
            {
                try
                {
                    FileSystemRename.MoveDirectory(backup, target);
                }
                catch (Exception restorationFailure)
                {
                    throw new AggregateException(
                        $"Directory installation failed; the previous tree remains at '{backup}' and could not be restored to '{target}'.",
                        installationFailure,
                        restorationFailure);
                }
            }
            throw;
        }

        if (!Directory.Exists(backup))
            return;
        try
        {
            Directory.Delete(backup, recursive: true);
        }
        catch (Exception cleanupFailure) when (cleanupFailure is IOException or UnauthorizedAccessException)
        {
            throw new IOException(
                $"Directory '{target}' was installed, but backup cleanup failed. The remaining backup is at '{backup}'.",
                cleanupFailure);
        }
    }

    private static bool IsContained(string relativePath)
        => !Path.IsPathRooted(relativePath)
           && relativePath != ".."
           && !relativePath.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal);
}
