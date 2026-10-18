using Inno.Core.Mathematics;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores one normalized vertex shared by all rendering backends.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("cfce5c30-dee4-5d62-9aa3-3e32ea5e7355")]
public readonly record struct GeometryVertex
{
    /// <summary>
    /// Creates a normalized mesh vertex.
    /// </summary>
    /// <param name="position">
    /// Object-space position.
    /// </param>
    /// <param name="normal">
    /// Object-space unit normal.
    /// </param>
    /// <param name="tangent">
    /// Object-space tangent and handedness.
    /// </param>
    /// <param name="textureCoordinate">
    /// Primary texture coordinate.
    /// </param>
    public GeometryVertex(
        Vector3 position,
        Vector3 normal,
        Vector4 tangent,
        Vector2 textureCoordinate
    ) {
        this.position = position;
        this.normal = normal;
        this.tangent = tangent;
        this.textureCoordinate = textureCoordinate;
    }

    /// <summary>
    /// Gets the object-space position.
    /// </summary>
    public Vector3 position { get; }

    /// <summary>
    /// Gets the object-space unit normal.
    /// </summary>
    public Vector3 normal { get; }

    /// <summary>
    /// Gets the object-space tangent and handedness.
    /// </summary>
    public Vector4 tangent { get; }

    /// <summary>
    /// Gets the primary texture coordinate.
    /// </summary>
    public Vector2 textureCoordinate { get; }
}

