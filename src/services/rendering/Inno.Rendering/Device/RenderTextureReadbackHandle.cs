namespace Inno.Rendering;

/// <summary>
/// Identifies one backend-neutral asynchronous texture readback operation.
/// </summary>
public readonly record struct RenderTextureReadbackHandle
{
    internal RenderTextureReadbackHandle(
        ulong value,
        uint deviceGeneration
    ) {
        this.value = value;
        this.deviceGeneration = deviceGeneration;
    }

    internal ulong value { get; }
    internal uint deviceGeneration { get; }

    /// <summary>
    /// Gets whether the handle identifies a device readback operation.
    /// </summary>
    public bool isValid => value != 0 && deviceGeneration != 0;
}
