using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Selects how texture samples are decoded for shader use.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("da42452c-1c45-5a09-8040-e72e37c00361")]
public enum TextureColorSpace
{
    /// <summary>
    /// Samples are interpreted as linear values.
    /// </summary>
    Linear,
    /// <summary>
    /// Color samples are decoded from sRGB at sampling time.
    /// </summary>
    Srgb
}

