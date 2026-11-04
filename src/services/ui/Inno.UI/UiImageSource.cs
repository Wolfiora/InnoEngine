using System;
using Inno.Rendering.Assets;

namespace Inno.UI;

/// <summary>
/// Describes an imported image without decoding or owning its GPU texture.
/// </summary>
public sealed class UiImageSource
{
    /// <summary>
    /// Captures an immutable texture artifact and its imported pixel dimensions.
    /// </summary>
    /// <param name="artifact">
    /// Validated imported texture slot and content identity.
    /// </param>
    /// <param name="width">
    /// Positive imported pixel width.
    /// </param>
    /// <param name="height">
    /// Positive imported pixel height.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The artifact is unassigned.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A dimension is not positive.
    /// </exception>
    public UiImageSource(
        RenderTextureArtifactReference artifact,
        int width,
        int height
    ) {
        if (artifact.assetId == Guid.Empty || string.IsNullOrWhiteSpace(artifact.slot.id)
            || string.IsNullOrWhiteSpace(artifact.slot.sourceOutputName))
            throw new ArgumentException("A UI image requires an assigned texture artifact.", nameof(artifact));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        this.artifact = artifact;
        this.width = width;
        this.height = height;
    }

    /// <summary>
    /// Gets the frozen imported texture identity; Rendering owns resolution and residency.
    /// </summary>
    public RenderTextureArtifactReference artifact { get; }

    /// <summary>
    /// Gets the imported pixel width used by layout.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the imported pixel height used by layout.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Captures a loaded texture without retaining the asset object.
    /// </summary>
    /// <param name="texture">
    /// Loaded texture in the caller's current asset generation.
    /// </param>
    /// <returns>
    /// An immutable source that remains independent of subsequent asset mutation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The asset is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The asset is Missing or has no imported dimensions.
    /// </exception>
    public static UiImageSource FromTexture(TextureAsset texture)
    {
        ArgumentNullException.ThrowIfNull(texture);
        if (texture.isMissing || texture.width <= 0 || texture.height <= 0)
            throw new InvalidOperationException("A UI image requires a loaded, non-Missing texture.");
        return new(texture.GetTextureArtifactReference(), texture.width, texture.height);
    }
}
