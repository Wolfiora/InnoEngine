using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores reload-safe configuration for one pipeline or feature extension generation.
/// </summary>
[StableTypeId("8683f095-765d-5aa3-9a9f-fb95777885c5")]
public struct SerializedRenderExtensionState
{
    /// <summary>
    /// Creates empty extension state for deserialization.
    /// </summary>
    public SerializedRenderExtensionState()
    {
        this.propertyData = [];
        this.dependencies = [];
    }

    /// <summary>
    /// Copies an owner-captured property snapshot, preserving its automatic resource dependency declarations.
    /// </summary>
    /// <param name="properties">
    /// Complete neutral properties produced by the asset authoring services.
    /// </param>
    public SerializedRenderExtensionState(AssetPropertySnapshot properties)
    {
        ArgumentNullException.ThrowIfNull(properties);
        this.stableTypeId = properties.stableTypeId;
        this.propertyData = properties.data.ToArray();
        this.dependencies = properties.dependencies.ToArray();
    }

    /// <summary>
    /// Creates immutable extension state from stable identity and property bytes.
    /// </summary>
    /// <param name="stableTypeId">
    /// Stable type identity, or empty when the extension has no typed settings.
    /// </param>
    /// <param name="propertyData">
    /// Neutral native property bytes produced by asset serialization. Use the snapshot constructor when settings reference assets.
    /// </param>
    public SerializedRenderExtensionState(
        Guid stableTypeId,
        ReadOnlySpan<byte> propertyData
    ) {
        this.stableTypeId = stableTypeId;
        this.propertyData = propertyData.ToArray();
        this.dependencies = [];
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

    /// <summary>
    /// Gets or sets asset dependencies captured with the neutral property payload.
    /// </summary>
    [SerializableProperty]
    public AssetDependency[] dependencies { get; set; }
}
