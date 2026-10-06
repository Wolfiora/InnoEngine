using Inno.Rendering;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Assets;

/// <summary>
/// Declares an immutable source artifact that is compiled into one sampled texture slot.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("e1b8347a-2b49-5793-b693-f32aba02030e")]
public readonly record struct RenderTextureArtifactSlot
{
    /// <summary>
    /// Creates one stable texture slot declaration.
    /// </summary>
    /// <param name="id">
    /// Asset-local stable slot identifier.
    /// </param>
    /// <param name="sourceOutputName">
    /// Named source artifact written by the owning asset importer.
    /// </param>
    /// <param name="colorSpace">
    /// Sampling color-space interpretation used by the target compiler and GPU resource.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> or <paramref name="sourceOutputName"/> is empty.
    /// </exception>
    public RenderTextureArtifactSlot(
        string id,
        string sourceOutputName,
        TextureColorSpace colorSpace
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceOutputName);
        this.id = id;
        this.sourceOutputName = sourceOutputName;
        this.colorSpace = colorSpace;
    }

    /// <summary>
    /// Gets the asset-local stable slot identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the named portable image artifact consumed by target compilation.
    /// </summary>
    public string sourceOutputName { get; }

    /// <summary>
    /// Gets the sampling color-space interpretation.
    /// </summary>
    public TextureColorSpace colorSpace { get; }
}

