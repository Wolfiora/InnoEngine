using System;
using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Contains a detached paste candidate and its newly allocated node identities.
/// </summary>
public sealed class ShaderGraphPasteResult
{
    internal ShaderGraphPasteResult(
        GraphDocument graph,
        GraphNodeID[] nodes
    ) {
        this.document = graph;
        this.insertedNodes = Array.AsReadOnly(nodes);
    }

    /// <summary>
    /// Gets the complete detached candidate; the caller owns its subsequent edits.
    /// </summary>
    public GraphDocument document { get; }

    /// <summary>
    /// Gets new identities suitable for selecting the pasted nodes.
    /// </summary>
    public IReadOnlyList<GraphNodeID> insertedNodes { get; }
}
