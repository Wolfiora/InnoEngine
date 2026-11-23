using System;
using System.Numerics;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Configures an interactive tree row.
/// </summary>
public readonly struct TreeNodeOptions
{
    /// <summary>
    /// Gets explicit indentation for a flattened row, or null for a nested native tree.
    /// Explicit-depth rows never push a native tree scope; callers must not call TreePop for them.
    /// </summary>
    public int? depth { get; init; }

    /// <summary>
    /// Gets whether this row should receive keyboard focus during this submission when its tree window is focused.
    /// </summary>
    public bool requestFocus { get; init; }

    /// <summary>
    /// Gets whether an active edit requires content submission even while its row is clipped.
    /// </summary>
    public bool keepContentActive { get; init; }

    /// <summary>
    /// Gets whether the row is selected.
    /// </summary>
    public bool selected { get; init; }

    /// <summary>
    /// Gets whether the row has no expandable children.
    /// </summary>
    public bool isLeaf { get; init; }

    /// <summary>
    /// Gets whether a custom background is drawn behind an unselected row.
    /// </summary>
    public bool showBackground { get; init; }

    /// <summary>
    /// Gets the custom background color used when <see cref="showBackground"/> is enabled.
    /// </summary>
    public Vector4 backgroundColor { get; init; }

    /// <summary>
    /// Gets whether the row keeps its configured background while hovered.
    /// </summary>
    public bool suppressHoverHighlight { get; init; }

    /// <summary>
    /// Gets whether ancestor and branch guide lines are omitted for this row.
    /// </summary>
    public bool hideGuideLines { get; init; }

    /// <summary>
    /// Gets the callback that draws controls fixed to the current viewport without extending the
    /// tree's horizontal content boundary.
    /// </summary>
    public Action? drawViewportOverlay { get; init; }
}
