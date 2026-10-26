using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported cond choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiCond : int
{
    /// <summary>
    /// No condition (always set the variable), same as _Always
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiCond.None,
    /// <summary>
    /// No condition (always set the variable), same as _None
    /// </summary>
    Always = (int)Inno.Native.ImGui.ImGuiCond.Always,
    /// <summary>
    /// Set the variable once per runtime session (only the first call will succeed)
    /// </summary>
    Once = (int)Inno.Native.ImGui.ImGuiCond.Once,
    /// <summary>
    /// Set the variable if the object/window has no persistently saved data (no entry in .ini file)
    /// </summary>
    FirstUseEver = (int)Inno.Native.ImGui.ImGuiCond.FirstUseEver,
    /// <summary>
    /// Set the variable if the object/window is appearing after being hidden/inactive (or the first time)
    /// </summary>
    Appearing = (int)Inno.Native.ImGui.ImGuiCond.Appearing,
}
