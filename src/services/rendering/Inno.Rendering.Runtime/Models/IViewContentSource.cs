using System;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Contributes model-independent world items to selected views.
/// </summary>
[StableTypeID("638698ee-7dd9-5d75-92aa-84a2457ed3f0")]
public interface IViewContentSource : IDisposable
{
    /// <summary>
    /// Collects items for one exact view.
    /// </summary>
    /// <param name="context">
    /// Frame-scoped host content and view.
    /// </param>
    /// <param name="sink">
    /// Collector receiving items.
    /// </param>
    void Collect(
        ViewContentContext context,
        IViewContentSink sink
    );
}
