using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Identifies a material property using a stable serialized string.
/// </summary>
public record struct ShaderPropertyId
{
    /// <summary>
    /// Creates a stable shader property identifier.
    /// </summary>
    /// <param name="value">
    /// Stable manifest property identifier.
    /// </param>
    public ShaderPropertyId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets or sets the stable manifest property identifier.
    /// </summary>
    public string value { get; set; }

    /// <summary>
    /// Gets whether this identifier has a usable value.
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

