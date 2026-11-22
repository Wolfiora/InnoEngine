using System;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Runtime;

namespace Inno.Editor.Rendering;

/// <summary>
/// Returns one rendering-model contribution without exposing a GPU backend.
/// </summary>
public sealed class EditorViewportContribution
{
    /// <summary>
    /// Creates a viewport model contribution.
    /// </summary>
    /// <param name="data">
    /// Pipeline-defined frame-only data.
    /// </param>
    /// <param name="pipeline">
    /// Contributor-selected pipeline, or null for the project default.
    /// </param>
    /// <param name="targetFormat">
    /// Presentation target format expected by the pipeline.
    /// </param>
    /// <param name="manipulationSpace">
    /// Optional exact view/projection contract for host-owned transform manipulation tools.
    /// </param>
    public EditorViewportContribution(
        RenderFrameData data,
        RenderPipelineAsset? pipeline = null,
        RenderTextureFormat targetFormat = RenderTextureFormat.RGBA8Srgb,
        EditorViewportManipulationSpace? manipulationSpace = null
    ) {
        ArgumentNullException.ThrowIfNull(data);
        this.data = data;
        this.pipeline = pipeline;
        this.targetFormat = targetFormat;
        this.manipulationSpace = manipulationSpace;
    }

    /// <summary>
    /// Gets pipeline-defined frame-only data.
    /// </summary>
    public RenderFrameData data { get; }

    /// <summary>
    /// Gets the selected pipeline, or null for the project default.
    /// </summary>
    public RenderPipelineAsset? pipeline { get; }

    /// <summary>
    /// Gets the presentation target format expected by the pipeline.
    /// </summary>
    public RenderTextureFormat targetFormat { get; }

    /// <summary>
    /// Gets the optional exact view/projection contract used by host-owned transform manipulation tools.
    /// </summary>
    public EditorViewportManipulationSpace? manipulationSpace { get; }
}
