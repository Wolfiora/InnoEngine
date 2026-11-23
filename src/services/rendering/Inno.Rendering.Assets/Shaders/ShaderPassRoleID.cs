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
/// Identifies an open pass purpose defined by a rendering provider.
/// </summary>
[StableTypeID("56d381cc-a73a-595b-824c-05191b9ce303")]
public record struct ShaderPassRoleID
{
    /// <summary>
    /// Creates a shader pass role identifier.
    /// </summary>
    /// <param name="value">
    /// Stable role value within a contract.
    /// </param>
    public ShaderPassRoleID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets or sets the stable role value.
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
