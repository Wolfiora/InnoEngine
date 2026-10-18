using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Owns one nested diagnostic-name prefix in a render graph builder.
/// </summary>
public sealed class RenderGraphNameScope : IDisposable
{
    private RenderGraphBuilder? m_graph;
    private readonly long m_id;

    internal RenderGraphNameScope(
        RenderGraphBuilder graph,
        long id
    ) {
        m_graph = graph;
        m_id = id;
    }

    /// <summary>
    /// Ends the name prefix scope.
    /// </summary>
    public void Dispose()
    {
        RenderGraphBuilder? graph = m_graph;
        if (graph is null)
            return;
        graph.EndNameScope(m_id);
        m_graph = null;
    }
}

