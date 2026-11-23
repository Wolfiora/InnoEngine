using System.Numerics;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Carries a measured dialog layout. Only its required extent is public; drawing mechanics remain owned by the widget.
/// </summary>
public readonly struct DialogLayout
{
    internal DialogLayout(
        Vector2 size,
        float textWidth,
        float bodyHeight,
        float bodyContentHeight,
        float spacing,
        float frameHeight,
        float actionWidth
    ) {
        this.size = size;
        this.textWidth = textWidth;
        this.bodyHeight = bodyHeight;
        this.bodyContentHeight = bodyContentHeight;
        this.spacing = spacing;
        this.frameHeight = frameHeight;
        this.actionWidth = actionWidth;
    }

    /// <summary>
    /// Gets the content extent, excluding the window title, border and padding.
    /// </summary>
    public Vector2 size { get; }

    internal float textWidth { get; }
    internal float bodyHeight { get; }
    internal float bodyContentHeight { get; }
    internal float spacing { get; }
    internal float frameHeight { get; }
    internal float actionWidth { get; }
}
