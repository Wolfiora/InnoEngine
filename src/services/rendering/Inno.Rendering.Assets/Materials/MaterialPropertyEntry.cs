using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores one persistent material property entry.
/// </summary>
[StableTypeID("f4c298ba-cda4-5155-a050-d33f70175c7c")]
public struct MaterialPropertyEntry
{
    /// <summary>
    /// Creates a persistent material property entry.
    /// </summary>
    /// <param name="id">
    /// Stable shader property identifier.
    /// </param>
    /// <param name="value">
    /// Neutral material value.
    /// </param>
    public MaterialPropertyEntry(
        ShaderPropertyID id,
        MaterialValue value
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A material property ID must be valid.", nameof(id));
        }
        this.id = id;
        this.value = value;
    }

    /// <summary>
    /// Gets or sets the stable shader property identifier.
    /// </summary>
    public ShaderPropertyID id { get; set; }

    /// <summary>
    /// Gets or sets the neutral material value.
    /// </summary>
    public MaterialValue value { get; set; }
}
