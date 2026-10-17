using System;
using System.IO;

namespace Inno.Adapter.Content.FileSystem;

/// <summary>
/// Freezes the host-selected cache location and bounded publication wait for immutable content.
/// </summary>
public sealed class FileContentCacheOptions
{
    /// <summary>
    /// Selects an application-owned absolute cache root without imposing a product prefix.
    /// </summary>
    /// <param name="applicationCacheRoot">
    /// The host-selected root under which this adapter owns only its Content subtree.
    /// </param>
    /// <param name="lockTimeout">
    /// The maximum wait for another preparing process; null selects thirty seconds.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The root is empty or relative, or the timeout is negative.
    /// </exception>
    public FileContentCacheOptions(
        string applicationCacheRoot,
        TimeSpan? lockTimeout = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationCacheRoot);
        if (!Path.IsPathFullyQualified(applicationCacheRoot))
            throw new ArgumentException("Content caches require an absolute host-selected root.", nameof(applicationCacheRoot));
        this.applicationCacheRoot = Path.GetFullPath(applicationCacheRoot);
        this.lockTimeout = lockTimeout ?? TimeSpan.FromSeconds(30);
        if (this.lockTimeout < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(lockTimeout));
    }

    /// <summary>
    /// Gets the host-selected application cache location.
    /// </summary>
    public string applicationCacheRoot { get; }

    /// <summary>
    /// Gets the bounded exclusive preparation wait.
    /// </summary>
    public TimeSpan lockTimeout { get; }
}
