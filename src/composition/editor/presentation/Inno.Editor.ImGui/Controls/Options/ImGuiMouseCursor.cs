using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported mouse cursor choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiMouseCursor : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiMouseCursor.None,
    /// <summary>
    /// Selects the arrow presentation behavior.
    /// </summary>
    Arrow = (int)Inno.Native.ImGui.ImGuiMouseCursor.Arrow,
    /// <summary>
    /// Selects the text input presentation behavior.
    /// </summary>
    TextInput = (int)Inno.Native.ImGui.ImGuiMouseCursor.TextInput,
    /// <summary>
    /// Selects the resize all presentation behavior.
    /// </summary>
    ResizeAll = (int)Inno.Native.ImGui.ImGuiMouseCursor.ResizeAll,
    /// <summary>
    /// Selects the resize ns presentation behavior.
    /// </summary>
    ResizeNs = (int)Inno.Native.ImGui.ImGuiMouseCursor.ResizeNs,
    /// <summary>
    /// Selects the resize ew presentation behavior.
    /// </summary>
    ResizeEw = (int)Inno.Native.ImGui.ImGuiMouseCursor.ResizeEw,
    /// <summary>
    /// Selects the resize nesw presentation behavior.
    /// </summary>
    ResizeNesw = (int)Inno.Native.ImGui.ImGuiMouseCursor.ResizeNesw,
    /// <summary>
    /// Selects the resize nwse presentation behavior.
    /// </summary>
    ResizeNwse = (int)Inno.Native.ImGui.ImGuiMouseCursor.ResizeNwse,
    /// <summary>
    /// Selects the hand presentation behavior.
    /// </summary>
    Hand = (int)Inno.Native.ImGui.ImGuiMouseCursor.Hand,
    /// <summary>
    /// Selects the wait presentation behavior.
    /// </summary>
    Wait = (int)Inno.Native.ImGui.ImGuiMouseCursor.Wait,
    /// <summary>
    /// Selects the progress presentation behavior.
    /// </summary>
    Progress = (int)Inno.Native.ImGui.ImGuiMouseCursor.Progress,
    /// <summary>
    /// Selects the not allowed presentation behavior.
    /// </summary>
    NotAllowed = (int)Inno.Native.ImGui.ImGuiMouseCursor.NotAllowed,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)Inno.Native.ImGui.ImGuiMouseCursor.Count,
}
