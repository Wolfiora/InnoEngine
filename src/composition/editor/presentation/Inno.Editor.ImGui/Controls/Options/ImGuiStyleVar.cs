using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported style var choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiStyleVar : int
{
    /// <summary>
    /// Selects the alpha presentation behavior.
    /// </summary>
    Alpha = (int)Inno.Native.ImGui.ImGuiStyleVar.Alpha,
    /// <summary>
    /// Selects the disabled alpha presentation behavior.
    /// </summary>
    DisabledAlpha = (int)Inno.Native.ImGui.ImGuiStyleVar.DisabledAlpha,
    /// <summary>
    /// Selects the window padding presentation behavior.
    /// </summary>
    WindowPadding = (int)Inno.Native.ImGui.ImGuiStyleVar.WindowPadding,
    /// <summary>
    /// Selects the window rounding presentation behavior.
    /// </summary>
    WindowRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.WindowRounding,
    /// <summary>
    /// Selects the window border size presentation behavior.
    /// </summary>
    WindowBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.WindowBorderSize,
    /// <summary>
    /// Selects the window min size presentation behavior.
    /// </summary>
    WindowMinSize = (int)Inno.Native.ImGui.ImGuiStyleVar.WindowMinSize,
    /// <summary>
    /// Selects the window title align presentation behavior.
    /// </summary>
    WindowTitleAlign = (int)Inno.Native.ImGui.ImGuiStyleVar.WindowTitleAlign,
    /// <summary>
    /// Selects the child rounding presentation behavior.
    /// </summary>
    ChildRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.ChildRounding,
    /// <summary>
    /// Selects the child border size presentation behavior.
    /// </summary>
    ChildBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.ChildBorderSize,
    /// <summary>
    /// Selects the popup rounding presentation behavior.
    /// </summary>
    PopupRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.PopupRounding,
    /// <summary>
    /// Selects the popup border size presentation behavior.
    /// </summary>
    PopupBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.PopupBorderSize,
    /// <summary>
    /// Selects the frame padding presentation behavior.
    /// </summary>
    FramePadding = (int)Inno.Native.ImGui.ImGuiStyleVar.FramePadding,
    /// <summary>
    /// Selects the frame rounding presentation behavior.
    /// </summary>
    FrameRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.FrameRounding,
    /// <summary>
    /// Selects the frame border size presentation behavior.
    /// </summary>
    FrameBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.FrameBorderSize,
    /// <summary>
    /// Selects the item spacing presentation behavior.
    /// </summary>
    ItemSpacing = (int)Inno.Native.ImGui.ImGuiStyleVar.ItemSpacing,
    /// <summary>
    /// Selects the item inner spacing presentation behavior.
    /// </summary>
    ItemInnerSpacing = (int)Inno.Native.ImGui.ImGuiStyleVar.ItemInnerSpacing,
    /// <summary>
    /// Selects the indent spacing presentation behavior.
    /// </summary>
    IndentSpacing = (int)Inno.Native.ImGui.ImGuiStyleVar.IndentSpacing,
    /// <summary>
    /// Selects the cell padding presentation behavior.
    /// </summary>
    CellPadding = (int)Inno.Native.ImGui.ImGuiStyleVar.CellPadding,
    /// <summary>
    /// Selects the scrollbar size presentation behavior.
    /// </summary>
    ScrollbarSize = (int)Inno.Native.ImGui.ImGuiStyleVar.ScrollbarSize,
    /// <summary>
    /// Selects the scrollbar rounding presentation behavior.
    /// </summary>
    ScrollbarRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.ScrollbarRounding,
    /// <summary>
    /// Selects the scrollbar padding presentation behavior.
    /// </summary>
    ScrollbarPadding = (int)Inno.Native.ImGui.ImGuiStyleVar.ScrollbarPadding,
    /// <summary>
    /// Selects the grab min size presentation behavior.
    /// </summary>
    GrabMinSize = (int)Inno.Native.ImGui.ImGuiStyleVar.GrabMinSize,
    /// <summary>
    /// Selects the grab rounding presentation behavior.
    /// </summary>
    GrabRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.GrabRounding,
    /// <summary>
    /// Selects the image rounding presentation behavior.
    /// </summary>
    ImageRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.ImageRounding,
    /// <summary>
    /// Selects the image border size presentation behavior.
    /// </summary>
    ImageBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.ImageBorderSize,
    /// <summary>
    /// Selects the tab rounding presentation behavior.
    /// </summary>
    TabRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.TabRounding,
    /// <summary>
    /// Selects the tab border size presentation behavior.
    /// </summary>
    TabBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.TabBorderSize,
    /// <summary>
    /// Selects the tab min width base presentation behavior.
    /// </summary>
    TabMinWidthBase = (int)Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthBase,
    /// <summary>
    /// Selects the tab min width shrink presentation behavior.
    /// </summary>
    TabMinWidthShrink = (int)Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthShrink,
    /// <summary>
    /// Selects the tab bar border size presentation behavior.
    /// </summary>
    TabBarBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.TabBarBorderSize,
    /// <summary>
    /// Selects the tab bar overline size presentation behavior.
    /// </summary>
    TabBarOverlineSize = (int)Inno.Native.ImGui.ImGuiStyleVar.TabBarOverlineSize,
    /// <summary>
    /// Selects the table angled headers angle presentation behavior.
    /// </summary>
    TableAngledHeadersAngle = (int)Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersAngle,
    /// <summary>
    /// Selects the table angled headers text align presentation behavior.
    /// </summary>
    TableAngledHeadersTextAlign = (int)Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersTextAlign,
    /// <summary>
    /// Selects the tree lines size presentation behavior.
    /// </summary>
    TreeLinesSize = (int)Inno.Native.ImGui.ImGuiStyleVar.TreeLinesSize,
    /// <summary>
    /// Selects the tree lines rounding presentation behavior.
    /// </summary>
    TreeLinesRounding = (int)Inno.Native.ImGui.ImGuiStyleVar.TreeLinesRounding,
    /// <summary>
    /// Selects the button text align presentation behavior.
    /// </summary>
    ButtonTextAlign = (int)Inno.Native.ImGui.ImGuiStyleVar.ButtonTextAlign,
    /// <summary>
    /// Selects the selectable text align presentation behavior.
    /// </summary>
    SelectableTextAlign = (int)Inno.Native.ImGui.ImGuiStyleVar.SelectableTextAlign,
    /// <summary>
    /// Selects the separator text border size presentation behavior.
    /// </summary>
    SeparatorTextBorderSize = (int)Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextBorderSize,
    /// <summary>
    /// Selects the separator text align presentation behavior.
    /// </summary>
    SeparatorTextAlign = (int)Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextAlign,
    /// <summary>
    /// Selects the separator text padding presentation behavior.
    /// </summary>
    SeparatorTextPadding = (int)Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextPadding,
    /// <summary>
    /// Selects the docking separator size presentation behavior.
    /// </summary>
    DockingSeparatorSize = (int)Inno.Native.ImGui.ImGuiStyleVar.DockingSeparatorSize,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)Inno.Native.ImGui.ImGuiStyleVar.Count,
}
