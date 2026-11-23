using System;

using NativeStyleVar = Inno.Native.ImGui.ImGuiStyleVar;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported style var choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiStyleVar : int
{
    /// <summary>
    /// Selects the alpha presentation behavior.
    /// </summary>
    Alpha = (int)NativeStyleVar.Alpha,
    /// <summary>
    /// Selects the disabled alpha presentation behavior.
    /// </summary>
    DisabledAlpha = (int)NativeStyleVar.DisabledAlpha,
    /// <summary>
    /// Selects the window padding presentation behavior.
    /// </summary>
    WindowPadding = (int)NativeStyleVar.WindowPadding,
    /// <summary>
    /// Selects the window rounding presentation behavior.
    /// </summary>
    WindowRounding = (int)NativeStyleVar.WindowRounding,
    /// <summary>
    /// Selects the window border size presentation behavior.
    /// </summary>
    WindowBorderSize = (int)NativeStyleVar.WindowBorderSize,
    /// <summary>
    /// Selects the window min size presentation behavior.
    /// </summary>
    WindowMinSize = (int)NativeStyleVar.WindowMinSize,
    /// <summary>
    /// Selects the window title align presentation behavior.
    /// </summary>
    WindowTitleAlign = (int)NativeStyleVar.WindowTitleAlign,
    /// <summary>
    /// Selects the child rounding presentation behavior.
    /// </summary>
    ChildRounding = (int)NativeStyleVar.ChildRounding,
    /// <summary>
    /// Selects the child border size presentation behavior.
    /// </summary>
    ChildBorderSize = (int)NativeStyleVar.ChildBorderSize,
    /// <summary>
    /// Selects the popup rounding presentation behavior.
    /// </summary>
    PopupRounding = (int)NativeStyleVar.PopupRounding,
    /// <summary>
    /// Selects the popup border size presentation behavior.
    /// </summary>
    PopupBorderSize = (int)NativeStyleVar.PopupBorderSize,
    /// <summary>
    /// Selects the frame padding presentation behavior.
    /// </summary>
    FramePadding = (int)NativeStyleVar.FramePadding,
    /// <summary>
    /// Selects the frame rounding presentation behavior.
    /// </summary>
    FrameRounding = (int)NativeStyleVar.FrameRounding,
    /// <summary>
    /// Selects the frame border size presentation behavior.
    /// </summary>
    FrameBorderSize = (int)NativeStyleVar.FrameBorderSize,
    /// <summary>
    /// Selects the item spacing presentation behavior.
    /// </summary>
    ItemSpacing = (int)NativeStyleVar.ItemSpacing,
    /// <summary>
    /// Selects the item inner spacing presentation behavior.
    /// </summary>
    ItemInnerSpacing = (int)NativeStyleVar.ItemInnerSpacing,
    /// <summary>
    /// Selects the indent spacing presentation behavior.
    /// </summary>
    IndentSpacing = (int)NativeStyleVar.IndentSpacing,
    /// <summary>
    /// Selects the cell padding presentation behavior.
    /// </summary>
    CellPadding = (int)NativeStyleVar.CellPadding,
    /// <summary>
    /// Selects the scrollbar size presentation behavior.
    /// </summary>
    ScrollbarSize = (int)NativeStyleVar.ScrollbarSize,
    /// <summary>
    /// Selects the scrollbar rounding presentation behavior.
    /// </summary>
    ScrollbarRounding = (int)NativeStyleVar.ScrollbarRounding,
    /// <summary>
    /// Selects the scrollbar padding presentation behavior.
    /// </summary>
    ScrollbarPadding = (int)NativeStyleVar.ScrollbarPadding,
    /// <summary>
    /// Selects the grab min size presentation behavior.
    /// </summary>
    GrabMinSize = (int)NativeStyleVar.GrabMinSize,
    /// <summary>
    /// Selects the grab rounding presentation behavior.
    /// </summary>
    GrabRounding = (int)NativeStyleVar.GrabRounding,
    /// <summary>
    /// Selects the image rounding presentation behavior.
    /// </summary>
    ImageRounding = (int)NativeStyleVar.ImageRounding,
    /// <summary>
    /// Selects the image border size presentation behavior.
    /// </summary>
    ImageBorderSize = (int)NativeStyleVar.ImageBorderSize,
    /// <summary>
    /// Selects the tab rounding presentation behavior.
    /// </summary>
    TabRounding = (int)NativeStyleVar.TabRounding,
    /// <summary>
    /// Selects the tab border size presentation behavior.
    /// </summary>
    TabBorderSize = (int)NativeStyleVar.TabBorderSize,
    /// <summary>
    /// Selects the tab min width base presentation behavior.
    /// </summary>
    TabMinWidthBase = (int)NativeStyleVar.TabMinWidthBase,
    /// <summary>
    /// Selects the tab min width shrink presentation behavior.
    /// </summary>
    TabMinWidthShrink = (int)NativeStyleVar.TabMinWidthShrink,
    /// <summary>
    /// Selects the tab bar border size presentation behavior.
    /// </summary>
    TabBarBorderSize = (int)NativeStyleVar.TabBarBorderSize,
    /// <summary>
    /// Selects the tab bar overline size presentation behavior.
    /// </summary>
    TabBarOverlineSize = (int)NativeStyleVar.TabBarOverlineSize,
    /// <summary>
    /// Selects the table angled headers angle presentation behavior.
    /// </summary>
    TableAngledHeadersAngle = (int)NativeStyleVar.TableAngledHeadersAngle,
    /// <summary>
    /// Selects the table angled headers text align presentation behavior.
    /// </summary>
    TableAngledHeadersTextAlign = (int)NativeStyleVar.TableAngledHeadersTextAlign,
    /// <summary>
    /// Selects the tree lines size presentation behavior.
    /// </summary>
    TreeLinesSize = (int)NativeStyleVar.TreeLinesSize,
    /// <summary>
    /// Selects the tree lines rounding presentation behavior.
    /// </summary>
    TreeLinesRounding = (int)NativeStyleVar.TreeLinesRounding,
    /// <summary>
    /// Selects the button text align presentation behavior.
    /// </summary>
    ButtonTextAlign = (int)NativeStyleVar.ButtonTextAlign,
    /// <summary>
    /// Selects the selectable text align presentation behavior.
    /// </summary>
    SelectableTextAlign = (int)NativeStyleVar.SelectableTextAlign,
    /// <summary>
    /// Selects the separator text border size presentation behavior.
    /// </summary>
    SeparatorTextBorderSize = (int)NativeStyleVar.SeparatorTextBorderSize,
    /// <summary>
    /// Selects the separator text align presentation behavior.
    /// </summary>
    SeparatorTextAlign = (int)NativeStyleVar.SeparatorTextAlign,
    /// <summary>
    /// Selects the separator text padding presentation behavior.
    /// </summary>
    SeparatorTextPadding = (int)NativeStyleVar.SeparatorTextPadding,
    /// <summary>
    /// Selects the docking separator size presentation behavior.
    /// </summary>
    DockingSeparatorSize = (int)NativeStyleVar.DockingSeparatorSize,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)NativeStyleVar.Count
}
