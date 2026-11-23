using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Contains render-graph compilation diagnostics and an optional executable graph.
/// </summary>
public sealed class RenderGraphCompileResult
{
    private readonly IReadOnlyList<RenderGraphDiagnostic> m_diagnostics;

    internal RenderGraphCompileResult(
        CompiledRenderGraph? graph,
        IReadOnlyList<RenderGraphDiagnostic> diagnostics,
        int culledPassCount = 0
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(culledPassCount);
        this.graph = graph;
        m_diagnostics = Array.AsReadOnly(diagnostics.ToArray());
        this.culledPassCount = culledPassCount;
    }

    /// <summary>
    /// Gets the executable graph, or <see langword="null"/> when compilation failed.
    /// </summary>
    public CompiledRenderGraph? graph { get; }

    /// <summary>
    /// Gets deterministic compilation diagnostics.
    /// </summary>
    public IReadOnlyList<RenderGraphDiagnostic> diagnostics => m_diagnostics;

    /// <summary>
    /// Gets passes removed because they did not contribute to an output or side effect.
    /// </summary>
    public int culledPassCount { get; }

    /// <summary>
    /// Gets whether an executable graph was produced.
    /// </summary>
    public bool succeeded => graph is not null;
}
