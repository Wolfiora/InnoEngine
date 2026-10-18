using System;

namespace Inno.Build.Toolchains;

/// <summary>
/// Defines bounded retirement pressure without allowing active or current generations to be removed.
/// </summary>
public sealed class BuildCachePolicy
{
    /// <summary>
    /// Creates an immutable cache budget and automatic retirement time limit.
    /// </summary>
    /// <param name="maximumBytes">
    /// The managed cache budget; defaults to 64 GiB.
    /// </param>
    /// <param name="minimumFreeBytes">
    /// The free-space threshold on the checkout volume; defaults to 20 GiB.
    /// </param>
    /// <param name="timeBudget">
    /// The retirement duration, or null for the five-second default.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A budget is negative or the time limit is not positive.
    /// </exception>
    public BuildCachePolicy(
        long maximumBytes = 64L * 1024 * 1024 * 1024,
        long minimumFreeBytes = 20L * 1024 * 1024 * 1024,
        TimeSpan? timeBudget = null
    ) {
        if (maximumBytes < 0)
            throw new ArgumentOutOfRangeException(nameof(maximumBytes));
        if (minimumFreeBytes < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumFreeBytes));
        TimeSpan duration = timeBudget ?? TimeSpan.FromSeconds(5);
        if (duration <= TimeSpan.Zero || duration == System.Threading.Timeout.InfiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(timeBudget));
        this.maximumBytes = maximumBytes;
        this.minimumFreeBytes = minimumFreeBytes;
        this.timeBudget = duration;
    }

    /// <summary>
    /// Gets the maximum size of explicitly registered build caches.
    /// </summary>
    public long maximumBytes { get; }

    /// <summary>
    /// Gets the free-space threshold below which additional retirement is requested.
    /// </summary>
    public long minimumFreeBytes { get; }

    /// <summary>
    /// Gets the duration after which an attempt starts no further retirement work.
    /// An already executing filesystem operation finishes before ownership is released.
    /// </summary>
    public TimeSpan timeBudget { get; }
}
