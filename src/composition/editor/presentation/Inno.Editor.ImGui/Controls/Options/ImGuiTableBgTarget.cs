using System;

using NativeTableBgTarget = Inno.Native.ImGui.ImGuiTableBgTarget;

namespace Inno.Editor.ImGui;

/// <summary>
/// Defines the supported table bg target choices at the Editor presentation boundary.
/// </summary>
public enum ImGuiTableBgTarget : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)NativeTableBgTarget.None,
    /// <summary>
    /// Set row background color 0 (generally used for background, automatically set when ImGuiTableFlags_RowBg is used)
    /// </summary>
    RowBg0 = (int)NativeTableBgTarget.RowBg0,
    /// <summary>
    /// Set row background color 1 (generally used for selection marking)
    /// </summary>
    RowBg1 = (int)NativeTableBgTarget.RowBg1,
    /// <summary>
    /// Set cell background color (top-most color)
    /// </summary>
    CellBg = (int)NativeTableBgTarget.CellBg
}
