using Inno.Core.Serialization;

namespace Inno.Animation;

/// <summary>
/// Stores one stable event marker embedded in an animation clip.
/// </summary>
public sealed class AnimationEventMarker : ISerializable
{
    /// <summary>
    /// Gets or sets clip-local marker time in seconds.
    /// </summary>
    [SerializableProperty]
    public float time { get; set; }

    /// <summary>
    /// Gets or sets the stable event protocol identifier.
    /// </summary>
    [SerializableProperty]
    public string eventId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets opaque reload-safe event payload bytes.
    /// </summary>
    [SerializableProperty]
    public byte[] payload { get; set; } = [];
}
