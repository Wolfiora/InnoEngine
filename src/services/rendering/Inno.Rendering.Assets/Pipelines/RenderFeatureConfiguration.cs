using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores one ordered feature extension selection using only stable data.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("aa788bff-6891-5055-8762-899971ba3bed")]
public struct RenderFeatureConfiguration
{
    /// <summary>
    /// Creates an empty feature configuration for deserialization.
    /// </summary>
    public RenderFeatureConfiguration()
    {
    }

    /// <summary>
    /// Creates a feature configuration.
    /// </summary>
    /// <param name="featureTypeId">
    /// Globally stable feature extension identifier.
    /// </param>
    /// <param name="state">
    /// Optional reload-safe settings state.
    /// </param>
    /// <param name="enabled">
    /// Whether the feature participates in graph building.
    /// </param>
    public RenderFeatureConfiguration(
        string featureTypeId,
        SerializedRenderExtensionState? state = null,
        bool enabled = true
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(featureTypeId);
        this.featureTypeId = featureTypeId;
        this.state = state ?? new SerializedRenderExtensionState();
        this.enabled = enabled;
    }

    /// <summary>
    /// Gets or sets the stable feature extension identifier.
    /// </summary>
    [SerializableProperty]
    public string featureTypeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets reload-safe settings state.
    /// </summary>
    [SerializableProperty]
    public SerializedRenderExtensionState state { get; set; }

    /// <summary>
    /// Gets or sets whether the feature participates in graph building.
    /// </summary>
    [SerializableProperty]
    public bool enabled { get; set; } = true;
}

