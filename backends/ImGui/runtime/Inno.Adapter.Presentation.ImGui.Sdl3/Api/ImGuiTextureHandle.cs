using System;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Identifies a backend-owned texture through an opaque ImGui token instead of a native graphics handle.
/// </summary>
public readonly record struct ImGuiTextureHandle
{
    /// <summary>
    /// Creates an opaque texture token allocated by an ImGui presentation backend.
    /// </summary>
    /// <param name="value">
    /// Non-zero backend token.
    /// </param>
    public ImGuiTextureHandle(ulong value)
    {
        ArgumentOutOfRangeException.ThrowIfZero(value);

        this.value = value;
    }

    /// <summary>
    /// Gets the opaque token consumed only by the active ImGui backend.
    /// </summary>
    public ulong value { get; }

    /// <summary>
    /// Gets whether this token identifies a registered texture.
    /// </summary>
    public bool isValid => value != 0;
}
