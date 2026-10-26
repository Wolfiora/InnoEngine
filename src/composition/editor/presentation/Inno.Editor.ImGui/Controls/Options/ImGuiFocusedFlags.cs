using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported focused flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiFocusedFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiFocusedFlags.None,
    /// <summary>
    /// Return true if any children of the window is focused
    /// </summary>
    ChildWindows = (int)Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows,
    /// <summary>
    /// Test from root window (top most parent of the current hierarchy)
    /// </summary>
    RootWindow = (int)Inno.Native.ImGui.ImGuiFocusedFlags.RootWindow,
    /// <summary>
    /// Return true if any window is focused. Important: If you are trying to tell how to dispatch your low-level inputs, do NOT use this. Use 'io.WantCaptureMouse' instead! Please read the FAQ!
    /// </summary>
    AnyWindow = (int)Inno.Native.ImGui.ImGuiFocusedFlags.AnyWindow,
    /// <summary>
    /// Do not consider popup hierarchy (do not treat popup emitter as parent of popup) (when used with _ChildWindows or _RootWindow)
    /// </summary>
    NoPopupHierarchy = (int)Inno.Native.ImGui.ImGuiFocusedFlags.NoPopupHierarchy,
    /// <summary>
    /// Consider docking hierarchy (treat dockspace host as parent of docked window) (when used with _ChildWindows or _RootWindow)
    /// </summary>
    DockHierarchy = (int)Inno.Native.ImGui.ImGuiFocusedFlags.DockHierarchy,
    /// <summary>
    /// Selects the root and child windows presentation behavior.
    /// </summary>
    RootAndChildWindows = (int)Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows,
}
