using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores one open metadata key and value.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("239d558b-cead-5efc-82d8-aee339c78f59")]
public struct ShaderMetadataEntry
{
    /// <summary>
    /// Creates one metadata entry.
    /// </summary>
    /// <param name="key">
    /// Stable provider-defined key.
    /// </param>
    /// <param name="value">
    /// Provider-defined value.
    /// </param>
    public ShaderMetadataEntry(
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

