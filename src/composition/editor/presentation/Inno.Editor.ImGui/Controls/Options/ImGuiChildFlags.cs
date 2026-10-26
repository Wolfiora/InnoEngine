using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported child flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiChildFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiChildFlags.None,
    /// <summary>
    /// Show an outer border and enable WindowPadding. (IMPORTANT: this is always == 1 == true for legacy reason)
    /// </summary>
    Borders = (int)Inno.Native.ImGui.ImGuiChildFlags.Borders,
    /// <summary>
    /// Pad with style.WindowPadding even if no border are drawn (no padding by default for non-bordered child windows because it makes more sense)
    /// </summary>
    AlwaysUseWindowPadding = (int)Inno.Native.ImGui.ImGuiChildFlags.AlwaysUseWindowPadding,
    /// <summary>
    /// Allow resize from right border (layout direction). Enable .ini saving (unless ImGuiWindowFlags_NoSavedSettings passed to window flags)
    /// </summary>
    ResizeX = (int)Inno.Native.ImGui.ImGuiChildFlags.ResizeX,
    /// <summary>
    /// Allow resize from bottom border (layout direction). "
    /// </summary>
    ResizeY = (int)Inno.Native.ImGui.ImGuiChildFlags.ResizeY,
    /// <summary>
    /// Enable auto-resizing width. Read "IMPORTANT: Size measurement" details above.
    /// </summary>
    AutoResizeX = (int)Inno.Native.ImGui.ImGuiChildFlags.AutoResizeX,
    /// <summary>
    /// Enable auto-resizing height. Read "IMPORTANT: Size measurement" details above.
    /// </summary>
    AutoResizeY = (int)Inno.Native.ImGui.ImGuiChildFlags.AutoResizeY,
    /// <summary>
    /// Combined with AutoResizeX/AutoResizeY. Always measure size even when child is hidden, always return true, always disable clipping optimization! NOT RECOMMENDED.
    /// </summary>
    AlwaysAutoResize = (int)Inno.Native.ImGui.ImGuiChildFlags.AlwaysAutoResize,
    /// <summary>
    /// Style the child window like a framed item: use FrameBg, FrameRounding, FrameBorderSize, FramePadding instead of ChildBg, ChildRounding, ChildBorderSize, WindowPadding.
    /// </summary>
    FrameStyle = (int)Inno.Native.ImGui.ImGuiChildFlags.FrameStyle,
    /// <summary>
    /// Share focus scope, allow keyboard/gamepad navigation to cross over parent border to this child or between sibling child windows.
    /// </summary>
    NavFlattened = (int)Inno.Native.ImGui.ImGuiChildFlags.NavFlattened,
}
