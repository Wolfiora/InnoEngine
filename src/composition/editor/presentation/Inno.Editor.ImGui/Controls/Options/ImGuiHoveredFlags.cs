using System;

using NativeHoveredFlags = Inno.Native.ImGui.ImGuiHoveredFlags;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported hovered flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiHoveredFlags : int
{
    /// <summary>
    /// Return true if directly over the item/window, not obstructed by another window, not obstructed by an active popup or modal blocking inputs under them.
    /// </summary>
    None = (int)NativeHoveredFlags.None,
    /// <summary>
    /// IsWindowHovered() only: Return true if any children of the window is hovered
    /// </summary>
    ChildWindows = (int)NativeHoveredFlags.ChildWindows,
    /// <summary>
    /// IsWindowHovered() only: Test from root window (top most parent of the current hierarchy)
    /// </summary>
    RootWindow = (int)NativeHoveredFlags.RootWindow,
    /// <summary>
    /// IsWindowHovered() only: Return true if any window is hovered
    /// </summary>
    AnyWindow = (int)NativeHoveredFlags.AnyWindow,
    /// <summary>
    /// IsWindowHovered() only: Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)
    /// </summary>
    NoPopupHierarchy = (int)NativeHoveredFlags.NoPopupHierarchy,
    /// <summary>
    /// IsWindowHovered() only: Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)
    /// </summary>
    DockHierarchy = (int)NativeHoveredFlags.DockHierarchy,
    /// <summary>
    /// Return true even if a popup window is normally blocking access to this item/window
    /// </summary>
    AllowWhenBlockedByPopup = (int)NativeHoveredFlags.AllowWhenBlockedByPopup,
    /// <summary>
    /// Return true even if an active item is blocking access to this item/window. Useful for Drag and Drop patterns.
    /// </summary>
    AllowWhenBlockedByActiveItem = (int)NativeHoveredFlags.AllowWhenBlockedByActiveItem,
    /// <summary>
    /// IsItemHovered() only: Return true even if the item uses AllowOverlap mode and is overlapped by another hoverable item.
    /// </summary>
    AllowWhenOverlappedByItem = (int)NativeHoveredFlags.AllowWhenOverlappedByItem,
    /// <summary>
    /// IsItemHovered() only: Return true even if the position is obstructed or overlapped by another window.
    /// </summary>
    AllowWhenOverlappedByWindow = (int)NativeHoveredFlags.AllowWhenOverlappedByWindow,
    /// <summary>
    /// IsItemHovered() only: Return true even if the item is disabled
    /// </summary>
    AllowWhenDisabled = (int)NativeHoveredFlags.AllowWhenDisabled,
    /// <summary>
    /// IsItemHovered() only: Disable using keyboard/gamepad navigation state when active, always query mouse
    /// </summary>
    NoNavOverride = (int)NativeHoveredFlags.NoNavOverride,
    /// <summary>
    /// Selects the allow when overlapped presentation behavior.
    /// </summary>
    AllowWhenOverlapped = (int)NativeHoveredFlags.AllowWhenOverlapped,
    /// <summary>
    /// Selects the rect only presentation behavior.
    /// </summary>
    RectOnly = (int)NativeHoveredFlags.RectOnly,
    /// <summary>
    /// Selects the root and child windows presentation behavior.
    /// </summary>
    RootAndChildWindows = (int)NativeHoveredFlags.RootAndChildWindows,
    /// <summary>
    /// Shortcut for standard flags when using IsItemHovered() + SetTooltip() sequence.
    /// </summary>
    ForTooltip = (int)NativeHoveredFlags.ForTooltip,
    /// <summary>
    /// Require mouse to be stationary for style.HoverStationaryDelay (~0.15 sec) _at least one time_. After this, can move on same item/window. Using the stationary test tends to reduces the need for a long delay.
    /// </summary>
    Stationary = (int)NativeHoveredFlags.Stationary,
    /// <summary>
    /// IsItemHovered() only: Return true immediately (default). As this is the default you generally ignore this.
    /// </summary>
    DelayNone = (int)NativeHoveredFlags.DelayNone,
    /// <summary>
    /// IsItemHovered() only: Return true after style.HoverDelayShort elapsed (~0.15 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).
    /// </summary>
    DelayShort = (int)NativeHoveredFlags.DelayShort,
    /// <summary>
    /// IsItemHovered() only: Return true after style.HoverDelayNormal elapsed (~0.40 sec) (shared between items) + requires mouse to be stationary for style.HoverStationaryDelay (once per item).
    /// </summary>
    DelayNormal = (int)NativeHoveredFlags.DelayNormal,
    /// <summary>
    /// IsItemHovered() only: Disable shared delay system where moving from one item to the next keeps the previous timer for a short time (standard for tooltips with long delays)
    /// </summary>
    NoSharedDelay = (int)NativeHoveredFlags.NoSharedDelay
}
