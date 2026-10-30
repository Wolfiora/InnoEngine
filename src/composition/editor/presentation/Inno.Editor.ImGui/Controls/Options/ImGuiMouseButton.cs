using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported mouse button choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiMouseButton : int
{
    /// <summary>
    /// Selects the left presentation behavior.
    /// </summary>
    Left = (int)Inno.Native.ImGui.ImGuiMouseButton.Left,
    /// <summary>
    /// Selects the right presentation behavior.
    /// </summary>
    Right = (int)Inno.Native.ImGui.ImGuiMouseButton.Right,
    /// <summary>
    /// Selects the middle presentation behavior.
    /// </summary>
    Middle = (int)Inno.Native.ImGui.ImGuiMouseButton.Middle,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)Inno.Native.ImGui.ImGuiMouseButton.Count,
}
