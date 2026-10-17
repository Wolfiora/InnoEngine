using Inno.Core.Diagnostics;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Identifies one provider-owned persistent GPU resource without exposing a native handle.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("c060d558-f0f1-5653-a77f-526e6b078507")]
public readonly record struct RenderPersistentResourceId
{
    /// <summary>
    /// Creates a globally stable persistent resource identifier.
    /// </summary>
    /// <param name="value">
    /// Provider-qualified stable identifier.
    /// </param>
    public RenderPersistentResourceId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the provider-qualified stable identifier.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether the identifier contains a usable value.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}

