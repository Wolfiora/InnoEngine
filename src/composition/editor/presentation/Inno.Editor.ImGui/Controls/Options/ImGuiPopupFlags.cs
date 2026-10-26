using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported popup flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiPopupFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiPopupFlags.None,
    /// <summary>
    /// For BeginPopupContext*(): open on Left Mouse release. Only one button allowed!
    /// </summary>
    MouseButtonLeft = (int)Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonLeft,
    /// <summary>
    /// For BeginPopupContext*(): open on Right Mouse release. Only one button allowed! (default)
    /// </summary>
    MouseButtonRight = (int)Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonRight,
    /// <summary>
    /// For BeginPopupContext*(): open on Middle Mouse release. Only one button allowed!
    /// </summary>
    MouseButtonMiddle = (int)Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMiddle,
    /// <summary>
    /// For OpenPopup*(), BeginPopupContext*(): don't reopen same popup if already open (won't reposition, won't reinitialize navigation)
    /// </summary>
    NoReopen = (int)Inno.Native.ImGui.ImGuiPopupFlags.NoReopen,
    /// <summary>
    /// For OpenPopup*(), BeginPopupContext*(): don't open if there's already a popup at the same level of the popup stack
    /// </summary>
    NoOpenOverExistingPopup = (int)Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverExistingPopup,
    /// <summary>
    /// For BeginPopupContextWindow(): don't return true when hovering items, only when hovering empty space
    /// </summary>
    NoOpenOverItems = (int)Inno.Native.ImGui.ImGuiPopupFlags.NoOpenOverItems,
    /// <summary>
    /// For IsPopupOpen(): ignore the ImGuiID parameter and test for any popup.
    /// </summary>
    AnyPopupId = (int)Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupId,
    /// <summary>
    /// For IsPopupOpen(): search/test at any level of the popup stack (default test in the current level)
    /// </summary>
    AnyPopupLevel = (int)Inno.Native.ImGui.ImGuiPopupFlags.AnyPopupLevel,
    /// <summary>
    /// Selects the any popup presentation behavior.
    /// </summary>
    AnyPopup = (int)Inno.Native.ImGui.ImGuiPopupFlags.AnyPopup,
    /// <summary>
    /// Selects the mouse button shift presentation behavior.
    /// </summary>
    MouseButtonShift = (int)Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonShift,
    /// <summary>
    /// Selects the mouse button mask presentation behavior.
    /// </summary>
    MouseButtonMask = (int)Inno.Native.ImGui.ImGuiPopupFlags.MouseButtonMask,
    /// <summary>
    /// Selects the invalid mask presentation behavior.
    /// </summary>
    InvalidMask = (int)Inno.Native.ImGui.ImGuiPopupFlags.InvalidMask,
}
