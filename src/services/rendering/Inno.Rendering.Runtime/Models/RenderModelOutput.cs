using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Gets one model's prepared pipeline data for a host-owned target.
/// </summary>
[StableTypeID("3954390f-0d08-5114-82e4-5e35aa777472")]
public sealed class RenderModelOutput
{
    /// <summary>
    /// Creates prepared model output.
    /// </summary>
    /// <param name="name">
    /// Frame-local diagnostic name.
    /// </param>
    /// <param name="pipeline">
    /// Exact model pipeline.
    /// </param>
    /// <param name="data">
    /// Immutable model frame data.
    /// </param>
    /// <param name="targetFormat">
    /// Required color target format.
    /// </param>
    public RenderModelOutput(
        string name,
        RenderPipelineAsset pipeline,
        RenderFrameData data,
        RenderTextureFormat targetFormat = RenderTextureFormat.RGBA8Srgb
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(pipeline);
        this.name = name;
        this.pipeline = pipeline;
        this.data = data?.Snapshot() ?? throw new ArgumentNullException(nameof(data));
        this.targetFormat = targetFormat;
    }

    /// <summary>
    /// Gets the diagnostic name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the model's pipeline.
    /// </summary>
    public RenderPipelineAsset pipeline { get; }

    /// <summary>
    /// Gets model frame data.
    /// </summary>
    public RenderFrameData data { get; }

    /// <summary>
    /// Gets required target format.
    /// </summary>
    public RenderTextureFormat targetFormat { get; }
}
