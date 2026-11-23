using System;
using System.Linq;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Audio;

/// <summary>
/// Selects a mixer extension and ordered features without defining a game-specific mixing model.
/// </summary>
[StableTypeID("6e167326-d078-45e9-ad33-993300ee5fed")]
public sealed class AudioMixerAsset : AssetObject
{
    /// <summary>
    /// Gets or sets the globally stable mixer extension identifier.
    /// </summary>
    [SerializableProperty]
    public string mixerTypeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets reload-safe mixer settings.
    /// </summary>
    [SerializableProperty]
    public SerializedAudioExtensionState mixerState { get; set; } = new();

    /// <summary>
    /// Gets or sets ordered feature configurations.
    /// </summary>
    [SerializableProperty]
    public AudioMixerFeatureConfiguration[] features
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value.ToArray();
        }
    } = [];
}
