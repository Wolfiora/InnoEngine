using System;

namespace Inno.Content;

/// <summary>
/// Bounds encoded packs, decoded entries, inventory size, and total expansion before any content is accepted.
/// </summary>
public sealed class ContentReadLimits
{
    /// <summary>
    /// Creates explicit positive budgets for one content preparation operation.
    /// </summary>
    /// <param name="packBytes">
    /// The maximum encoded pack length.
    /// </param>
    /// <param name="entryBytes">
    /// The maximum decoded length of one payload.
    /// </param>
    /// <param name="totalBytes">
    /// The maximum sum of all decoded payload lengths.
    /// </param>
    /// <param name="indexBytes">
    /// The maximum decoded inventory document length.
    /// </param>
    /// <param name="entryCount">
    /// The maximum number of payload entries.
    /// </param>
    public ContentReadLimits(
        long packBytes = 2L * 1024 * 1024 * 1024,
        long entryBytes = 512L * 1024 * 1024,
        long totalBytes = 8L * 1024 * 1024 * 1024,
        int indexBytes = 32 * 1024 * 1024,
        int entryCount = 100_000
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(packBytes);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryBytes);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalBytes);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(indexBytes);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(entryCount);
        this.packBytes = packBytes;
        this.entryBytes = entryBytes;
        this.totalBytes = totalBytes;
        this.indexBytes = indexBytes;
        this.entryCount = entryCount;
    }

    /// <summary>
    /// Gets the encoded pack budget.
    /// </summary>
    public long packBytes { get; }

    /// <summary>
    /// Gets the decoded single-payload budget.
    /// </summary>
    public long entryBytes { get; }

    /// <summary>
    /// Gets the decoded total payload budget.
    /// </summary>
    public long totalBytes { get; }

    /// <summary>
    /// Gets the decoded index budget.
    /// </summary>
    public int indexBytes { get; }

    /// <summary>
    /// Gets the payload count budget.
    /// </summary>
    public int entryCount { get; }
}
