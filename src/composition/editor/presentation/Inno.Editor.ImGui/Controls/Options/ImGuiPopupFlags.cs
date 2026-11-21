using System;

using NativePopupFlags = Inno.Native.ImGui.ImGuiPopupFlags;

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
    None = (int)NativePopupFlags.None,
    /// <summary>
    /// For BeginPopupContext*(): open on Left Mouse release. Only one button allowed!
    /// </summary>
    MouseButtonLeft = (int)NativePopupFlags.MouseButtonLeft,
    /// <summary>
    /// For BeginPopupContext*(): open on Right Mouse release. Only one button allowed! (default)
    /// </summary>
    MouseButtonRight = (int)NativePopupFlags.MouseButtonRight,
    /// <summary>
    /// For BeginPopupContext*(): open on Middle Mouse release. Only one button allowed!
    /// </summary>
    MouseButtonMiddle = (int)NativePopupFlags.MouseButtonMiddle,
    /// <summary>
    /// For OpenPopup*(), BeginPopupContext*(): don't reopen same popup if already open (won't reposition, won't reinitialize navigation)
    /// </summary>
    NoReopen = (int)NativePopupFlags.NoReopen,
    /// <summary>
    /// For OpenPopup*(), BeginPopupContext*(): don't open if there's already a popup at the same level of the popup stack
    /// </summary>
    NoOpenOverExistingPopup = (int)NativePopupFlags.NoOpenOverExistingPopup,
    /// <summary>
    /// For BeginPopupContextWindow(): don't return true when hovering items, only when hovering empty space
    /// </summary>
    NoOpenOverItems = (int)NativePopupFlags.NoOpenOverItems,
    /// <summary>
    /// For IsPopupOpen(): ignore the ImGuiID parameter and test for any popup.
    /// </summary>
    AnyPopupId = (int)NativePopupFlags.AnyPopupId,
    /// <summary>
    /// For IsPopupOpen(): search/test at any level of the popup stack (default test in the current level)
    /// </summary>
    AnyPopupLevel = (int)NativePopupFlags.AnyPopupLevel,
    /// <summary>
    /// Selects the any popup presentation behavior.
    /// </summary>
    AnyPopup = (int)NativePopupFlags.AnyPopup,
    /// <summary>
    /// Selects the mouse button shift presentation behavior.
    /// </summary>
    MouseButtonShift = (int)NativePopupFlags.MouseButtonShift,
    /// <summary>
    /// Selects the mouse button mask presentation behavior.
    /// </summary>
    MouseButtonMask = (int)NativePopupFlags.MouseButtonMask,
    /// <summary>
    /// Selects the invalid mask presentation behavior.
    /// </summary>
    InvalidMask = (int)NativePopupFlags.InvalidMask
}
