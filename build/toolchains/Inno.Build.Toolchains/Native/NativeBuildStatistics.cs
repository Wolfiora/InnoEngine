namespace Inno.Build.Toolchains;

/// <summary>
/// Reports actual input reading and compiler process work accumulated by one build context.
/// </summary>
public sealed record NativeBuildStatistics
{
    /// <summary>
    /// Gets the number of complete file hashes read, including required stability verification.
    /// </summary>
    public long hashedFiles { get; init; }

    /// <summary>
    /// Gets the actual bytes consumed by input hashing, excluding output integrity validation.
    /// </summary>
    public long hashedBytes { get; init; }

    /// <summary>
    /// Gets native execution processes started through the context's frozen tool selection.
    /// SDK discovery and managed task bootstrapping are recorded by their separate build logs.
    /// </summary>
    public long nativeProcesses { get; init; }
}
