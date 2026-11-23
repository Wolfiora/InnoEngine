using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Provides immutable backend-facing metadata for one scheduled pass.
/// </summary>
public sealed class CompiledRenderPass
{
    private readonly IReadOnlyList<CompiledRenderAttachment> m_attachments;
    private readonly Action<RenderPassContext> m_execute;

    internal CompiledRenderPass(
        string name,
        RenderPhaseID phase,
        RenderPassKind kind,
        int viewIndex,
        IReadOnlyList<CompiledRenderAttachment> attachments,
        RenderSurfaceHandle surface,
        bool clearsPresentationTarget,
        RenderClearColor presentationClearColor,
        RenderViewTransform? viewTransform,
        RenderPassRecordingMode recordingMode,
        Action<RenderPassContext> execute
    ) {
        this.name = name;
        this.phase = phase;
        this.kind = kind;
        this.viewIndex = viewIndex;
        m_attachments = Array.AsReadOnly(attachments.ToArray());
        this.surface = surface;
        this.clearsPresentationTarget = clearsPresentationTarget;
        this.presentationClearColor = presentationClearColor;
        this.viewTransform = viewTransform;
        this.recordingMode = recordingMode;
        m_execute = execute;
    }

    /// <summary>
    /// Gets the unique diagnostic name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the open render phase identifier.
    /// </summary>
    public RenderPhaseID phase { get; }

    /// <summary>
    /// Gets the pass command domain.
    /// </summary>
    public RenderPassKind kind { get; }

    /// <summary>
    /// Gets the backend-neutral logical view order.
    /// </summary>
    public int viewIndex { get; }

    /// <summary>
    /// Gets raster attachments in declaration order.
    /// </summary>
    public IReadOnlyList<CompiledRenderAttachment> attachments => m_attachments;

    /// <summary>
    /// Gets the detached presentation surface, or an invalid handle for the primary backbuffer.
    /// </summary>
    public RenderSurfaceHandle surface { get; }

    /// <summary>
    /// Gets whether this pass clears its presentation target before drawing.
    /// </summary>
    public bool clearsPresentationTarget { get; }

    /// <summary>
    /// Gets the linear clear color for a presentation target.
    /// </summary>
    public RenderClearColor presentationClearColor { get; }

    /// <summary>
    /// Gets backend-ready view transforms, or <see langword="null"/> for a matrix-free pass.
    /// </summary>
    public RenderViewTransform? viewTransform { get; }

    /// <summary>
    /// Gets whether callback recording is serial or worker-thread eligible.
    /// </summary>
    public RenderPassRecordingMode recordingMode { get; }

    internal void Execute(RenderPassContext context) => m_execute(context);
}
