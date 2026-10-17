using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Marks a reloadable pipeline feature implementation with a stable extension identifier.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Inno.Extensibility.Types.StableTypeId("f290f8ca-3a8b-5e90-aa2d-1b0a541cd95a")]
public sealed class RenderFeatureExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a feature extension declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable feature extension identifier.
    /// </param>
    public RenderFeatureExtensionAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the globally stable feature extension identifier.
    /// </summary>
    public string id { get; }
}

