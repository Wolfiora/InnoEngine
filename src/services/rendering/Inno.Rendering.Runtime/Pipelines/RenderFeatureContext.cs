using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Supplies one configured feature with frame-scoped graph services.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("00810cb8-162a-5a95-9a30-1a01992ac501")]
public sealed class RenderFeatureContext
{
    /// <summary>
    /// Creates a feature build context.
    /// </summary>
    /// <param name="pipeline">
    /// Owning pipeline context.
    /// </param>
    /// <param name="configuration">
    /// Stable feature configuration.
    /// </param>
    public RenderFeatureContext(
        RenderPipelineContext pipeline,
        RenderFeatureConfiguration configuration
    ) {
        this.pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        this.configuration = configuration;
    }

    /// <summary>
    /// Gets the owning pipeline context.
    /// </summary>
    public RenderPipelineContext pipeline { get; }

    /// <summary>
    /// Gets stable feature configuration.
    /// </summary>
    public RenderFeatureConfiguration configuration { get; }

    /// <summary>
    /// Gets the current frame graph builder.
    /// </summary>
    public RenderGraphBuilder graph => pipeline.graph;

    /// <summary>
    /// Gets open semantic resources for the request.
    /// </summary>
    public RenderResourceMap resources => pipeline.resources;

    /// <summary>
    /// Gets current device capabilities.
    /// </summary>
    public GraphicsCapabilities capabilities => pipeline.capabilities;

    /// <summary>
    /// Gets the generation-aware neutral GPU resource service.
    /// </summary>
    public IRenderResourceService resourceService => pipeline.resourceService;

    /// <summary>
    /// Gets the frame-scoped streaming buffer service.
    /// </summary>
    public IRenderFrameUploadService uploads => pipeline.uploads;
}

