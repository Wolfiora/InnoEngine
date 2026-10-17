namespace Inno.Build.Toolchains;

/// <summary>
/// Describes actual full reads and physical read reuse within one explicitly bounded input phase.
/// </summary>
public sealed record NativeBuildPhaseStatistics
{
    /// <summary>
    /// Gets the named operation phase which owns these read results.
    /// </summary>
    public required string phase { get; init; }
    /// <summary>
    /// Gets the number of complete physical file reads.
    /// </summary>
    public long files { get; init; }
    /// <summary>
    /// Gets the number of bytes actually read for hashing.
    /// </summary>
    public long bytes { get; init; }
    /// <summary>
    /// Gets repeated physical hash requests satisfied from this phase's cache.
    /// </summary>
    public long reusedReads { get; init; }
}
