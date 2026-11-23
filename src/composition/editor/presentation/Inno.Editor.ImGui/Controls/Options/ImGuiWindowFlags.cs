using System;

using NativeWindowFlags = Inno.Native.ImGui.ImGuiWindowFlags;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported window flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiWindowFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeWindowFlags.None,
    /// <summary>
    /// Disable title-bar
    /// </summary>
    NoTitleBar = (int)NativeWindowFlags.NoTitleBar,
    /// <summary>
    /// Disable user resizing with the lower-right grip
    /// </summary>
    NoResize = (int)NativeWindowFlags.NoResize,
    /// <summary>
    /// Disable user moving the window
    /// </summary>
    NoMove = (int)NativeWindowFlags.NoMove,
    /// <summary>
    /// Disable scrollbars (window can still scroll with mouse or programmatically)
    /// </summary>
    NoScrollbar = (int)NativeWindowFlags.NoScrollbar,
    /// <summary>
    /// Disable user vertically scrolling with mouse wheel. On child window, mouse wheel will be forwarded to the parent unless NoScrollbar is also set.
    /// </summary>
    NoScrollWithMouse = (int)NativeWindowFlags.NoScrollWithMouse,
    /// <summary>
    /// Disable user collapsing window by double-clicking on it. Also referred to as Window Menu Button (e.g. within a docking node).
    /// </summary>
    NoCollapse = (int)NativeWindowFlags.NoCollapse,
    /// <summary>
    /// Resize every window to its content every frame
    /// </summary>
    AlwaysAutoResize = (int)NativeWindowFlags.AlwaysAutoResize,
    /// <summary>
    /// Disable drawing background color (WindowBg, etc.) and outside border. Similar as using SetNextWindowBgAlpha(0.0f).
    /// </summary>
    NoBackground = (int)NativeWindowFlags.NoBackground,
    /// <summary>
    /// Never load/save settings in .ini file
    /// </summary>
    NoSavedSettings = (int)NativeWindowFlags.NoSavedSettings,
    /// <summary>
    /// Disable catching mouse, hovering test with pass through.
    /// </summary>
    NoMouseInputs = (int)NativeWindowFlags.NoMouseInputs,
    /// <summary>
    /// Has a menu-bar
    /// </summary>
    MenuBar = (int)NativeWindowFlags.MenuBar,
    /// <summary>
    /// Allow horizontal scrollbar to appear (off by default). You may use SetNextWindowContentSize(ImVec2(width,0.0f)); prior to calling Begin() to specify width. Read code in imgui_demo in the "Horizontal Scrolling" section.
    /// </summary>
    HorizontalScrollbar = (int)NativeWindowFlags.HorizontalScrollbar,
    /// <summary>
    /// Disable taking focus when transitioning from hidden to visible state
    /// </summary>
    NoFocusOnAppearing = (int)NativeWindowFlags.NoFocusOnAppearing,
    /// <summary>
    /// Disable bringing window to front when taking focus (e.g. clicking on it or programmatically giving it focus)
    /// </summary>
    NoBringToFrontOnFocus = (int)NativeWindowFlags.NoBringToFrontOnFocus,
    /// <summary>
    /// Always show vertical scrollbar (even if ContentSize.y &lt; Size.y)
    /// </summary>
    AlwaysVerticalScrollbar = (int)NativeWindowFlags.AlwaysVerticalScrollbar,
    /// <summary>
    /// Always show horizontal scrollbar (even if ContentSize.x &lt; Size.x)
    /// </summary>
    AlwaysHorizontalScrollbar = (int)NativeWindowFlags.AlwaysHorizontalScrollbar,
    /// <summary>
    /// No keyboard/gamepad navigation within the window
    /// </summary>
    NoNavInputs = (int)NativeWindowFlags.NoNavInputs,
    /// <summary>
    /// No focusing toward this window with keyboard/gamepad navigation (e.g. skipped by Ctrl+Tab)
    /// </summary>
    NoNavFocus = (int)NativeWindowFlags.NoNavFocus,
    /// <summary>
    /// Display a dot next to the title. When used in a tab/docking context, tab is selected when clicking the X + closure is not assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
    /// </summary>
    UnsavedDocument = (int)NativeWindowFlags.UnsavedDocument,
    /// <summary>
    /// Disable docking of this window
    /// </summary>
    NoDocking = (int)NativeWindowFlags.NoDocking,
    /// <summary>
    /// Selects the no nav presentation behavior.
    /// </summary>
    NoNav = (int)NativeWindowFlags.NoNav,
    /// <summary>
    /// Selects the no decoration presentation behavior.
    /// </summary>
    NoDecoration = (int)NativeWindowFlags.NoDecoration,
    /// <summary>
    /// Selects the no inputs presentation behavior.
    /// </summary>
    NoInputs = (int)NativeWindowFlags.NoInputs,
    /// <summary>
    /// Don't use! For internal use by Begin()/NewFrame()
    /// </summary>
    DockNodeHost = (int)NativeWindowFlags.DockNodeHost,
    /// <summary>
    /// Don't use! For internal use by BeginChild()
    /// </summary>
    ChildWindow = (int)NativeWindowFlags.ChildWindow,
    /// <summary>
    /// Don't use! For internal use by BeginTooltip()
    /// </summary>
    Tooltip = (int)NativeWindowFlags.Tooltip,
    /// <summary>
    /// Don't use! For internal use by BeginPopup()
    /// </summary>
    Popup = (int)NativeWindowFlags.Popup,
    /// <summary>
    /// Don't use! For internal use by BeginPopupModal()
    /// </summary>
    Modal = (int)NativeWindowFlags.Modal,
    /// <summary>
    /// Don't use! For internal use by BeginMenu()
    /// </summary>
    ChildMenu = (int)NativeWindowFlags.ChildMenu
}
