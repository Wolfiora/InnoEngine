using System;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Supplies frame timing, capabilities and the request sink to one provider invocation.
/// </summary>
[StableTypeID("67067617-9592-55b2-bbb0-739c6927984b")]
public sealed class RenderRequestProviderContext
{
    /// <summary>
    /// Creates a frame-scoped provider context.
    /// </summary>
    /// <param name="requests">
    /// Sink accepting requests for the current frame.
    /// </param>
    /// <param name="content">
    /// Host-selected content roots visible to request providers this frame.
    /// </param>
    /// <param name="capabilities">
    /// Active backend-neutral capability snapshot.
    /// </param>
    /// <param name="primaryPresentationSize">
    /// Current primary pixel extent, or null while no primary output is available.
    /// </param>
    /// <param name="primaryPresentationViewport">
    /// Host-selected content region, or null when the primary extent is unavailable.
    /// </param>
    /// <param name="frameIndex">
    /// Monotonic render frame index.
    /// </param>
    /// <param name="deltaTime">
    /// Elapsed frame time in seconds.
    /// </param>
    /// <param name="viewContent">
    /// Collector for model-independent world items.
    /// </param>
    /// <param name="input">
    /// Viewport-local input for the primary output.
    /// </param>
    public RenderRequestProviderContext(
        IRenderRequestSink requests,
        ContentReadScope content,
        GraphicsCapabilities capabilities,
        RenderPresentationSize? primaryPresentationSize,
        RenderViewport? primaryPresentationViewport,
        ulong frameIndex,
        float deltaTime,
        IViewContentCollector viewContent,
        RenderOutputInput? input = null
    ) {
        ArgumentNullException.ThrowIfNull(requests);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentNullException.ThrowIfNull(viewContent);
        this.requests = requests;
        this.content = content;
        this.capabilities = capabilities;
        this.primaryPresentationSize = primaryPresentationSize;
        if (primaryPresentationSize.HasValue != primaryPresentationViewport.HasValue)
        {
            throw new ArgumentException("Primary presentation size and viewport must have the same availability.");
        }
        if (primaryPresentationSize is RenderPresentationSize size && primaryPresentationViewport is RenderViewport viewport
            && (!size.isValid || viewport.width <= 0 || viewport.height <= 0
                || (long)viewport.x + viewport.width > size.width
                || (long)viewport.y + viewport.height > size.height)
        ) {
            throw new ArgumentException(
                "The primary presentation viewport must fit inside the primary presentation surface.",
                nameof(primaryPresentationViewport)
            );
        }
        this.primaryPresentationViewport = primaryPresentationViewport;
        this.frameIndex = frameIndex;
        this.deltaTime = deltaTime;
        this.viewContent = viewContent;
        this.input = primaryPresentationSize.HasValue ? input ?? RenderOutputInput.empty : RenderOutputInput.suspended;
    }

    /// <summary>
    /// Gets the sink accepting requests for the current frame.
    /// </summary>
    public IRenderRequestSink requests { get; }

    /// <summary>
    /// Gets the explicit ordered host content visible to request providers this frame.
    /// </summary>
    public ContentReadScope content { get; }

    /// <summary>
    /// Gets the active backend-neutral capability snapshot.
    /// </summary>
    public GraphicsCapabilities capabilities { get; }

    /// <summary>
    /// Gets the current primary pixel extent, or null while only offscreen outputs are available.
    /// </summary>
    public RenderPresentationSize? primaryPresentationSize { get; }

    /// <summary>
    /// Gets the host-selected primary region, or null while no primary output is available.
    /// </summary>
    public RenderViewport? primaryPresentationViewport { get; }

    /// <summary>
    /// Gets the monotonic render frame index.
    /// </summary>
    public ulong frameIndex { get; }

    /// <summary>
    /// Gets the elapsed frame time in seconds.
    /// </summary>
    public float deltaTime { get; }

    /// <summary>
    /// Gets the active generation's world-content collector.
    /// </summary>
    public IViewContentCollector viewContent { get; }

    /// <summary>
    /// Gets viewport-local input for the primary output.
    /// </summary>
    public RenderOutputInput input { get; }
}
