using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Restricts a neutral world-content collector to an output layer's source IDs.
/// </summary>
[StableTypeID("f4217ba1-0650-58be-8e76-d6f2c2c303c5")]
public sealed class SelectedViewContentCollector : IViewContentCollector
{
    private readonly IViewContentCollector m_inner;
    private readonly IReadOnlyList<string> m_sourceIDs;

    /// <summary>
    /// Creates a collector that forwards only the selected source identities.
    /// </summary>
    /// <param name="inner">
    /// Active generation's source collector.
    /// </param>
    /// <param name="sourceIDs">
    /// Source IDs exclusively assigned to this layer.
    /// </param>
    public SelectedViewContentCollector(
        IViewContentCollector inner,
        IReadOnlyList<string> sourceIDs
    ) {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(sourceIDs);
        m_inner = inner;
        m_sourceIDs = sourceIDs;
    }

    /// <summary>
    /// Collects only the assigned world-content sources for one exact view.
    /// </summary>
    /// <param name="context">
    /// The original model view, output input, and frame timing.
    /// </param>
    /// <returns>
    /// Frame-local content emitted by the assigned sources.
    /// </returns>
    public IReadOnlyList<ViewContentItem> Collect(ViewContentContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return m_inner.Collect(
            new ViewContentContext(
                context.content,
                context.sessionID,
                context.view,
                context.frameIndex,
                context.deltaTime,
                context.input,
                context.views,
                m_sourceIDs
            )
        );
    }
}
