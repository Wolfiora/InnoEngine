namespace Inno.Assets;

/// <summary>
/// Reports current explicit runtime asset residency accounting.
/// </summary>
public readonly record struct AssetResidencyStatistics
{
    /// <summary>
    /// Creates one immutable residency snapshot.
    /// </summary>
    /// <param name="residentAssetCount">
    /// Number of currently materialized canonical assets.
    /// </param>
    /// <param name="residentBytes">
    /// Runtime payload bytes retained by materialized assets.
    /// </param>
    /// <param name="budgetBytes">
    /// Configured runtime payload budget.
    /// </param>
    public AssetResidencyStatistics(
        int residentAssetCount,
        long residentBytes,
        long budgetBytes
    ) {
        this.residentAssetCount = residentAssetCount;
        this.residentBytes = residentBytes;
        this.budgetBytes = budgetBytes;
    }

    /// <summary>
    /// Gets the number of materialized canonical assets.
    /// </summary>
    public int residentAssetCount { get; }

    /// <summary>
    /// Gets retained runtime payload bytes.
    /// </summary>
    public long residentBytes { get; }

    /// <summary>
    /// Gets the configured runtime payload budget.
    /// </summary>
    public long budgetBytes { get; }
}
