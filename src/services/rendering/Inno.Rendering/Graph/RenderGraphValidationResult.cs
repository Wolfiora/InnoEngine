using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Describes graph validity and scheduling counts without creating resources or executable callbacks.
/// </summary>
public sealed class RenderGraphValidationResult
{
    internal RenderGraphValidationResult(
        bool isValid,
        IEnumerable<RenderGraphDiagnostic> diagnostics,
        int passCount,
        int scheduledPassCount,
        int textureCount,
        int bufferCount
    ) {
        this.isValid = isValid;
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
        this.passCount = passCount;
        this.scheduledPassCount = scheduledPassCount;
        this.textureCount = textureCount;
        this.bufferCount = bufferCount;
    }

    /// <summary>
    /// Gets whether the current graph satisfies device, dependency and presentation limits.
    /// </summary>
    public bool isValid { get; }

    /// <summary>
    /// Gets frozen deterministic diagnostics for the validated revision.
    /// </summary>
    public IReadOnlyList<RenderGraphDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets the number of declared passes, including passes not contributing to an output.
    /// </summary>
    public int passCount { get; }

    /// <summary>
    /// Gets scheduled passes, or zero when validation could not produce a valid schedule.
    /// </summary>
    public int scheduledPassCount { get; }

    /// <summary>
    /// Gets passes removed by valid output and side-effect analysis; returns zero for an invalid graph.
    /// </summary>
    public int culledPassCount => isValid ? passCount - scheduledPassCount : 0;

    /// <summary>
    /// Gets the number of declared transient and imported textures.
    /// </summary>
    public int textureCount { get; }

    /// <summary>
    /// Gets the number of declared transient and imported buffers.
    /// </summary>
    public int bufferCount { get; }
}
