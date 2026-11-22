using System;
using Inno.Core.Serialization;

namespace Inno.Audio;

/// <summary>
/// Stores one ordered mixer feature selection using only stable data.
/// </summary>
public struct AudioMixerFeatureConfiguration
{
    /// <summary>
    /// Creates an empty feature configuration for deserialization.
    /// </summary>
    public AudioMixerFeatureConfiguration() {}

    /// <summary>
    /// Creates a mixer feature configuration.
    /// </summary>
    /// <param name="featureTypeID">
    /// Globally stable feature extension identifier.
    /// </param>
    /// <param name="state">
    /// Optional reload-safe feature state.
    /// </param>
    /// <param name="enabled">
    /// Whether the feature participates in graph construction.
    /// </param>
    public AudioMixerFeatureConfiguration(
        string featureTypeID,
        SerializedAudioExtensionState? state = null,
        bool enabled = true
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(featureTypeID);
        this.featureTypeId = featureTypeID;
        this.state = state ?? new SerializedAudioExtensionState();
        this.enabled = enabled;
    }

    /// <summary>
    /// Gets or sets the stable feature extension identifier.
    /// </summary>
    [SerializableProperty]
    public string featureTypeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets reload-safe feature settings.
    /// </summary>
    [SerializableProperty]
    public SerializedAudioExtensionState state { get; set; }

    /// <summary>
    /// Gets or sets whether the feature participates in graph construction.
    /// </summary>
    [SerializableProperty]
    public bool enabled { get; set; } = true;
}
