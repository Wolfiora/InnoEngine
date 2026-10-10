using System.Collections.Generic;

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

    /// <summary>
    /// Gets immutable phase-specific read accounting accumulated by this operation.
    /// </summary>
    public IReadOnlyList<NativeBuildPhaseStatistics> phases { get; init; } = [];

    /// <summary>
    /// Gets complete binding batches requested by this operation.
    /// </summary>
    public long bindingBatches { get; init; }

    /// <summary>
    /// Gets actual bridge or managed generation invocations, excluding valid artifact reuse.
    /// </summary>
    public long bindingGenerations { get; init; }

    /// <summary>
    /// Gets bytes copied to freeze mutable repository inputs.
    /// </summary>
    public long materializedBytes { get; init; }

    /// <summary>
    /// Gets independent artifact output hashes performed for integrity verification and publication.
    /// </summary>
    public long outputFiles { get; init; }

    /// <summary>
    /// Gets bytes read from actual outputs, separately from source snapshot verification.
    /// </summary>
    public long outputBytes { get; init; }

    /// <summary>
    /// Gets managed SDK processes started for generator extension preparation.
    /// </summary>
    public long managedProcesses { get; init; }

}
