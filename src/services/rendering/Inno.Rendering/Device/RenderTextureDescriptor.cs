using System;

namespace Inno.Rendering;

/// <summary>
/// Describes a render-graph texture independently from a graphics backend.
/// </summary>
public sealed class RenderTextureDescriptor : IEquatable<RenderTextureDescriptor>
{
    /// <summary>
    /// Creates a texture descriptor.
    /// </summary>
    /// <param name="width">
    /// Texture width in pixels.
    /// </param>
    /// <param name="height">
    /// Texture height in pixels.
    /// </param>
    /// <param name="format">
    /// Storage format.
    /// </param>
    /// <param name="usage">
    /// Permitted operations.
    /// </param>
    /// <param name="mipCount">
    /// Number of mip levels.
    /// </param>
    /// <param name="arrayLayers">
    /// Number of array layers.
    /// </param>
    /// <param name="sampleCount">
    /// Raster sample count.
    /// </param>
    /// <param name="dimension">
    /// Texture dimensional shape.
    /// </param>
    /// <param name="depth">
    /// Base mip depth for a three-dimensional texture.
    /// </param>
    public RenderTextureDescriptor(
        int width,
        int height,
        RenderTextureFormat format,
        RenderTextureUsage usage,
        int mipCount = 1,
        int arrayLayers = 1,
        int sampleCount = 1,
        RenderTextureDimension dimension = RenderTextureDimension.Texture2D,
        int depth = 1
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(mipCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(arrayLayers);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(depth);
        if (sampleCount is not (1 or 2 or 4 or 8 or 16))
        {
            throw new ArgumentOutOfRangeException(nameof(sampleCount), "Sample count must be 1, 2, 4, 8, or 16.");
        }

        switch (dimension)
        {
            case RenderTextureDimension.Texture2D when depth != 1:
                throw new ArgumentException("A two-dimensional texture must have depth one.", nameof(depth));
            case RenderTextureDimension.Texture3D when arrayLayers != 1:
                throw new ArgumentException(
                    "A three-dimensional texture cannot also declare array layers.",
                    nameof(arrayLayers)
                );
            case RenderTextureDimension.Texture3D when sampleCount != 1:
                throw new ArgumentException("A three-dimensional texture cannot be multisampled.", nameof(sampleCount));
            case RenderTextureDimension.Cube when width != height:
                throw new ArgumentException("Cubemap width and height must be equal.", nameof(height));
            case RenderTextureDimension.Cube when depth != 1:
                throw new ArgumentException("A cubemap must have depth one.", nameof(depth));
            case RenderTextureDimension.Cube when sampleCount != 1:
                throw new ArgumentException("A cubemap cannot be multisampled.", nameof(sampleCount));
            case RenderTextureDimension.Texture2D:
            case RenderTextureDimension.Texture3D:
            case RenderTextureDimension.Cube:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dimension));
        }

        this.width = width;
        this.height = height;
        this.format = format;
        this.usage = usage;
        this.mipCount = mipCount;
        this.arrayLayers = arrayLayers;
        this.sampleCount = sampleCount;
        this.dimension = dimension;
        this.depth = depth;
    }

    /// <summary>
    /// Gets the texture width in pixels.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the texture height in pixels.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets the storage format.
    /// </summary>
    public RenderTextureFormat format { get; }

    /// <summary>
    /// Gets permitted operations.
    /// </summary>
    public RenderTextureUsage usage { get; }

    /// <summary>
    /// Gets the number of mip levels.
    /// </summary>
    public int mipCount { get; }

    /// <summary>
    /// Gets the number of array layers.
    /// </summary>
    public int arrayLayers { get; }

    /// <summary>
    /// Gets the raster sample count.
    /// </summary>
    public int sampleCount { get; }

    /// <summary>
    /// Gets the dimensional texture shape.
    /// </summary>
    public RenderTextureDimension dimension { get; }

    /// <summary>
    /// Gets the base mip depth for a three-dimensional texture.
    /// </summary>
    public int depth { get; }

    /// <summary>
    /// Gets the number of independently addressable layers or slices at one mip level.
    /// </summary>
    /// <param name="mipLevel">
    /// Zero-based mip level.
    /// </param>
    /// <returns>
    /// The array-layer count for 2D textures, six faces per cubemap layer, or the mip depth for 3D textures.
    /// </returns>
    public int GetSubresourceLayerCount(int mipLevel)
    {
        if ((uint)mipLevel >= (uint)mipCount)
        {
            throw new ArgumentOutOfRangeException(nameof(mipLevel));
        }
        return dimension switch
        {
            RenderTextureDimension.Texture2D => arrayLayers,
            RenderTextureDimension.Texture3D => Math.Max(1, depth >> mipLevel),
            RenderTextureDimension.Cube => checked(arrayLayers * 6),
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };
    }

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <param name="other">
    /// The value to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RenderTextureDescriptor? other)
        => other is not null
           && width == other.width
           && height == other.height
           && format == other.format
           && usage == other.usage
           && mipCount == other.mipCount
           && arrayLayers == other.arrayLayers
           && sampleCount == other.sampleCount
           && dimension == other.dimension
           && depth == other.depth;

    /// <summary>
    /// Determines whether this instance and the supplied value represent the same logical state.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with this instance.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when both values represent the same logical state; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) => Equals(obj as RenderTextureDescriptor);

    /// <summary>
    /// Computes a hash code from the fields that participate in logical equality.
    /// </summary>
    /// <returns>
    /// A hash code consistent with the implemented equality contract.
    /// </returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(width);
        hash.Add(height);
        hash.Add(format);
        hash.Add(usage);
        hash.Add(mipCount);
        hash.Add(arrayLayers);
        hash.Add(sampleCount);
        hash.Add(dimension);
        hash.Add(depth);
        return hash.ToHashCode();
    }
}
