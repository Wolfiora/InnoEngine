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
/// Represents imported texture content without owning a GPU handle.
/// </summary>
[StableTypeId("e174b6eb-f79a-470f-a460-84f88ab49d0e")]
public sealed class TextureAsset : AssetObject, IRenderTextureArtifactSource
{
    private static readonly RenderTextureArtifactSlot[] S_linearTextureArtifacts =
        [new RenderTextureArtifactSlot("main", "source", TextureColorSpace.Linear)];
    private static readonly RenderTextureArtifactSlot[] S_sRgbTextureArtifacts =
        [new RenderTextureArtifactSlot("main", "source", TextureColorSpace.Srgb)];

    internal TextureAsset()
    {
    }

    /// <summary>
    /// Creates an immutable imported texture description.
    /// </summary>
    /// <param name="width">
    /// The positive source pixel width.
    /// </param>
    /// <param name="height">
    /// The positive source pixel height.
    /// </param>
    /// <param name="colorSpace">
    /// The color-space interpretation used when sampling the texture.
    /// </param>
    /// <param name="sourceFormat">
    /// The normalized source container name without a leading period.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="width"/> or <paramref name="height"/> is not positive.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="sourceFormat"/> is empty or contains only white-space characters.
    /// </exception>
    public TextureAsset(
        int width,
        int height,
        TextureColorSpace colorSpace,
        string sourceFormat
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceFormat);
        this.width = width;
        this.height = height;
        this.colorSpace = colorSpace;
        this.sourceFormat = sourceFormat;
    }

    /// <summary>
    /// Gets the source pixel width.
    /// </summary>
    [SerializableProperty]
    public int width { get; internal set; }

    /// <summary>
    /// Gets the source pixel height.
    /// </summary>
    [SerializableProperty]
    public int height { get; internal set; }

    /// <summary>
    /// Gets the declared sample color space.
    /// </summary>
    [SerializableProperty]
    public TextureColorSpace colorSpace { get; internal set; }

    /// <summary>
    /// Gets the normalized source container name.
    /// </summary>
    [SerializableProperty]
    public string sourceFormat { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the single source artifact compiled for this imported texture.
    /// </summary>
    public IReadOnlyList<RenderTextureArtifactSlot> textureArtifacts
        => colorSpace == TextureColorSpace.Srgb
            ? S_sRgbTextureArtifacts
            : S_linearTextureArtifacts;

    /// <summary>
    /// Creates a stable reference to this texture's current imported content.
    /// </summary>
    /// <returns>
    /// A reference suitable for target compilation and generation-scoped GPU resolution.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when this texture has not been assigned a persistent asset identity.
    /// </exception>
    public RenderTextureArtifactReference GetTextureArtifactReference()
    {
        Guid id = identity.persistentId;
        if (id == Guid.Empty)
            throw new InvalidOperationException("Texture must have a persistent asset identity.");
        return new RenderTextureArtifactReference(id, contentVersion, textureArtifacts[0]);
    }
}

