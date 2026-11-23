using System;

using NativeCol = Inno.Native.ImGui.ImGuiCol;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported col choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiCol : int
{
    /// <summary>
    /// Selects the text presentation behavior.
    /// </summary>
    Text = (int)NativeCol.Text,
    /// <summary>
    /// Selects the text disabled presentation behavior.
    /// </summary>
    TextDisabled = (int)NativeCol.TextDisabled,
    /// <summary>
    /// Selects the window bg presentation behavior.
    /// </summary>
    WindowBg = (int)NativeCol.WindowBg,
    /// <summary>
    /// Selects the child bg presentation behavior.
    /// </summary>
    ChildBg = (int)NativeCol.ChildBg,
    /// <summary>
    /// Selects the popup bg presentation behavior.
    /// </summary>
    PopupBg = (int)NativeCol.PopupBg,
    /// <summary>
    /// Selects the border presentation behavior.
    /// </summary>
    Border = (int)NativeCol.Border,
    /// <summary>
    /// Selects the border shadow presentation behavior.
    /// </summary>
    BorderShadow = (int)NativeCol.BorderShadow,
    /// <summary>
    /// Selects the frame bg presentation behavior.
    /// </summary>
    FrameBg = (int)NativeCol.FrameBg,
    /// <summary>
    /// Selects the frame bg hovered presentation behavior.
    /// </summary>
    FrameBgHovered = (int)NativeCol.FrameBgHovered,
    /// <summary>
    /// Selects the frame bg active presentation behavior.
    /// </summary>
    FrameBgActive = (int)NativeCol.FrameBgActive,
    /// <summary>
    /// Selects the title bg presentation behavior.
    /// </summary>
    TitleBg = (int)NativeCol.TitleBg,
    /// <summary>
    /// Selects the title bg active presentation behavior.
    /// </summary>
    TitleBgActive = (int)NativeCol.TitleBgActive,
    /// <summary>
    /// Selects the title bg collapsed presentation behavior.
    /// </summary>
    TitleBgCollapsed = (int)NativeCol.TitleBgCollapsed,
    /// <summary>
    /// Selects the menu bar bg presentation behavior.
    /// </summary>
    MenuBarBg = (int)NativeCol.MenuBarBg,
    /// <summary>
    /// Selects the scrollbar bg presentation behavior.
    /// </summary>
    ScrollbarBg = (int)NativeCol.ScrollbarBg,
    /// <summary>
    /// Selects the scrollbar grab presentation behavior.
    /// </summary>
    ScrollbarGrab = (int)NativeCol.ScrollbarGrab,
    /// <summary>
    /// Selects the scrollbar grab hovered presentation behavior.
    /// </summary>
    ScrollbarGrabHovered = (int)NativeCol.ScrollbarGrabHovered,
    /// <summary>
    /// Selects the scrollbar grab active presentation behavior.
    /// </summary>
    ScrollbarGrabActive = (int)NativeCol.ScrollbarGrabActive,
    /// <summary>
    /// Selects the check mark presentation behavior.
    /// </summary>
    CheckMark = (int)NativeCol.CheckMark,
    /// <summary>
    /// Selects the slider grab presentation behavior.
    /// </summary>
    SliderGrab = (int)NativeCol.SliderGrab,
    /// <summary>
    /// Selects the slider grab active presentation behavior.
    /// </summary>
    SliderGrabActive = (int)NativeCol.SliderGrabActive,
    /// <summary>
    /// Selects the button presentation behavior.
    /// </summary>
    Button = (int)NativeCol.Button,
    /// <summary>
    /// Selects the button hovered presentation behavior.
    /// </summary>
    ButtonHovered = (int)NativeCol.ButtonHovered,
    /// <summary>
    /// Selects the button active presentation behavior.
    /// </summary>
    ButtonActive = (int)NativeCol.ButtonActive,
    /// <summary>
    /// Selects the header presentation behavior.
    /// </summary>
    Header = (int)NativeCol.Header,
    /// <summary>
    /// Selects the header hovered presentation behavior.
    /// </summary>
    HeaderHovered = (int)NativeCol.HeaderHovered,
    /// <summary>
    /// Selects the header active presentation behavior.
    /// </summary>
    HeaderActive = (int)NativeCol.HeaderActive,
    /// <summary>
    /// Selects the separator presentation behavior.
    /// </summary>
    Separator = (int)NativeCol.Separator,
    /// <summary>
    /// Selects the separator hovered presentation behavior.
    /// </summary>
    SeparatorHovered = (int)NativeCol.SeparatorHovered,
    /// <summary>
    /// Selects the separator active presentation behavior.
    /// </summary>
    SeparatorActive = (int)NativeCol.SeparatorActive,
    /// <summary>
    /// Selects the resize grip presentation behavior.
    /// </summary>
    ResizeGrip = (int)NativeCol.ResizeGrip,
    /// <summary>
    /// Selects the resize grip hovered presentation behavior.
    /// </summary>
    ResizeGripHovered = (int)NativeCol.ResizeGripHovered,
    /// <summary>
    /// Selects the resize grip active presentation behavior.
    /// </summary>
    ResizeGripActive = (int)NativeCol.ResizeGripActive,
    /// <summary>
    /// Selects the input text cursor presentation behavior.
    /// </summary>
    InputTextCursor = (int)NativeCol.InputTextCursor,
    /// <summary>
    /// Selects the tab hovered presentation behavior.
    /// </summary>
    TabHovered = (int)NativeCol.TabHovered,
    /// <summary>
    /// Selects the tab presentation behavior.
    /// </summary>
    Tab = (int)NativeCol.Tab,
    /// <summary>
    /// Selects the tab selected presentation behavior.
    /// </summary>
    TabSelected = (int)NativeCol.TabSelected,
    /// <summary>
    /// Selects the tab selected overline presentation behavior.
    /// </summary>
    TabSelectedOverline = (int)NativeCol.TabSelectedOverline,
    /// <summary>
    /// Selects the tab dimmed presentation behavior.
    /// </summary>
    TabDimmed = (int)NativeCol.TabDimmed,
    /// <summary>
    /// Selects the tab dimmed selected presentation behavior.
    /// </summary>
    TabDimmedSelected = (int)NativeCol.TabDimmedSelected,
    /// <summary>
    /// Selects the tab dimmed selected overline presentation behavior.
    /// </summary>
    TabDimmedSelectedOverline = (int)NativeCol.TabDimmedSelectedOverline,
    /// <summary>
    /// Selects the docking preview presentation behavior.
    /// </summary>
    DockingPreview = (int)NativeCol.DockingPreview,
    /// <summary>
    /// Selects the docking empty bg presentation behavior.
    /// </summary>
    DockingEmptyBg = (int)NativeCol.DockingEmptyBg,
    /// <summary>
    /// Selects the plot lines presentation behavior.
    /// </summary>
    PlotLines = (int)NativeCol.PlotLines,
    /// <summary>
    /// Selects the plot lines hovered presentation behavior.
    /// </summary>
    PlotLinesHovered = (int)NativeCol.PlotLinesHovered,
    /// <summary>
    /// Selects the plot histogram presentation behavior.
    /// </summary>
    PlotHistogram = (int)NativeCol.PlotHistogram,
    /// <summary>
    /// Selects the plot histogram hovered presentation behavior.
    /// </summary>
    PlotHistogramHovered = (int)NativeCol.PlotHistogramHovered,
    /// <summary>
    /// Selects the table header bg presentation behavior.
    /// </summary>
    TableHeaderBg = (int)NativeCol.TableHeaderBg,
    /// <summary>
    /// Selects the table border strong presentation behavior.
    /// </summary>
    TableBorderStrong = (int)NativeCol.TableBorderStrong,
    /// <summary>
    /// Selects the table border light presentation behavior.
    /// </summary>
    TableBorderLight = (int)NativeCol.TableBorderLight,
    /// <summary>
    /// Selects the table row bg presentation behavior.
    /// </summary>
    TableRowBg = (int)NativeCol.TableRowBg,
    /// <summary>
    /// Selects the table row bg alt presentation behavior.
    /// </summary>
    TableRowBgAlt = (int)NativeCol.TableRowBgAlt,
    /// <summary>
    /// Selects the text link presentation behavior.
    /// </summary>
    TextLink = (int)NativeCol.TextLink,
    /// <summary>
    /// Selects the text selected bg presentation behavior.
    /// </summary>
    TextSelectedBg = (int)NativeCol.TextSelectedBg,
    /// <summary>
    /// Selects the tree lines presentation behavior.
    /// </summary>
    TreeLines = (int)NativeCol.TreeLines,
    /// <summary>
    /// Selects the drag drop target presentation behavior.
    /// </summary>
    DragDropTarget = (int)NativeCol.DragDropTarget,
    /// <summary>
    /// Selects the drag drop target bg presentation behavior.
    /// </summary>
    DragDropTargetBg = (int)NativeCol.DragDropTargetBg,
    /// <summary>
    /// Selects the unsaved marker presentation behavior.
    /// </summary>
    UnsavedMarker = (int)NativeCol.UnsavedMarker,
    /// <summary>
    /// Selects the nav cursor presentation behavior.
    /// </summary>
    NavCursor = (int)NativeCol.NavCursor,
    /// <summary>
    /// Selects the nav windowing highlight presentation behavior.
    /// </summary>
    NavWindowingHighlight = (int)NativeCol.NavWindowingHighlight,
    /// <summary>
    /// Selects the nav windowing dim bg presentation behavior.
    /// </summary>
    NavWindowingDimBg = (int)NativeCol.NavWindowingDimBg,
    /// <summary>
    /// Selects the modal window dim bg presentation behavior.
    /// </summary>
    ModalWindowDimBg = (int)NativeCol.ModalWindowDimBg,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)NativeCol.Count
}
