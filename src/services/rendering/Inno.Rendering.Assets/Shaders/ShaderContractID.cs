using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;

namespace Inno.Rendering.Assets;

/// <summary>
/// Identifies an open shader and pipeline compatibility contract.
/// </summary>
[StableTypeID("741c24fb-b13e-5582-98b0-c13e12921fda")]
public record struct ShaderContractID
{
    /// <summary>
    /// Creates a shader contract identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable contract value.
    /// </param>
    public ShaderContractID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets or sets the globally stable contract value.
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
    public override readonly string ToString() => value ?? string.Empty;
}
