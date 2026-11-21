using System;

using NativeMouseCursor = Inno.Native.ImGui.ImGuiMouseCursor;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported mouse cursor choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiMouseCursor : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeMouseCursor.None,
    /// <summary>
    /// Selects the arrow presentation behavior.
    /// </summary>
    Arrow = (int)NativeMouseCursor.Arrow,
    /// <summary>
    /// Selects the text input presentation behavior.
    /// </summary>
    TextInput = (int)NativeMouseCursor.TextInput,
    /// <summary>
    /// Selects the resize all presentation behavior.
    /// </summary>
    ResizeAll = (int)NativeMouseCursor.ResizeAll,
    /// <summary>
    /// Selects the resize ns presentation behavior.
    /// </summary>
    ResizeNs = (int)NativeMouseCursor.ResizeNs,
    /// <summary>
    /// Selects the resize ew presentation behavior.
    /// </summary>
    ResizeEw = (int)NativeMouseCursor.ResizeEw,
    /// <summary>
    /// Selects the resize nesw presentation behavior.
    /// </summary>
    ResizeNesw = (int)NativeMouseCursor.ResizeNesw,
    /// <summary>
    /// Selects the resize nwse presentation behavior.
    /// </summary>
    ResizeNwse = (int)NativeMouseCursor.ResizeNwse,
    /// <summary>
    /// Selects the hand presentation behavior.
    /// </summary>
    Hand = (int)NativeMouseCursor.Hand,
    /// <summary>
    /// Selects the wait presentation behavior.
    /// </summary>
    Wait = (int)NativeMouseCursor.Wait,
    /// <summary>
    /// Selects the progress presentation behavior.
    /// </summary>
    Progress = (int)NativeMouseCursor.Progress,
    /// <summary>
    /// Selects the not allowed presentation behavior.
    /// </summary>
    NotAllowed = (int)NativeMouseCursor.NotAllowed,
    /// <summary>
    /// Marks the exclusive end of the valid option range.
    /// </summary>
    Count = (int)NativeMouseCursor.Count
}
