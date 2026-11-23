using System;

namespace Inno.Rendering;

/// <summary>
/// Captures device limits used to validate and compile render graphs.
/// </summary>
public sealed class GraphicsLimits
{
    /// <summary>
    /// Creates a graphics limits snapshot.
    /// </summary>
    /// <param name="maxViews">
    /// Maximum logical backend views in one frame.
    /// </param>
    /// <param name="maxColorAttachments">
    /// Maximum color attachments in one raster pass.
    /// </param>
    /// <param name="maxTextureSize">
    /// Maximum two-dimensional texture extent.
    /// </param>
    /// <param name="maxComputeBindings">
    /// Maximum storage bindings in one compute pass.
    /// </param>
    public GraphicsLimits(
        int maxViews,
        int maxColorAttachments,
        int maxTextureSize,
        int maxComputeBindings
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxViews);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxColorAttachments);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxTextureSize);
        ArgumentOutOfRangeException.ThrowIfNegative(maxComputeBindings);
        this.maxViews = maxViews;
        this.maxColorAttachments = maxColorAttachments;
        this.maxTextureSize = maxTextureSize;
        this.maxComputeBindings = maxComputeBindings;
    }

    /// <summary>
    /// Gets the maximum logical backend views in one frame.
    /// </summary>
    public int maxViews { get; }

    /// <summary>
    /// Gets the maximum color attachments in one raster pass.
    /// </summary>
    public int maxColorAttachments { get; }

    /// <summary>
    /// Gets the maximum two-dimensional texture extent.
    /// </summary>
    public int maxTextureSize { get; }

    /// <summary>
    /// Gets the maximum storage bindings in one compute pass.
    /// </summary>
    public int maxComputeBindings { get; }
}
