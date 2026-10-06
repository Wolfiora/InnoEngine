using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Selects the triangle face rejected by a raster pass.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("a0efdb24-2746-512d-aef5-46180c539581")]
public enum ShaderCullMode
{
    /// <summary>
    /// Does not reject either face orientation.
    /// </summary>
    None,
    /// <summary>
    /// Rejects front-facing triangles.
    /// </summary>
    Front,
    /// <summary>
    /// Rejects back-facing triangles.
    /// </summary>
    Back
}

/// <summary>
/// Selects the comparison used by depth testing.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("cababee4-46b3-5bd8-800f-46d58149959b")]
public enum ShaderCompareFunction
{
    /// <summary>
    /// Always rejects the fragment.
    /// </summary>
    Never,
    /// <summary>
    /// Accepts a fragment with a smaller depth.
    /// </summary>
    Less,
    /// <summary>
    /// Accepts a fragment with an equal depth.
    /// </summary>
    Equal,
    /// <summary>
    /// Accepts a fragment with a smaller or equal depth.
    /// </summary>
    LessEqual,
    /// <summary>
    /// Accepts a fragment with a greater depth.
    /// </summary>
    Greater,
    /// <summary>
    /// Accepts a fragment with a different depth.
    /// </summary>
    NotEqual,
    /// <summary>
    /// Accepts a fragment with a greater or equal depth.
    /// </summary>
    GreaterEqual,
    /// <summary>
    /// Always accepts the fragment.
    /// </summary>
    Always
}

/// <summary>
/// Declares backend-neutral fixed-function state for one shader pass.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("1ee6df72-41ef-5fd0-a32a-2a8220164675")]
public struct ShaderRenderState
{
    /// <summary>
    /// Gets the default opaque raster state.
    /// </summary>
    public static ShaderRenderState opaque => new()
    {
        topology = RenderPrimitiveTopology.TriangleList,
        cull = ShaderCullMode.Back,
        frontFace = RenderFrontFace.CounterClockwise,
        depthCompare = ShaderCompareFunction.LessEqual,
        depthWrite = true,
        blend = RenderBlendState.opaque,
        colorWriteMask = 0x0f,
        multisampling = true
    };

    /// <summary>
    /// Gets or sets the primitive assembly used by raster draws.
    /// </summary>
    public RenderPrimitiveTopology topology { get; set; }

    /// <summary>
    /// Gets or sets the face culling mode.
    /// </summary>
    public ShaderCullMode cull { get; set; }

    /// <summary>
    /// Gets or sets the winding order interpreted as the front face.
    /// </summary>
    public RenderFrontFace frontFace { get; set; }

    /// <summary>
    /// Gets or sets the depth comparison function.
    /// </summary>
    public ShaderCompareFunction depthCompare { get; set; }

    /// <summary>
    /// Gets or sets whether accepted fragments update depth.
    /// </summary>
    public bool depthWrite { get; set; }

    /// <summary>
    /// Gets or sets independent RGB and alpha blending.
    /// </summary>
    public RenderBlendState blend { get; set; }

    /// <summary>
    /// Gets or sets the four-bit RGBA color write mask.
    /// </summary>
    public byte colorWriteMask { get; set; }

    /// <summary>
    /// Gets or sets whether compatible targets use multisample rasterization.
    /// </summary>
    public bool multisampling { get; set; }
}

