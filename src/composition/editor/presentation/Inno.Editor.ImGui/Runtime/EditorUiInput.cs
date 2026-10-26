using System.Numerics;
using Inno.Native.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Copies frame input facts used by presentation without exposing native input storage.
/// </summary>
public readonly struct EditorUiInput
{
    /// <summary>
    /// Gets whether Alt is held.
    /// </summary>
    public bool alt { get; }
    /// <summary>
    /// Gets whether Control is held.
    /// </summary>
    public bool control { get; }
    /// <summary>
    /// Gets whether Shift is held.
    /// </summary>
    public bool shift { get; }
    /// <summary>
    /// Gets whether the platform super key is held.
    /// </summary>
    public bool super { get; }
    /// <summary>
    /// Gets whether an active control owns text input.
    /// </summary>
    public bool wantsText { get; }
    /// <summary>
    /// Gets the current pointer movement in UI units.
    /// </summary>
    public Vector2 mouseDelta { get; }
    /// <summary>
    /// Gets the vertical scroll movement.
    /// </summary>
    public float wheel { get; }
    /// <summary>
    /// Gets the frame duration in seconds.
    /// </summary>
    public float deltaTime { get; }
    /// <summary>
    /// Gets the drawable pixel scale relative to logical UI coordinates.
    /// </summary>
    public Vector2 framebufferScale { get; }

    internal unsafe EditorUiInput(ImGuiIO* io)
    {
        alt = io->KeyAlt != 0;
        control = io->KeyCtrl != 0;
        shift = io->KeyShift != 0;
        super = io->KeySuper != 0;
        wantsText = io->WantTextInput != 0;
        mouseDelta = ImGui.FromNative(io->MouseDelta);
        framebufferScale = ImGui.FromNative(io->DisplayFramebufferScale);
        wheel = io->MouseWheel;
        deltaTime = io->DeltaTime;
    }
}
