using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Identifies an artist-facing shader property type.
/// </summary>
public enum ShaderPropertyType
{
    /// <summary>
    /// Scalar floating-point value.
    /// </summary>
    Float,
    /// <summary>
    /// Two-component floating-point vector.
    /// </summary>
    Vector2,
    /// <summary>
    /// Three-component floating-point vector.
    /// </summary>
    Vector3,
    /// <summary>
    /// Four-component floating-point vector.
    /// </summary>
    Vector4,
    /// <summary>
    /// Linear RGBA color.
    /// </summary>
    Color,
    /// <summary>
    /// Four-by-four matrix.
    /// </summary>
    Matrix4x4,
    /// <summary>
    /// Two-dimensional texture.
    /// </summary>
    Texture2D,
    /// <summary>
    /// Layered two-dimensional texture.
    /// </summary>
    Texture2DArray,
    /// <summary>
    /// Three-dimensional volume texture.
    /// </summary>
    Texture3D,
    /// <summary>
    /// Cube texture.
    /// </summary>
    TextureCube,
    /// <summary>
    /// Read-only or read-write buffer.
    /// </summary>
    Buffer
}

