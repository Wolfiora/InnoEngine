using System.Collections.Generic;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed class BgfxPipelineResource
{
    /// <summary>
    /// Creates a validated bgfx pipeline resource instance.
    /// </summary>
    /// <param name="program">
    /// The program consumed by bgfx pipeline resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="bindings">
    /// The bindings consumed by bgfx pipeline resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="vertexLayout">
    /// The vertex layout consumed by bgfx pipeline resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="vertexLayoutHandle">
    /// The vertex layout handle consumed by bgfx pipeline resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="rasterState">
    /// The raster state consumed by bgfx pipeline resource; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="compute">
    /// Whether compute behavior is enabled while bgfx pipeline resource executes.
    /// </param>
    public BgfxPipelineResource(
        bgfx.ProgramHandle program,
        IReadOnlyDictionary<string, BgfxShaderBindingResource> bindings,
        RenderVertexLayout? vertexLayout,
        bgfx.VertexLayoutHandle vertexLayoutHandle,
        RenderRasterState? rasterState,
        bool compute
    ) {
        this.program = program;
        this.bindings = bindings;
        this.vertexLayout = vertexLayout;
        this.vertexLayoutHandle = vertexLayoutHandle;
        this.rasterState = rasterState;
        this.compute = compute;
    }

    /// <summary>
    /// Gets the linked BGFX shader program owned by this pipeline resource.
    /// </summary>
    public bgfx.ProgramHandle program { get; }

    /// <summary>
    /// Gets the immutable lookup of stable binding IDs to BGFX uniform resources.
    /// </summary>
    public IReadOnlyDictionary<string, BgfxShaderBindingResource> bindings { get; }

    /// <summary>
    /// Gets the optional vertex layout required by this resource or pipeline.
    /// </summary>
    public RenderVertexLayout? vertexLayout { get; }

    /// <summary>
    /// Gets the BGFX vertex-layout handle owned by this pipeline resource.
    /// </summary>
    public bgfx.VertexLayoutHandle vertexLayoutHandle { get; }

    /// <summary>
    /// Gets the optional rasterization state encoded by this pipeline.
    /// </summary>
    public RenderRasterState? rasterState { get; }

    /// <summary>
    /// Gets whether the caller-visible condition represented by this property is satisfied.
    /// </summary>
    public bool compute { get; }
}
