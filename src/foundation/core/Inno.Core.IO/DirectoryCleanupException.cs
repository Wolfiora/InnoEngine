using System;
using System.IO;

namespace Inno.Core.IO;

/// <summary>
/// Reports backup cleanup failure after a complete directory installation has already committed.
/// The installed tree remains authoritative and must not be treated as a failed installation.
/// </summary>
public sealed class DirectoryCleanupException : IOException
{
    internal DirectoryCleanupException(
        string installedDirectory,
        string backupDirectory,
        Exception cleanupFailure
    ) : base($"Directory '{installedDirectory}' was installed, but backup cleanup failed. "
        + $"The remaining backup is at '{backupDirectory}'.", cleanupFailure) {
        this.installedDirectory = installedDirectory;
        this.backupDirectory = backupDirectory;
    }

    /// <summary>
    /// Gets the complete installed tree that remains authoritative after cleanup failed.
    /// </summary>
    public string installedDirectory { get; }

    /// <summary>
    /// Gets the previous tree retained for a later owner-coordinated cleanup attempt.
    /// </summary>
    public string backupDirectory { get; }
}
