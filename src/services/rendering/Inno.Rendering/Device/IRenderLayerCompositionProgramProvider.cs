namespace Inno.Rendering;

/// <summary>
/// Supplies a backend-specific graphics program for compositing ordered render layers.
/// </summary>
public interface IRenderLayerCompositionProgramProvider
{
    /// <summary>
    /// Creates a pipeline descriptor compatible with the supplied fullscreen vertex layout.
    /// </summary>
    /// <param name="capabilities">
    /// Capabilities of the device that will own the pipeline.
    /// </param>
    /// <param name="vertexLayout">
    /// Layout of the fullscreen quad submitted by the runtime.
    /// </param>
    /// <returns>
    /// A pipeline descriptor that samples a texture named <c>s_tex</c> with premultiplied alpha.
    /// </returns>
    GraphicsPipelineDescriptor CreateDescriptor(
        GraphicsCapabilities capabilities,
        RenderVertexLayout vertexLayout
    );

    /// <summary>
    /// Creates the final transfer pipeline for a presentation target without automatic sRGB encoding.
    /// </summary>
    /// <param name="capabilities">
    /// Capabilities of the device that will own the pipeline.
    /// </param>
    /// <param name="vertexLayout">
    /// Layout of the fullscreen quad submitted by the runtime.
    /// </param>
    /// <returns>
    /// An opaque pipeline descriptor that encodes premultiplied linear color to sRGB.
    /// </returns>
    GraphicsPipelineDescriptor CreateOutputTransferDescriptor(
        GraphicsCapabilities capabilities,
        RenderVertexLayout vertexLayout
    );
}
