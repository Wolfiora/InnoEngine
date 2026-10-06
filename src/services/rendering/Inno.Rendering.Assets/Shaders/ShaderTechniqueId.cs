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
/// Identifies one material-selectable technique in a shader.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("250032b9-5391-5f1c-a220-c66baf0d15d0")]
public record struct ShaderTechniqueId
{
    /// <summary>
    /// Creates a shader technique identifier.
    /// </summary>
    /// <param name="value">
    /// Stable technique value within a shader.
    /// </param>
    public ShaderTechniqueId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets or sets the stable technique value.
    /// </summary>
    public string value { get; set; }

    /// <summary>
    /// Gets whether the identifier has a usable value.
    /// </summary>
    public readonly bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public readonly override string ToString() => value ?? string.Empty;
}

