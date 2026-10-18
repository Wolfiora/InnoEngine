using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Marks a reloadable rendering model with a stable identity.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Inno.Extensibility.Types.StableTypeId("eccab85a-0b21-55e1-b581-885c4bbc5192")]
public sealed class RenderModelExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a model declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable model identity.
    /// </param>
    public RenderModelExtensionAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the globally stable model identity.
    /// </summary>
    public string id { get; }
}

