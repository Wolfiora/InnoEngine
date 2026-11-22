using System.Collections.Generic;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Collects all active world-content sources for one exact view.
/// </summary>
[StableTypeID("b8d46505-dda2-52b6-b3c8-133e48641fed")]
public interface IViewContentCollector
{
    /// <summary>
    /// Collects active source items in deterministic source order.
    /// </summary>
    /// <param name="context">
    /// Frame-scoped host content and view.
    /// </param>
    /// <returns>
    /// Frame-local items; the caller owns their sort and composition.
    /// </returns>
    IReadOnlyList<ViewContentItem> Collect(ViewContentContext context);
}
