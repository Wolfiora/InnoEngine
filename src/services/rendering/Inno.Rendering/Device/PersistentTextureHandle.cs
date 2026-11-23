namespace Inno.Rendering;

/// <summary>
/// Identifies a persistent device texture without exposing a backend-native handle.
/// </summary>
public readonly record struct PersistentTextureHandle
{
    internal PersistentTextureHandle(
        ulong value,
        uint deviceGeneration
    ) {
        this.value = value;
        this.deviceGeneration = deviceGeneration;
    }

    internal ulong value { get; }
    internal uint deviceGeneration { get; }

    /// <summary>
    /// Gets whether the handle identifies a device texture.
    /// </summary>
    public bool isValid => value != 0 && deviceGeneration != 0;
}
