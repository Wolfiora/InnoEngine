using System;
using Inno.Core.Mathematics;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Describes one backend-neutral view produced by a rendering model.
/// </summary>
[StableTypeID("eb2ef876-46a8-5155-8c45-c96b14ad2043")]
public readonly record struct RenderView
{
    /// <summary>
    /// Creates the exact view used to render content into a viewport.
    /// </summary>
    /// <param name="id">
    /// Stable identity within the current output session.
    /// </param>
    /// <param name="viewport">
    /// Destination pixel rectangle.
    /// </param>
    /// <param name="viewMatrix">
    /// World-to-view transform.
    /// </param>
    /// <param name="projectionMatrix">
    /// View-to-clip transform.
    /// </param>
    /// <param name="visibilityMask">
    /// Model-defined visible content bits.
    /// </param>
    public RenderView(
        string id,
        RenderViewport viewport,
        Matrix viewMatrix,
        Matrix projectionMatrix,
        ulong visibilityMask = ulong.MaxValue
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
        this.viewport = viewport;
        this.viewMatrix = viewMatrix;
        this.projectionMatrix = projectionMatrix;
        this.visibilityMask = visibilityMask;
    }

    /// <summary>
    /// Gets the view identity within its output session.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the destination pixel rectangle.
    /// </summary>
    public RenderViewport viewport { get; }

    /// <summary>
    /// Gets the world-to-view transform.
    /// </summary>
    public Matrix viewMatrix { get; }

    /// <summary>
    /// Gets the view-to-clip transform.
    /// </summary>
    public Matrix projectionMatrix { get; }

    /// <summary>
    /// Gets the model-defined visible content bits.
    /// </summary>
    public ulong visibilityMask { get; }
}
