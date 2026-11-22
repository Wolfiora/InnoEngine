using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Receives world items without imposing a rendering-model sort key.
/// </summary>
[StableTypeID("ee4aa3b7-c773-5f05-9655-5a3cfd457c26")]
public interface IViewContentSink
{
    /// <summary>
    /// Adds an item to the current view.
    /// </summary>
    /// <param name="item">
    /// Frame-local content item.
    /// </param>
    void Submit(ViewContentItem item);
}
