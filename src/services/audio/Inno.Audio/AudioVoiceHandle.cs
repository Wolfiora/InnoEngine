namespace Inno.Audio;

/// <summary>
/// Identifies one playback voice within a device generation.
/// </summary>
public readonly record struct AudioVoiceHandle
{
    internal AudioVoiceHandle(
        ulong value,
        uint ownerGeneration
    ) {
        this.value = value;
        this.ownerGeneration = ownerGeneration;
    }

    internal ulong value { get; }

    /// <summary>
    /// Gets the generation of the device that created this handle.
    /// </summary>
    public uint ownerGeneration { get; }

    /// <summary>
    /// Gets whether the handle contains a non-zero backend identity and generation.
    /// </summary>
    public bool isValid => value != 0 && ownerGeneration != 0;
}
