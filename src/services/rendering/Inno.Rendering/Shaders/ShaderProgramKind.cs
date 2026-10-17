using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Selects the programmable stage combination of a pass.
/// </summary>
public enum ShaderProgramKind
{
    /// <summary>
    /// Vertex and fragment stages used by a raster pass.
    /// </summary>
    Raster,
    /// <summary>
    /// A compute stage used by a compute pass.
    /// </summary>
    Compute
}

