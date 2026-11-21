using System;

using NativeTableRowFlags = Inno.Native.ImGui.ImGuiTableRowFlags;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported table row flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiTableRowFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeTableRowFlags.None,
    /// <summary>
    /// Identify header row (set default background color + width of its contents accounted differently for auto column width)
    /// </summary>
    Headers = (int)NativeTableRowFlags.Headers
}
