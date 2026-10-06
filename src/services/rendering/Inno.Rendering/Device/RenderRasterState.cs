using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Selects triangle culling for a backend-neutral raster pipeline.
/// </summary>
public enum RenderCullMode
{
    /// <summary>
    /// Disables face culling.
    /// </summary>
    None,
    /// <summary>
    /// Culls front-facing triangles.
    /// </summary>
    Front,
    /// <summary>
    /// Culls back-facing triangles.
    /// </summary>
    Back
}

/// <summary>
/// Selects the winding order interpreted as the front face.
/// </summary>
public enum RenderFrontFace
{
    /// <summary>
    /// Clockwise vertices form a front-facing triangle.
    /// </summary>
    Clockwise,
    /// <summary>
    /// Counter-clockwise vertices form a front-facing triangle.
    /// </summary>
    CounterClockwise
}

/// <summary>
/// Selects depth comparison for a backend-neutral raster pipeline.
/// </summary>
public enum RenderDepthCompare
{
    /// <summary>
    /// Always rejects.
    /// </summary>
    Never,
    /// <summary>
    /// Accepts smaller depth.
    /// </summary>
    Less,
    /// <summary>
    /// Accepts equal depth.
    /// </summary>
    Equal,
    /// <summary>
    /// Accepts smaller or equal depth.
    /// </summary>
    LessEqual,
    /// <summary>
    /// Accepts greater depth.
    /// </summary>
    Greater,
    /// <summary>
    /// Accepts unequal depth.
    /// </summary>
    NotEqual,
    /// <summary>
    /// Accepts greater or equal depth.
    /// </summary>
    GreaterEqual,
    /// <summary>
    /// Always accepts.
    /// </summary>
    Always
}

/// <summary>
/// Stores backend-neutral fixed-function raster state.
/// </summary>
public sealed class RenderRasterState
{
    /// <summary>
    /// Creates the default opaque raster state.
    /// </summary>
    public RenderRasterState()
    {
    }

    /// <summary>
    /// Creates a complete immutable raster configuration without relying on init-only setters.
    /// </summary>
    /// <param name="cull">
    /// Face culling mode.
    /// </param>
    /// <param name="frontFace">
    /// Winding order interpreted as the front face.
    /// </param>
    /// <param name="depthCompare">
    /// Depth comparison.
    /// </param>
    /// <param name="depthWrite">
    /// Whether accepted fragments update depth.
    /// </param>
    /// <param name="blend">
    /// Independent RGB and alpha blending.
    /// </param>
    /// <param name="colorWriteMask">
    /// Four-bit RGBA write mask.
    /// </param>
    /// <param name="multisampling">
    /// Whether multisample rasterization is enabled.
    /// </param>
    /// <param name="topology">
    /// Primitive assembly for subsequent draw commands.
    /// </param>
    public RenderRasterState(
        RenderCullMode cull,
        RenderFrontFace frontFace,
        RenderDepthCompare depthCompare,
        bool depthWrite,
        RenderBlendState blend,
        byte colorWriteMask,
        bool multisampling,
        RenderPrimitiveTopology topology
    ) {
        this.cull = cull;
        this.frontFace = frontFace;
        this.depthCompare = depthCompare;
        this.depthWrite = depthWrite;
        this.blend = blend;
        this.colorWriteMask = colorWriteMask;
        this.multisampling = multisampling;
        this.topology = topology;
    }

    /// <summary>
    /// Gets the default opaque raster state.
    /// </summary>
    public static RenderRasterState opaque { get; } = new();

    /// <summary>
    /// Gets the face culling mode.
    /// </summary>
    public RenderCullMode cull { get; init; } = RenderCullMode.Back;

    /// <summary>
    /// Gets the winding order interpreted as the front face.
    /// </summary>
    public RenderFrontFace frontFace { get; init; } = RenderFrontFace.CounterClockwise;

    /// <summary>
    /// Gets depth comparison.
    /// </summary>
    public RenderDepthCompare depthCompare { get; init; } = RenderDepthCompare.LessEqual;

    /// <summary>
    /// Gets whether accepted fragments update depth.
    /// </summary>
    public bool depthWrite { get; init; } = true;

    /// <summary>
    /// Gets independent RGB and alpha blending.
    /// </summary>
    public RenderBlendState blend { get; init; } = RenderBlendState.opaque;

    /// <summary>
    /// Gets the four-bit RGBA write mask.
    /// </summary>
    public byte colorWriteMask { get; init; } = 0x0f;

    /// <summary>
    /// Gets whether multisample rasterization is enabled for compatible targets.
    /// </summary>
    public bool multisampling { get; init; } = true;

    /// <summary>
    /// Gets primitive assembly for subsequent draw commands.
    /// </summary>
    public RenderPrimitiveTopology topology { get; init; } = RenderPrimitiveTopology.TriangleList;
}

