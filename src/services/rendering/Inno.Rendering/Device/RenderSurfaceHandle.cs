namespace Inno.Rendering;

/// <summary>
/// Identifies a persistent presentation surface without exposing a backend framebuffer or swapchain handle.
/// </summary>
public readonly record struct RenderSurfaceHandle
{
    internal RenderSurfaceHandle(
        ulong value,
        uint deviceGeneration
    ) {
        this.value = value;
        this.deviceGeneration = deviceGeneration;
    }

    internal ulong value { get; }
    internal uint deviceGeneration { get; }

    /// <summary>
    /// Gets whether the handle identifies a presentation surface.
    /// </summary>
    public bool isValid => value != 0 && deviceGeneration != 0;
}
