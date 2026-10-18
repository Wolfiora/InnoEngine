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
/// Represents imported geometry without prescribing scene or draw semantics.
/// </summary>
[StableTypeId("f214637e-0a54-438d-8a72-9d892bd29a56")]
public sealed class GeometryAsset : AssetObject
{
    internal GeometryAsset()
    {
    }

    /// <summary>
    /// Creates an immutable imported geometry description.
    /// </summary>
    /// <param name="vertexCount">
    /// The number of normalized vertices.
    /// </param>
    /// <param name="indexCount">
    /// The number of indices.
    /// </param>
    /// <param name="sectionCount">
    /// The number of independently submitted sections.
    /// </param>
    /// <param name="boundsCenter">
    /// The object-space center of the geometry bounds.
    /// </param>
    /// <param name="boundsExtents">
    /// The non-negative object-space half-extents of the geometry bounds.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any count is negative or a bounds extent is negative.
    /// </exception>
    public GeometryAsset(
        int vertexCount,
        int indexCount,
        int sectionCount,
        Vector3 boundsCenter,
        Vector3 boundsExtents
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(vertexCount);
        ArgumentOutOfRangeException.ThrowIfNegative(indexCount);
        ArgumentOutOfRangeException.ThrowIfNegative(sectionCount);
        if (boundsExtents.x < 0f || boundsExtents.y < 0f || boundsExtents.z < 0f)
            throw new ArgumentOutOfRangeException(nameof(boundsExtents), "Geometry bounds extents cannot be negative.");
        this.vertexCount = vertexCount;
        this.indexCount = indexCount;
        this.sectionCount = sectionCount;
        this.boundsCenter = boundsCenter;
        this.boundsExtents = boundsExtents;
    }

    /// <summary>
    /// Gets the number of normalized vertices.
    /// </summary>
    [SerializableProperty]
    public int vertexCount { get; internal set; }

    /// <summary>
    /// Gets the number of indices.
    /// </summary>
    [SerializableProperty]
    public int indexCount { get; internal set; }

    /// <summary>
    /// Gets the number of independently submitted geometry sections.
    /// </summary>
    [SerializableProperty]
    public int sectionCount { get; internal set; }

    /// <summary>
    /// Gets the object-space center of imported geometry bounds.
    /// </summary>
    [SerializableProperty]
    public Vector3 boundsCenter { get; internal set; }

    /// <summary>
    /// Gets the non-negative object-space half-extents of imported geometry bounds.
    /// </summary>
    [SerializableProperty]
    public Vector3 boundsExtents { get; internal set; }
}

