namespace Inno.Animation;

/// <summary>
/// Identifies one generation-checked animation playback owned by a runtime service.
/// </summary>
public readonly record struct AnimationPlaybackHandle
{
    internal AnimationPlaybackHandle(
        ulong value,
        uint runtimeGeneration
    ) {
        this.value = value;
        this.runtimeGeneration = runtimeGeneration;
    }

    /// <summary>
    /// Gets whether the handle was initialized by an animation service.
    /// </summary>
    public bool isValid => value != 0 && runtimeGeneration != 0;

    internal ulong value { get; }

    /// <summary>
    /// Gets the isolated runtime generation that created this handle.
    /// </summary>
    public uint runtimeGeneration { get; }
}
