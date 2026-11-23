namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Provides the native geometry of a tree row while its custom content is being drawn.
/// </summary>
public readonly struct TreeNodeDrawContext
{
    internal TreeNodeDrawContext(float rowHeight)
    {
        this.rowHeight = rowHeight;
    }

    /// <summary>
    /// Gets the actual native row height established for the current tree node.
    /// </summary>
    public float rowHeight { get; }
}
