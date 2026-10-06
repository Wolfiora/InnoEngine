using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores one open material metadata key and value.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("38365af5-31ec-5646-8f17-42c31c98f7c2")]
public struct MaterialMetadataEntry
{
    /// <summary>
    /// Creates a material metadata entry.
    /// </summary>
    /// <param name="key">
    /// Stable provider-defined key.
    /// </param>
    /// <param name="value">
    /// Provider-defined value.
    /// </param>
    public MaterialMetadataEntry(
        string key,
        string value
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        this.key = key;
        this.value = value ?? string.Empty;
    }

    /// <summary>
    /// Gets or sets the stable metadata key.
    /// </summary>
    public string key { get; set; }

    /// <summary>
    /// Gets or sets the metadata value.
    /// </summary>
    public string value { get; set; }
}

