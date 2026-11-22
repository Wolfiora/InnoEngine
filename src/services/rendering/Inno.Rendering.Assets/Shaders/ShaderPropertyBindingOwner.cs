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
/// Identifies which layer supplies a declared shader property's value for each dispatch or draw.
/// </summary>
[StableTypeID("0c1a224a-4d13-56de-bda2-93128c1374cb")]
public enum ShaderPropertyBindingOwner
{
    /// <summary>
    /// The material and its optional property block supply the value.
    /// </summary>
    Material,
    /// <summary>
    /// The render pass supplies the value directly through its command encoder.
    /// </summary>
    RenderPass
}
