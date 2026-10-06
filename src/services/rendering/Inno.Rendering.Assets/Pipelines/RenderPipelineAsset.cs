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
/// Selects a pipeline extension and ordered feature configuration without defining a render path.
/// </summary>
[StableTypeId("b17d289e-62de-4299-9e85-497143911798")]
public sealed class RenderPipelineAsset : AssetObject
{
    private RenderFeatureConfiguration[] m_features = [];

    /// <summary>
    /// Gets or sets the globally stable pipeline extension identifier.
    /// </summary>
    [SerializableProperty]
    public string pipelineTypeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets reload-safe pipeline settings.
    /// </summary>
    [SerializableProperty]
    public SerializedRenderExtensionState pipelineState { get; set; } = new();

    /// <summary>
    /// Gets ordered feature configurations.
    /// </summary>
    [SerializableProperty]
    public RenderFeatureConfiguration[] features
    {
        get => m_features;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            m_features = value.ToArray();
        }
    }

    /// <summary>
    /// Replaces ordered feature configurations.
    /// </summary>
    /// <param name="features">
    /// Complete ordered feature configuration set.
    /// </param>
    public void SetFeatures(IEnumerable<RenderFeatureConfiguration> features)
    {
        ArgumentNullException.ThrowIfNull(features);
        this.features = features.ToArray();
    }
}

