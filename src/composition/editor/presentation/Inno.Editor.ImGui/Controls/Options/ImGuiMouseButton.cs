using System;

using NativeMouseButton = Inno.Native.ImGui.ImGuiMouseButton;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported mouse button choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiMouseButton : int
{
    /// <summary>
    /// Selects the left presentation behavior.
    /// </summary>
    Left = (int)NativeMouseButton.Left,
    /// <summary>
    /// Selects the right presentation behavior.
    /// </summary>
    Right = (int)NativeMouseButton.Right,
    /// <summary>
    /// Selects the middle presentation behavior.
    /// </summary>
    Middle = (int)NativeMouseButton.Middle,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)NativeMouseButton.Count
}
