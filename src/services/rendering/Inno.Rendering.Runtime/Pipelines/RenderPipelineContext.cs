using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Supplies one request and frame-scoped services to a render pipeline.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("fe6c1616-212d-521e-b4ab-9e61a72b9c45")]
public sealed class RenderPipelineContext
{
    /// <summary>
    /// Creates a pipeline build context.
    /// </summary>
    /// <param name="request">
    /// Pipeline-defined render request.
    /// </param>
    /// <param name="pipelineAsset">
    /// Selected persistent pipeline configuration.
    /// </param>
    /// <param name="graph">
    /// Current frame graph builder.
    /// </param>
    /// <param name="capabilities">
    /// Current device capability snapshot.
    /// </param>
    /// <param name="resources">
    /// Open semantic resource registry.
    /// </param>
    /// <param name="diagnostics">
    /// Structured diagnostic sink.
    /// </param>
    /// <param name="resourceService">
    /// Generation-aware shader, material and persistent GPU resource service.
    /// </param>
    /// <param name="uploads">
    /// Frame-scoped streaming buffer service.
    /// </param>
    /// <param name="frameIndex">
    /// Monotonic render frame index.
    /// </param>
    /// <param name="preservePresentationTarget">
    /// Whether an earlier successful model contribution already initialized the same presentation target.
    /// </param>
    /// <param name="outputTexture">
    /// Imported offscreen target, or an invalid handle for the backbuffer.
    /// </param>
    public RenderPipelineContext(
        RenderRequest request,
        RenderPipelineAsset pipelineAsset,
        RenderGraphBuilder graph,
        GraphicsCapabilities capabilities,
        RenderResourceMap resources,
        IDiagnosticReporter diagnostics,
        IRenderResourceService resourceService,
        IRenderFrameUploadService uploads,
        ulong frameIndex,
        bool preservePresentationTarget,
        RenderTextureHandle outputTexture = default
    ) {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(pipelineAsset);
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentNullException.ThrowIfNull(resources);
        ArgumentNullException.ThrowIfNull(diagnostics);
        ArgumentNullException.ThrowIfNull(resourceService);
        ArgumentNullException.ThrowIfNull(uploads);
        this.request = request;
        this.pipelineAsset = pipelineAsset;
        this.graph = graph;
        this.capabilities = capabilities;
        this.resources = resources;
        this.diagnostics = diagnostics;
        this.resourceService = resourceService;
        this.uploads = uploads;
        this.frameIndex = frameIndex;
        this.preservePresentationTarget = preservePresentationTarget;
        this.outputTexture = outputTexture;
    }

    /// <summary>
    /// Gets the current request.
    /// </summary>
    public RenderRequest request { get; }

    /// <summary>
    /// Gets selected pipeline configuration.
    /// </summary>
    public RenderPipelineAsset pipelineAsset { get; }

    /// <summary>
    /// Gets the current frame graph builder.
    /// </summary>
    public RenderGraphBuilder graph { get; }

    /// <summary>
    /// Gets current device capabilities.
    /// </summary>
    public GraphicsCapabilities capabilities { get; }

    /// <summary>
    /// Gets open semantic resources for this request.
    /// </summary>
    public RenderResourceMap resources { get; }

    /// <summary>
    /// Gets the structured diagnostic sink.
    /// </summary>
    public IDiagnosticReporter diagnostics { get; }

    /// <summary>
    /// Gets the generation-aware neutral GPU resource service.
    /// </summary>
    public IRenderResourceService resourceService { get; }

    /// <summary>
    /// Gets the frame-scoped streaming buffer service.
    /// </summary>
    public IRenderFrameUploadService uploads { get; }

    /// <summary>
    /// Gets the monotonic render frame index.
    /// </summary>
    public ulong frameIndex { get; }

    /// <summary>
    /// Gets whether this model contribution must load and preserve an earlier contribution to the same target.
    /// </summary>
    /// <remarks>
    /// A pipeline must not clear its presentation color target when this value is <see langword="true"/>.
    /// </remarks>
    public bool preservePresentationTarget { get; }

    /// <summary>
    /// Gets the imported offscreen target, or an invalid handle for the backbuffer.
    /// </summary>
    public RenderTextureHandle outputTexture { get; }
}

