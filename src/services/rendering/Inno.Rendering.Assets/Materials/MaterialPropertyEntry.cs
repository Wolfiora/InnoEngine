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
/// Stores one persistent material property entry.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("f4c298ba-cda4-5155-a050-d33f70175c7c")]
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
        ShaderPropertyId id,
        MaterialValue value
    ) {
        if (!id.isValid)
            throw new ArgumentException("A material property ID must be valid.", nameof(id));
        this.id = id;
        this.value = value;
    }

    /// <summary>
    /// Gets or sets the stable shader property identifier.
    /// </summary>
    public ShaderPropertyId id { get; set; }

    /// <summary>
    /// Gets or sets the neutral material value.
    /// </summary>
    public MaterialValue value { get; set; }
}

