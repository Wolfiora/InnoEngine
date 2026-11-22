using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Supplies one content source with the host-selected scene roots and an exact view.
/// </summary>
[StableTypeID("733a6b3a-4cec-5734-83c3-261f0e34b6f8")]
public sealed class ViewContentContext
{
    /// <summary>
    /// Creates a frame-scoped content collection context.
    /// </summary>
    /// <param name="content">
    /// Ordered roots selected by the host.
    /// </param>
    /// <param name="sessionID">
    /// Stable identity of the host output session.
    /// </param>
    /// <param name="view">
    /// Exact view that will consume the content.
    /// </param>
    /// <param name="frameIndex">
    /// Monotonic output frame number.
    /// </param>
    /// <param name="deltaTime">
    /// Elapsed frame time in seconds.
    /// </param>
    /// <param name="input">
    /// Pointer and keyboard input in output pixel coordinates.
    /// </param>
    /// <param name="views">
    /// All views in the same model output, used to choose shared content resolution.
    /// </param>
    /// <param name="sourceIDs">
    /// Optional explicit source allowlist for a routed model layer.
    /// </param>
    public ViewContentContext(
        ContentReadScope content,
        string sessionID,
        RenderView view,
        ulong frameIndex,
        float deltaTime,
        RenderOutputInput? input = null,
        IReadOnlyList<RenderView>? views = null,
        IReadOnlyList<string>? sourceIDs = null
    ) {
        ArgumentNullException.ThrowIfNull(content);
        this.content = content;
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionID);
        this.sessionID = sessionID;
        this.view = view;
        this.frameIndex = frameIndex;
        this.deltaTime = deltaTime;
        this.input = input ?? RenderOutputInput.empty;
        this.views = views ?? [view];
        this.sourceIDs = sourceIDs;
    }

    /// <summary>
    /// Gets the ordered host-selected content roots.
    /// </summary>
    public ContentReadScope content { get; }

    /// <summary>
    /// Gets the stable host output session identity.
    /// </summary>
    public string sessionID { get; }

    /// <summary>
    /// Gets the exact destination view.
    /// </summary>
    public RenderView view { get; }

    /// <summary>
    /// Gets the monotonic output frame number.
    /// </summary>
    public ulong frameIndex { get; }

    /// <summary>
    /// Gets elapsed time in seconds.
    /// </summary>
    public float deltaTime { get; }

    /// <summary>
    /// Gets frame-local input for this output.
    /// </summary>
    public RenderOutputInput input { get; }

    /// <summary>
    /// Gets every view in the same model output.
    /// </summary>
    public IReadOnlyList<RenderView> views { get; }

    /// <summary>
    /// Gets the exclusive source allowlist; null collects every active source.
    /// </summary>
    public IReadOnlyList<string>? sourceIDs { get; }
}
