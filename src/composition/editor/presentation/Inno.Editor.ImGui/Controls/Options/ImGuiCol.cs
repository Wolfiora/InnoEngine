using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported col choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiCol : int
{
    /// <summary>
    /// Selects the text presentation behavior.
    /// </summary>
    Text = (int)Inno.Native.ImGui.ImGuiCol.Text,
    /// <summary>
    /// Selects the text disabled presentation behavior.
    /// </summary>
    TextDisabled = (int)Inno.Native.ImGui.ImGuiCol.TextDisabled,
    /// <summary>
    /// Selects the window bg presentation behavior.
    /// </summary>
    WindowBg = (int)Inno.Native.ImGui.ImGuiCol.WindowBg,
    /// <summary>
    /// Selects the child bg presentation behavior.
    /// </summary>
    ChildBg = (int)Inno.Native.ImGui.ImGuiCol.ChildBg,
    /// <summary>
    /// Selects the popup bg presentation behavior.
    /// </summary>
    PopupBg = (int)Inno.Native.ImGui.ImGuiCol.PopupBg,
    /// <summary>
    /// Selects the border presentation behavior.
    /// </summary>
    Border = (int)Inno.Native.ImGui.ImGuiCol.Border,
    /// <summary>
    /// Selects the border shadow presentation behavior.
    /// </summary>
    BorderShadow = (int)Inno.Native.ImGui.ImGuiCol.BorderShadow,
    /// <summary>
    /// Selects the frame bg presentation behavior.
    /// </summary>
    FrameBg = (int)Inno.Native.ImGui.ImGuiCol.FrameBg,
    /// <summary>
    /// Selects the frame bg hovered presentation behavior.
    /// </summary>
    FrameBgHovered = (int)Inno.Native.ImGui.ImGuiCol.FrameBgHovered,
    /// <summary>
    /// Selects the frame bg active presentation behavior.
    /// </summary>
    FrameBgActive = (int)Inno.Native.ImGui.ImGuiCol.FrameBgActive,
    /// <summary>
    /// Selects the title bg presentation behavior.
    /// </summary>
    TitleBg = (int)Inno.Native.ImGui.ImGuiCol.TitleBg,
    /// <summary>
    /// Selects the title bg active presentation behavior.
    /// </summary>
    TitleBgActive = (int)Inno.Native.ImGui.ImGuiCol.TitleBgActive,
    /// <summary>
    /// Selects the title bg collapsed presentation behavior.
    /// </summary>
    TitleBgCollapsed = (int)Inno.Native.ImGui.ImGuiCol.TitleBgCollapsed,
    /// <summary>
    /// Selects the menu bar bg presentation behavior.
    /// </summary>
    MenuBarBg = (int)Inno.Native.ImGui.ImGuiCol.MenuBarBg,
    /// <summary>
    /// Selects the scrollbar bg presentation behavior.
    /// </summary>
    ScrollbarBg = (int)Inno.Native.ImGui.ImGuiCol.ScrollbarBg,
    /// <summary>
    /// Selects the scrollbar grab presentation behavior.
    /// </summary>
    ScrollbarGrab = (int)Inno.Native.ImGui.ImGuiCol.ScrollbarGrab,
    /// <summary>
    /// Selects the scrollbar grab hovered presentation behavior.
    /// </summary>
    ScrollbarGrabHovered = (int)Inno.Native.ImGui.ImGuiCol.ScrollbarGrabHovered,
    /// <summary>
    /// Selects the scrollbar grab active presentation behavior.
    /// </summary>
    ScrollbarGrabActive = (int)Inno.Native.ImGui.ImGuiCol.ScrollbarGrabActive,
    /// <summary>
    /// Selects the check mark presentation behavior.
    /// </summary>
    CheckMark = (int)Inno.Native.ImGui.ImGuiCol.CheckMark,
    /// <summary>
    /// Selects the slider grab presentation behavior.
    /// </summary>
    SliderGrab = (int)Inno.Native.ImGui.ImGuiCol.SliderGrab,
    /// <summary>
    /// Selects the slider grab active presentation behavior.
    /// </summary>
    SliderGrabActive = (int)Inno.Native.ImGui.ImGuiCol.SliderGrabActive,
    /// <summary>
    /// Selects the button presentation behavior.
    /// </summary>
    Button = (int)Inno.Native.ImGui.ImGuiCol.Button,
    /// <summary>
    /// Selects the button hovered presentation behavior.
    /// </summary>
    ButtonHovered = (int)Inno.Native.ImGui.ImGuiCol.ButtonHovered,
    /// <summary>
    /// Selects the button active presentation behavior.
    /// </summary>
    ButtonActive = (int)Inno.Native.ImGui.ImGuiCol.ButtonActive,
    /// <summary>
    /// Selects the header presentation behavior.
    /// </summary>
    Header = (int)Inno.Native.ImGui.ImGuiCol.Header,
    /// <summary>
    /// Selects the header hovered presentation behavior.
    /// </summary>
    HeaderHovered = (int)Inno.Native.ImGui.ImGuiCol.HeaderHovered,
    /// <summary>
    /// Selects the header active presentation behavior.
    /// </summary>
    HeaderActive = (int)Inno.Native.ImGui.ImGuiCol.HeaderActive,
    /// <summary>
    /// Selects the separator presentation behavior.
    /// </summary>
    Separator = (int)Inno.Native.ImGui.ImGuiCol.Separator,
    /// <summary>
    /// Selects the separator hovered presentation behavior.
    /// </summary>
    SeparatorHovered = (int)Inno.Native.ImGui.ImGuiCol.SeparatorHovered,
    /// <summary>
    /// Selects the separator active presentation behavior.
    /// </summary>
    SeparatorActive = (int)Inno.Native.ImGui.ImGuiCol.SeparatorActive,
    /// <summary>
    /// Selects the resize grip presentation behavior.
    /// </summary>
    ResizeGrip = (int)Inno.Native.ImGui.ImGuiCol.ResizeGrip,
    /// <summary>
    /// Selects the resize grip hovered presentation behavior.
    /// </summary>
    ResizeGripHovered = (int)Inno.Native.ImGui.ImGuiCol.ResizeGripHovered,
    /// <summary>
    /// Selects the resize grip active presentation behavior.
    /// </summary>
    ResizeGripActive = (int)Inno.Native.ImGui.ImGuiCol.ResizeGripActive,
    /// <summary>
    /// Selects the input text cursor presentation behavior.
    /// </summary>
    InputTextCursor = (int)Inno.Native.ImGui.ImGuiCol.InputTextCursor,
    /// <summary>
    /// Selects the tab hovered presentation behavior.
    /// </summary>
    TabHovered = (int)Inno.Native.ImGui.ImGuiCol.TabHovered,
    /// <summary>
    /// Selects the tab presentation behavior.
    /// </summary>
    Tab = (int)Inno.Native.ImGui.ImGuiCol.Tab,
    /// <summary>
    /// Selects the tab selected presentation behavior.
    /// </summary>
    TabSelected = (int)Inno.Native.ImGui.ImGuiCol.TabSelected,
    /// <summary>
    /// Selects the tab selected overline presentation behavior.
    /// </summary>
    TabSelectedOverline = (int)Inno.Native.ImGui.ImGuiCol.TabSelectedOverline,
    /// <summary>
    /// Selects the tab dimmed presentation behavior.
    /// </summary>
    TabDimmed = (int)Inno.Native.ImGui.ImGuiCol.TabDimmed,
    /// <summary>
    /// Selects the tab dimmed selected presentation behavior.
    /// </summary>
    TabDimmedSelected = (int)Inno.Native.ImGui.ImGuiCol.TabDimmedSelected,
    /// <summary>
    /// Selects the tab dimmed selected overline presentation behavior.
    /// </summary>
    TabDimmedSelectedOverline = (int)Inno.Native.ImGui.ImGuiCol.TabDimmedSelectedOverline,
    /// <summary>
    /// Selects the docking preview presentation behavior.
    /// </summary>
    DockingPreview = (int)Inno.Native.ImGui.ImGuiCol.DockingPreview,
    /// <summary>
    /// Selects the docking empty bg presentation behavior.
    /// </summary>
    DockingEmptyBg = (int)Inno.Native.ImGui.ImGuiCol.DockingEmptyBg,
    /// <summary>
    /// Selects the plot lines presentation behavior.
    /// </summary>
    PlotLines = (int)Inno.Native.ImGui.ImGuiCol.PlotLines,
    /// <summary>
    /// Selects the plot lines hovered presentation behavior.
    /// </summary>
    PlotLinesHovered = (int)Inno.Native.ImGui.ImGuiCol.PlotLinesHovered,
    /// <summary>
    /// Selects the plot histogram presentation behavior.
    /// </summary>
    PlotHistogram = (int)Inno.Native.ImGui.ImGuiCol.PlotHistogram,
    /// <summary>
    /// Selects the plot histogram hovered presentation behavior.
    /// </summary>
    PlotHistogramHovered = (int)Inno.Native.ImGui.ImGuiCol.PlotHistogramHovered,
    /// <summary>
    /// Selects the table header bg presentation behavior.
    /// </summary>
    TableHeaderBg = (int)Inno.Native.ImGui.ImGuiCol.TableHeaderBg,
    /// <summary>
    /// Selects the table border strong presentation behavior.
    /// </summary>
    TableBorderStrong = (int)Inno.Native.ImGui.ImGuiCol.TableBorderStrong,
    /// <summary>
    /// Selects the table border light presentation behavior.
    /// </summary>
    TableBorderLight = (int)Inno.Native.ImGui.ImGuiCol.TableBorderLight,
    /// <summary>
    /// Selects the table row bg presentation behavior.
    /// </summary>
    TableRowBg = (int)Inno.Native.ImGui.ImGuiCol.TableRowBg,
    /// <summary>
    /// Selects the table row bg alt presentation behavior.
    /// </summary>
    TableRowBgAlt = (int)Inno.Native.ImGui.ImGuiCol.TableRowBgAlt,
    /// <summary>
    /// Selects the text link presentation behavior.
    /// </summary>
    TextLink = (int)Inno.Native.ImGui.ImGuiCol.TextLink,
    /// <summary>
    /// Selects the text selected bg presentation behavior.
    /// </summary>
    TextSelectedBg = (int)Inno.Native.ImGui.ImGuiCol.TextSelectedBg,
    /// <summary>
    /// Selects the tree lines presentation behavior.
    /// </summary>
    TreeLines = (int)Inno.Native.ImGui.ImGuiCol.TreeLines,
    /// <summary>
    /// Selects the drag drop target presentation behavior.
    /// </summary>
    DragDropTarget = (int)Inno.Native.ImGui.ImGuiCol.DragDropTarget,
    /// <summary>
    /// Selects the drag drop target bg presentation behavior.
    /// </summary>
    DragDropTargetBg = (int)Inno.Native.ImGui.ImGuiCol.DragDropTargetBg,
    /// <summary>
    /// Selects the unsaved marker presentation behavior.
    /// </summary>
    UnsavedMarker = (int)Inno.Native.ImGui.ImGuiCol.UnsavedMarker,
    /// <summary>
    /// Selects the nav cursor presentation behavior.
    /// </summary>
    NavCursor = (int)Inno.Native.ImGui.ImGuiCol.NavCursor,
    /// <summary>
    /// Selects the nav windowing highlight presentation behavior.
    /// </summary>
    NavWindowingHighlight = (int)Inno.Native.ImGui.ImGuiCol.NavWindowingHighlight,
    /// <summary>
    /// Selects the nav windowing dim bg presentation behavior.
    /// </summary>
    NavWindowingDimBg = (int)Inno.Native.ImGui.ImGuiCol.NavWindowingDimBg,
    /// <summary>
    /// Selects the modal window dim bg presentation behavior.
    /// </summary>
    ModalWindowDimBg = (int)Inno.Native.ImGui.ImGuiCol.ModalWindowDimBg,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)Inno.Native.ImGui.ImGuiCol.Count,
}
