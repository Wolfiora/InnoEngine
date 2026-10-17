using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Marks a reloadable render pipeline implementation with a stable extension identifier.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[Inno.Extensibility.Types.StableTypeId("bbaa3835-1ee7-5e0f-b690-304d4f6f98ed")]
public sealed class RenderPipelineExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a pipeline extension declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable pipeline extension identifier.
    /// </param>
    public RenderPipelineExtensionAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the globally stable pipeline extension identifier.
    /// </summary>
    public string id { get; }
}

