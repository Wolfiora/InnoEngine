using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Requests one pipeline-defined rendering operation without prescribing world semantics.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("b553fd8c-9410-53a7-8d8f-276e2a302e66")]
public sealed class RenderRequest
{
    /// <summary>
    /// Creates an immutable render request.
    /// </summary>
    /// <param name="name">
    /// Frame-local diagnostic name.
    /// </param>
    /// <param name="target">
    /// Render destination.
    /// </param>
    /// <param name="viewport">
    /// Destination pixel viewport.
    /// </param>
    /// <param name="pipeline">
    /// Optional per-request pipeline asset; the project default is used when null.
    /// </param>
    /// <param name="data">
    /// Optional pipeline-defined frame data copied into an immutable snapshot.
    /// </param>
    /// <param name="priority">
    /// Ascending frame scheduling priority.
    /// </param>
    public RenderRequest(
        string name,
        RenderTarget target,
        RenderViewport viewport,
        RenderPipelineAsset? pipeline = null,
        RenderFrameData? data = null,
        int priority = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        this.name = name;
        this.target = target;
        this.viewport = viewport;
        this.pipeline = pipeline;
        this.data = data?.Snapshot() ?? new RenderFrameData().Snapshot();
        this.priority = priority;
    }

    /// <summary>
    /// Gets the frame-local diagnostic name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the render destination.
    /// </summary>
    public RenderTarget target { get; }

    /// <summary>
    /// Gets the destination pixel viewport.
    /// </summary>
    public RenderViewport viewport { get; }

    /// <summary>
    /// Gets the per-request pipeline asset, or null to use the project default.
    /// </summary>
    public RenderPipelineAsset? pipeline { get; }

    /// <summary>
    /// Gets immutable pipeline-defined frame data.
    /// </summary>
    public RenderFrameData data { get; }

    /// <summary>
    /// Gets the ascending frame scheduling priority.
    /// </summary>
    public int priority { get; }
}

