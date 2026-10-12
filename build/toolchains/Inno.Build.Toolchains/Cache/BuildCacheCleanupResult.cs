using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Reports completed retirement independently from protected, active and failed artifacts.
/// </summary>
public sealed class BuildCacheCleanupResult
{
    internal BuildCacheCleanupResult(
        IEnumerable<string> retired,
        IEnumerable<string> skipped,
        IReadOnlyDictionary<string, string> failures,
        long retiredBytes,
        bool timeLimitReached
    ) {
        this.retired = Array.AsReadOnly(retired.ToArray());
        this.skipped = Array.AsReadOnly(skipped.ToArray());
        this.failures = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(failures));
        this.retiredBytes = retiredBytes;
        this.timeLimitReached = timeLimitReached;
    }

    /// <summary>
    /// Gets directories actually retired; a preview always returns an empty collection.
    /// </summary>
    public IReadOnlyList<string> retired { get; }

    /// <summary>
    /// Gets candidates preserved because ownership, protection or dependencies changed.
    /// </summary>
    public IReadOnlyList<string> skipped { get; }

    /// <summary>
    /// Gets failed paths and their observable IO or permission diagnostics.
    /// </summary>
    public IReadOnlyDictionary<string, string> failures { get; }

    /// <summary>
    /// Gets the declared file bytes of successfully retired artifacts, excluding persistent leases.
    /// This is a logical size; hardlinks, compression and other writers can affect actual volume recovery.
    /// </summary>
    public long retiredBytes { get; }

    /// <summary>
    /// Gets whether the bounded attempt ended before processing all eligible candidates.
    /// </summary>
    public bool timeLimitReached { get; }
}
