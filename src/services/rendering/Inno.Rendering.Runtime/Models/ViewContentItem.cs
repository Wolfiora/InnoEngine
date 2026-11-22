using System;
using Inno.Core.Identity;
using Inno.Core.Mathematics;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// One world item supplied to a rendering model without choosing its sort policy.
/// </summary>
[StableTypeID("4aecd31e-d107-56b4-bc92-e07bc89c125b")]
public sealed class ViewContentItem
{
    /// <summary>
    /// Creates an item whose lifetime is limited to the current frame.
    /// </summary>
    /// <param name="owner">
    /// Scene object identity used for model-owned sorting and visibility.
    /// </param>
    /// <param name="localToWorld">
    /// Exact transform of its local geometry.
    /// </param>
    /// <param name="localBoundsMin">
    /// Local minimum corner for conservative culling.
    /// </param>
    /// <param name="localBoundsMax">
    /// Local maximum corner for conservative culling.
    /// </param>
    /// <param name="drawable">
    /// Drawable inserted into the model's scene pass.
    /// </param>
    /// <param name="pointerTarget">
    /// Optional input target routed by the rendering model.
    /// </param>
    public ViewContentItem(
        Identity owner,
        Matrix localToWorld,
        Vector3 localBoundsMin,
        Vector3 localBoundsMax,
        IViewDrawable drawable,
        IViewPointerTarget? pointerTarget = null
    ) {
        ArgumentNullException.ThrowIfNull(drawable);
        this.owner = owner;
        this.localToWorld = localToWorld;
        this.localBoundsMin = localBoundsMin;
        this.localBoundsMax = localBoundsMax;
        this.drawable = drawable;
        this.pointerTarget = pointerTarget;
    }

    /// <summary>
    /// Gets the owner identity for model-owned sorting and visibility.
    /// </summary>
    public Identity owner { get; }

    /// <summary>
    /// Gets the local-to-world transform.
    /// </summary>
    public Matrix localToWorld { get; }

    /// <summary>
    /// Gets the local minimum corner.
    /// </summary>
    public Vector3 localBoundsMin { get; }

    /// <summary>
    /// Gets the local maximum corner.
    /// </summary>
    public Vector3 localBoundsMax { get; }

    /// <summary>
    /// Gets the drawable encoded by the selected rendering model.
    /// </summary>
    public IViewDrawable drawable { get; }

    /// <summary>
    /// Gets the optional pointer target associated with this draw item.
    /// </summary>
    public IViewPointerTarget? pointerTarget { get; }
}
