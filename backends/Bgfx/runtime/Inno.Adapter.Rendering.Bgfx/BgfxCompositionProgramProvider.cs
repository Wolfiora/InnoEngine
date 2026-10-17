using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Supplies the BGFX shader program used to composite model output layers.
/// </summary>
public sealed class BgfxCompositionProgramProvider : IRenderLayerCompositionProgramProvider
{
    private static readonly RenderBindingId S_TEXTURE = new("s_tex");

    /// <summary>
    /// Loads the BGFX target program matching the active device renderer.
    /// </summary>
    /// <param name="capabilities">
    /// The device capabilities whose backend identifies the target shader artifact.
    /// </param>
    /// <param name="vertexLayout">
    /// The fullscreen quad layout that the returned pipeline must accept.
    /// </param>
    /// <returns>
    /// A validated premultiplied-alpha pipeline descriptor for the target renderer.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The target shader artifact is missing or has an invalid texture binding.
    /// </exception>
    public GraphicsPipelineDescriptor CreateDescriptor(
        GraphicsCapabilities capabilities,
        RenderVertexLayout vertexLayout
    ) => CreateDescriptor("Composition", RenderBlendState.premultiplied, capabilities, vertexLayout);

    /// <summary>
    /// Loads the target program that encodes the completed linear composition for presentation.
    /// </summary>
    /// <param name="capabilities">
    /// The active graphics backend used to select the compiled shader artifact.
    /// </param>
    /// <param name="vertexLayout">
    /// The fullscreen quad layout accepted by the output pipeline.
    /// </param>
    /// <returns>
    /// An opaque pipeline descriptor for the final linear-to-sRGB output transfer.
    /// </returns>
    public GraphicsPipelineDescriptor CreateOutputTransferDescriptor(
        GraphicsCapabilities capabilities,
        RenderVertexLayout vertexLayout
    ) => CreateDescriptor("OutputTransfer", RenderBlendState.opaque, capabilities, vertexLayout);

    private static GraphicsPipelineDescriptor CreateDescriptor(
        string programName,
        RenderBlendState blend,
        GraphicsCapabilities capabilities,
        RenderVertexLayout vertexLayout
    ) {
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentNullException.ThrowIfNull(vertexLayout);
        Assembly assembly = typeof(BgfxCompositionProgramProvider).Assembly;
        string suffix = "." + capabilities.backend.value;
        string[] resources = assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith($"Inno.BgfxShaders.{programName}.", StringComparison.Ordinal)
                && name.EndsWith(suffix, StringComparison.Ordinal))
            .ToArray();
        if (resources.Length != 1)
            throw new InvalidDataException(
                $"The BGFX adapter requires exactly one {programName} shader for '{capabilities.backend}'; found {resources.Length}.");
        using Stream source = assembly.GetManifestResourceStream(resources[0])
            ?? throw new InvalidDataException($"The BGFX adapter cannot read {programName} shader '{resources[0]}'.");
        using var bytes = new MemoryStream();
        source.CopyTo(bytes);
        GraphicsPipelineDescriptor program = GraphicsProgramArtifactCodec.Decode(bytes.ToArray(), vertexLayout);
        RenderShaderBindingDescriptor binding = program.bindings.Single();
        if (binding.id.value != "s_tex"
            || binding.kind != RenderShaderBindingKind.Texture
            || binding.slot != 0)
            throw new InvalidDataException($"The BGFX {programName} shader must expose s_tex at slot zero.");
        RenderRasterState authored = program.rasterState;
        var raster = new RenderRasterState(
            RenderCullMode.None, authored.frontFace, RenderDepthCompare.Always,
            depthWrite: false, blend,
            authored.colorWriteMask, authored.multisampling, authored.topology);
        return new GraphicsPipelineDescriptor(
            program.vertexShader.Span,
            program.fragmentShader.Span,
            [new(S_TEXTURE, RenderShaderBindingKind.Texture, slot: 0, nativeName: binding.nativeName)],
            vertexLayout, raster);
    }
}
