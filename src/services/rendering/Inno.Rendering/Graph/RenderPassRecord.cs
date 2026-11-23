using System;
using System.Collections.Generic;

namespace Inno.Rendering;

internal sealed class RenderPassRecord
{
    /// <summary>
    /// Gets the human-readable name used for presentation and diagnostics.
    /// </summary>
    public required string name { get; init; }

    /// <summary>
    /// Gets the render phase that owns this pass declaration.
    /// </summary>
    public required RenderPhaseID phase { get; init; }

    /// <summary>
    /// Gets the operation kind that determines how this value is interpreted.
    /// </summary>
    public required RenderPassKind kind { get; init; }

    /// <summary>
    /// Gets the callback that records commands for this render pass.
    /// </summary>
    public required Action<RenderPassContext> execute { get; init; }

    /// <summary>
    /// Gets the render phases that must execute after this pass.
    /// </summary>
    public List<RenderPhaseID> before { get; } = [];

    /// <summary>
    /// Gets the render phases that must execute before this pass.
    /// </summary>
    public List<RenderPhaseID> after { get; } = [];

    /// <summary>
    /// Gets the complete resource access declarations used for graph dependency analysis.
    /// </summary>
    public List<RenderResourceUse> resources { get; } = [];

    /// <summary>
    /// Gets the color and depth attachments written by this render pass.
    /// </summary>
    public List<RenderAttachment> attachments { get; } = [];

    /// <summary>
    /// Gets the presentation surface targeted by this render pass.
    /// </summary>
    public RenderSurfaceHandle surface { get; set; }

    /// <summary>
    /// Gets the color used when the presentation surface is cleared.
    /// </summary>
    public RenderClearColor presentationClearColor { get; set; }

    /// <summary>
    /// Gets the optional view and projection transform applied by this pass.
    /// </summary>
    public RenderViewTransform? viewTransform { get; set; }

    /// <summary>
    /// Gets whether the caller-visible condition represented by this property is satisfied.
    /// </summary>
    public bool clearsPresentationTarget { get; set; }

    /// <summary>
    /// Gets whether this value has side effect.
    /// </summary>
    public bool hasSideEffect { get; set; }

    /// <summary>
    /// Gets whether the pass records graphics, compute, or transfer commands.
    /// </summary>
    public RenderPassRecordingMode recordingMode { get; set; }

    internal required int index { get; init; }
}

/// <summary>
/// Controls where a frame-local pass callback records backend-neutral commands.
/// </summary>
public enum RenderPassRecordingMode
{
    /// <summary>
    /// Records directly on the graph execution thread.
    /// </summary>
    Serial,
    /// <summary>
    /// Records into an isolated command list on a worker thread, then replays in graph order.
    /// </summary>
    Parallel
}
