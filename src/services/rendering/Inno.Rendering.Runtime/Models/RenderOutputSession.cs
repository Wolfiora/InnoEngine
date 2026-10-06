using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Declares a host output without naming a camera or scene model.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("18d40f92-089a-5c0e-94a9-402b68225c2b")]
public sealed class RenderOutputSession
{
    /// <summary>
    /// Creates a frame-scoped output session.
    /// </summary>
    /// <param name="id">
    /// Stable host output identity.
    /// </param>
    /// <param name="content">
    /// Host-selected content roots.
    /// </param>
    /// <param name="viewport">
    /// Pixel rectangle within the target.
    /// </param>
    /// <param name="frameIndex">
    /// Shared output frame index.
    /// </param>
    /// <param name="deltaTime">
    /// Elapsed frame time in seconds.
    /// </param>
    /// <param name="viewContent">
    /// Model-independent world content collector.
    /// </param>
    /// <param name="input">
    /// Viewport-local input.
    /// </param>
    /// <param name="route">
    /// Explicit composition route when several models are applicable.
    /// </param>
    public RenderOutputSession(
        string id,
        ContentReadScope content,
        RenderViewport viewport,
        ulong frameIndex,
        float deltaTime,
        IViewContentCollector viewContent,
        RenderOutputInput? input = null,
        RenderOutputRoute? route = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
        this.content = content ?? throw new ArgumentNullException(nameof(content));
        this.viewport = viewport;
        this.frameIndex = frameIndex;
        this.deltaTime = deltaTime;
        this.viewContent = viewContent ?? throw new ArgumentNullException(nameof(viewContent));
        this.input = input ?? RenderOutputInput.empty;
        this.route = route;
    }

    /// <summary>
    /// Gets the stable host output identity.
    /// </summary>
    public string id { get; }
    /// <summary>
    /// Gets selected content roots.
    /// </summary>
    public ContentReadScope content { get; }
    /// <summary>
    /// Gets the output pixel viewport.
    /// </summary>
    public RenderViewport viewport { get; }
    /// <summary>
    /// Gets the shared output frame index.
    /// </summary>
    public ulong frameIndex { get; }
    /// <summary>
    /// Gets elapsed frame time.
    /// </summary>
    public float deltaTime { get; }
    /// <summary>
    /// Gets the world content collector.
    /// </summary>
    public IViewContentCollector viewContent { get; }
    /// <summary>
    /// Gets viewport-local input.
    /// </summary>
    public RenderOutputInput input { get; }
    /// <summary>
    /// Gets the explicit model route, if configured.
    /// </summary>
    public RenderOutputRoute? route { get; }

    /// <summary>
    /// Creates the session seen by one explicitly assigned model layer.
    /// </summary>
    /// <param name="layer">
    /// The model and content-source assignment.
    /// </param>
    /// <returns>
    /// A session with only the assigned world-content sources.
    /// </returns>
    public RenderOutputSession ForLayer(RenderOutputLayer layer)
    {
        ArgumentNullException.ThrowIfNull(layer);
        return new RenderOutputSession(id, content, viewport, frameIndex, deltaTime,
            new SelectedViewContentCollector(viewContent, layer.sourceIds), input);
    }
}

