using System;

using NativeButtonFlags = Inno.Native.ImGui.ImGuiButtonFlags;

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
    None = (int)NativeButtonFlags.None,
    /// <summary>
    /// React on left mouse button (default)
    /// </summary>
    MouseButtonLeft = (int)NativeButtonFlags.MouseButtonLeft,
    /// <summary>
    /// React on right mouse button
    /// </summary>
    MouseButtonRight = (int)NativeButtonFlags.MouseButtonRight,
    /// <summary>
    /// React on center mouse button
    /// </summary>
    MouseButtonMiddle = (int)NativeButtonFlags.MouseButtonMiddle,
    /// <summary>
    /// Selects the mouse button mask presentation behavior.
    /// </summary>
    MouseButtonMask = (int)NativeButtonFlags.MouseButtonMask,
    /// <summary>
    /// InvisibleButton(): do not disable navigation/tabbing. Otherwise disabled by default.
    /// </summary>
    EnableNav = (int)NativeButtonFlags.EnableNav
}
