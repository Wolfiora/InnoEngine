using System;
using System.IO;

namespace Inno.Core.IO;

/// <summary>
/// Provides rollback-safe installation of complete directory trees.
/// </summary>
public static class AtomicDirectory
{
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
            Directory.Move(target, backup);
        try
        {
            Directory.Move(candidate, target);
        }
        catch (Exception installationFailure)
        {
            if (Directory.Exists(backup))
            {
                try
                {
                    Directory.Move(backup, target);
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
