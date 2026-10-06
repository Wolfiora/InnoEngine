using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Owns isolated graph additions; earlier pass declarations are frozen and failure removes every addition.
/// </summary>
public sealed class RenderGraphMutationScope : IDisposable
{
    private RenderGraphBuilder? m_graph;

    internal RenderGraphMutationScope(
        RenderGraphBuilder graph,
        int textureCount,
        int bufferCount,
        int passCount,
        int outputCount,
        int nameScopeCount,
        RenderGraphValidationState? validation
    ) {
        m_graph = graph;
        this.textureCount = textureCount;
        this.bufferCount = bufferCount;
        this.passCount = passCount;
        this.outputCount = outputCount;
        this.nameScopeCount = nameScopeCount;
        this.validation = validation;
    }

    /// <summary>
    /// Validates and freezes additions in the current scope. Dispose must still end the scope before new work.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The scope is not current, was already committed, or the graph additions are invalid.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// This scope has already ended.
    /// </exception>
    public void Commit()
    {
        ObjectDisposedException.ThrowIf(m_graph is null, this);
        m_graph.CommitMutation(this);
    }

    /// <summary>
    /// Ends the current scope and removes all uncommitted additions, including output and name declarations.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// A nested mutation scope must be ended first.
    /// </exception>
    public void Dispose()
    {
        RenderGraphBuilder? graph = m_graph;
        if (graph is null)
            return;
        graph.EndMutation(this);
        m_graph = null;
    }

    internal int textureCount { get; }
    internal int bufferCount { get; }
    internal int passCount { get; }
    internal int outputCount { get; }
    internal int nameScopeCount { get; }
    internal RenderGraphValidationState? validation { get; }
    internal bool isCommitted { get; set; }
}
