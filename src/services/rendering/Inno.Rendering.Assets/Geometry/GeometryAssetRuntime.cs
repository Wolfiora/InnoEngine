using Inno.Core.Mathematics;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inno.Rendering.Assets;

/// <summary>
/// Decodes normalized CPU geometry committed by mesh importers.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("81298da5-7d85-5964-8cb9-6f70a1e0564f")]
public static class GeometryAssetRuntime
{
    /// <summary>
    /// Decodes the current committed mesh payload.
    /// </summary>
    /// <param name="geometry">
    /// Imported geometry asset.
    /// </param>
    /// <returns>
    /// Normalized vertices, triangle indices and submeshes.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when the payload is missing or corrupt.
    /// </exception>
    public static GeometryData GetGeometryData(GeometryAsset geometry)
    {
        ArgumentNullException.ThrowIfNull(geometry);
        return GeometryArtifact.Decode(geometry.runtimePayload.Span);
    }
}

