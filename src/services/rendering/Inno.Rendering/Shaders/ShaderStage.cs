using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;

namespace Inno.Rendering;

/// <summary>
/// Identifies a programmable shader stage.
/// </summary>
[Flags]
public enum ShaderStage
{
    /// <summary>
    /// No shader stage.
    /// </summary>
    None = 0,
    /// <summary>
    /// Vertex shader stage.
    /// </summary>
    Vertex = 1 << 0,
    /// <summary>
    /// Fragment shader stage.
    /// </summary>
    Fragment = 1 << 1,
    /// <summary>
    /// Compute shader stage.
    /// </summary>
    Compute = 1 << 2
}
