using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported button flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiButtonFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiButtonFlags.None,
    /// <summary>
    /// React on left mouse button (default)
    /// </summary>
    MouseButtonLeft = (int)Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonLeft,
    /// <summary>
    /// React on right mouse button
    /// </summary>
    MouseButtonRight = (int)Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonRight,
    /// <summary>
    /// React on center mouse button
    /// </summary>
    MouseButtonMiddle = (int)Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonMiddle,
    /// <summary>
    /// Selects the mouse button mask presentation behavior.
    /// </summary>
    MouseButtonMask = (int)Inno.Native.ImGui.ImGuiButtonFlags.MouseButtonMask,
    /// <summary>
    /// InvisibleButton(): do not disable navigation/tabbing. Otherwise disabled by default.
    /// </summary>
    EnableNav = (int)Inno.Native.ImGui.ImGuiButtonFlags.EnableNav,
}
