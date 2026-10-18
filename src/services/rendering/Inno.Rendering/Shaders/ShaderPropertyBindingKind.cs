using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Defines how one shader property enters the backend-neutral resource interface.
/// </summary>
public enum ShaderPropertyBindingKind
{
    /// <summary>
    /// Vector or matrix uniform data.
    /// </summary>
    Uniform,
    /// <summary>
    /// Texture sampled through an explicit material sampler.
    /// </summary>
    SampledTexture,
    /// <summary>
    /// Texture bound for unordered shader access by a Pipeline.
    /// </summary>
    StorageTexture,
    /// <summary>
    /// Buffer bound for unordered shader access by a Pipeline.
    /// </summary>
    StorageBuffer
}

