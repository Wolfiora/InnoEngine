using System;
using Inno.Core.Serialization;

namespace Inno.Audio;

/// <summary>
/// Stores reload-safe configuration for one mixer or feature extension generation.
/// </summary>
public struct SerializedAudioExtensionState
{
    /// <summary>
    /// Creates empty extension state for deserialization.
    /// </summary>
    public SerializedAudioExtensionState()
    {
        this.propertyData = [];
    }

    /// <summary>
    /// Creates neutral extension state.
    /// </summary>
    /// <param name="stableTypeID">
    /// Stable settings type identity, or empty when no typed settings exist.
    /// </param>
    /// <param name="propertyData">
    /// Neutral serialized property bytes.
    /// </param>
    public SerializedAudioExtensionState(
        Guid stableTypeID,
        ReadOnlySpan<byte> propertyData
    ) {
        this.stableTypeId = stableTypeID;
        this.propertyData = propertyData.ToArray();
    }

    /// <summary>
    /// Gets or sets the stable settings type identity.
    /// </summary>
    [SerializableProperty]
    public Guid stableTypeId { get; set; }

    /// <summary>
    /// Gets or sets neutral serialized property bytes.
    /// </summary>
    [SerializableProperty]
    public byte[] propertyData { get; set; }
}
