using System;
using Inno.Rendering.Assets;
using Inno.Rendering.Runtime;

namespace Inno.Editor.Rendering;

/// <summary>
/// Describes one model-neutral layer in an Editor viewport composition.
/// </summary>
public sealed class EditorViewportLayer
{
    /// <summary>
    /// Creates one immutable model-neutral viewport layer.
    /// </summary>
    /// <param name="contributorID">
    /// Stable identity of the rendering-model contributor that produced the layer.
    /// </param>
    /// <param name="pipeline">
    /// Contributor-selected pipeline, or null for the project default.
    /// </param>
    /// <param name="data">
    /// Contributor-defined frame-only data.
    /// </param>
    /// <param name="order">
    /// Ascending model-composition order.
    /// </param>
    public EditorViewportLayer(
        string contributorID,
        RenderPipelineAsset? pipeline,
        RenderFrameData data,
        int order
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(contributorID);
        ArgumentNullException.ThrowIfNull(data);
        this.contributorID = contributorID;
        this.pipeline = pipeline;
        this.data = data;
        this.order = order;
    }

    /// <summary>
    /// Gets the stable identity of the rendering-model contributor.
    /// </summary>
    public string contributorID { get; }

    /// <summary>
    /// Gets the contributor-selected pipeline, or null for the project default.
    /// </summary>
    public RenderPipelineAsset? pipeline { get; }

    /// <summary>
    /// Gets contributor-defined frame-only data.
    /// </summary>
    public RenderFrameData data { get; }

    /// <summary>
    /// Gets ascending model-composition order.
    /// </summary>
    public int order { get; }
}
