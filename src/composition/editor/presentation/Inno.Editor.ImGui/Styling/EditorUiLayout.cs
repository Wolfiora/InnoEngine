using System.Numerics;
using Inno.Native.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Copies the presentation metrics needed by custom layouts without retaining native style memory.
/// </summary>
public readonly struct EditorUiLayout
{
    /// <summary>
    /// Gets spacing between neighboring controls.
    /// </summary>
    public Vector2 itemSpacing { get; }
    /// <summary>
    /// Gets the current window inset.
    /// </summary>
    public Vector2 windowPadding { get; }
    /// <summary>
    /// Gets table cell padding.
    /// </summary>
    public Vector2 cellPadding { get; }
    /// <summary>
    /// Gets spacing between parts of a compound control.
    /// </summary>
    public Vector2 itemInnerSpacing { get; }
    /// <summary>
    /// Gets padding inside a control.
    /// </summary>
    public Vector2 framePadding { get; }
    /// <summary>
    /// Gets dock splitter thickness.
    /// </summary>
    public float dockingSeparatorSize { get; }
    /// <summary>
    /// Gets scrollbar width.
    /// </summary>
    public float scrollbarSize { get; }
    /// <summary>
    /// Gets control corner radius.
    /// </summary>
    public float frameRounding { get; }

    internal unsafe EditorUiLayout(ImGuiStyle* style)
    {
        itemSpacing = ImGui.FromNative(style->ItemSpacing);
        windowPadding = ImGui.FromNative(style->WindowPadding);
        cellPadding = ImGui.FromNative(style->CellPadding);
        itemInnerSpacing = ImGui.FromNative(style->ItemInnerSpacing);
        framePadding = ImGui.FromNative(style->FramePadding);
        dockingSeparatorSize = style->DockingSeparatorSize;
        scrollbarSize = style->ScrollbarSize;
        frameRounding = style->FrameRounding;
    }
}
