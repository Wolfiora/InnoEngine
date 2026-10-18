using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    /// <summary>
    /// Updates the transform state and applies the resulting invariants.
    /// </summary>
    /// <param name="columnMajorMatrix">
    /// The column major matrix consumed by set transform; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void SetTransform(ReadOnlySpan<float> columnMajorMatrix)
    {
        if (columnMajorMatrix.Length != 16)
        {
            throw new ArgumentException("An object transform requires exactly sixteen values.", nameof(columnMajorMatrix));
        }

        fixed (float* matrix = columnMajorMatrix)
        {
            bgfx.encoder_set_transform(m_encoder, matrix, 1);
        }
    }

    /// <summary>
    /// Updates the raster state state and applies the resulting invariants.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    public override void SetRasterState(RenderRasterState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        _ = RequireGraphicsPipeline();
        if (state.blend.alphaToCoverage
            && !m_device.capabilities.Supports(GraphicsCapability.AlphaToCoverage))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support alpha-to-coverage rasterization.");
        }
        m_rasterState = state;
    }

    /// <summary>
    /// Updates the stencil state and applies the resulting invariants.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    public override void SetStencil(RenderStencilState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        _ = RequireGraphicsPipeline();
        if (state.enabled && state.writeMask != byte.MaxValue)
        {
            throw new NotSupportedException(
                "BGFX does not expose an independent dynamic stencil write mask.");
        }
        m_stencilState = state;
    }

    /// <summary>
    /// Updates the viewport state and applies the resulting invariants.
    /// </summary>
    /// <param name="x">
    /// The horizontal or first component.
    /// </param>
    /// <param name="y">
    /// The vertical or second component.
    /// </param>
    /// <param name="width">
    /// The width in logical units or pixels required by this operation.
    /// </param>
    /// <param name="height">
    /// The height in logical units or pixels required by this operation.
    /// </param>
    public override void SetViewport(
        int x,
        int y,
        int width,
        int height
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        bgfx.set_view_rect(
            m_viewId,
            checked((ushort)x),
            checked((ushort)y),
            checked((ushort)width),
            checked((ushort)height));
    }

    /// <summary>
    /// Updates the scissor state and applies the resulting invariants.
    /// </summary>
    /// <param name="x">
    /// The horizontal or first component.
    /// </param>
    /// <param name="y">
    /// The vertical or second component.
    /// </param>
    /// <param name="width">
    /// The width in logical units or pixels required by this operation.
    /// </param>
    /// <param name="height">
    /// The height in logical units or pixels required by this operation.
    /// </param>
    public override void SetScissor(
        int x,
        int y,
        int width,
        int height
    ) {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        bgfx.encoder_set_scissor(
            m_encoder,
            checked((ushort)x),
            checked((ushort)y),
            checked((ushort)width),
            checked((ushort)height));
    }

    private static ulong RasterState(RenderRasterState state)
    {
        bgfx.StateFlags flags = bgfx.StateFlags.None;
        if ((state.colorWriteMask & 0x01) != 0)
        {
            flags |= bgfx.StateFlags.WriteR;
        }

        if ((state.colorWriteMask & 0x02) != 0)
        {
            flags |= bgfx.StateFlags.WriteG;
        }

        if ((state.colorWriteMask & 0x04) != 0)
        {
            flags |= bgfx.StateFlags.WriteB;
        }

        if ((state.colorWriteMask & 0x08) != 0)
        {
            flags |= bgfx.StateFlags.WriteA;
        }

        if (state.depthWrite)
        {
            flags |= bgfx.StateFlags.WriteZ;
        }

        flags |= state.depthCompare switch
        {
            RenderDepthCompare.Never => bgfx.StateFlags.DepthTestNever,
            RenderDepthCompare.Less => bgfx.StateFlags.DepthTestLess,
            RenderDepthCompare.Equal => bgfx.StateFlags.DepthTestEqual,
            RenderDepthCompare.LessEqual => bgfx.StateFlags.DepthTestLequal,
            RenderDepthCompare.Greater => bgfx.StateFlags.DepthTestGreater,
            RenderDepthCompare.NotEqual => bgfx.StateFlags.DepthTestNotequal,
            RenderDepthCompare.GreaterEqual => bgfx.StateFlags.DepthTestGequal,
            RenderDepthCompare.Always => bgfx.StateFlags.DepthTestAlways,
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };

        if (state.frontFace == RenderFrontFace.CounterClockwise)
        {
            flags |= bgfx.StateFlags.FrontCcw;
        }

        flags |= state.cull switch
        {
            RenderCullMode.None => bgfx.StateFlags.None,
            RenderCullMode.Front when state.frontFace == RenderFrontFace.CounterClockwise => bgfx.StateFlags.CullCcw,
            RenderCullMode.Front => bgfx.StateFlags.CullCw,
            RenderCullMode.Back when state.frontFace == RenderFrontFace.CounterClockwise => bgfx.StateFlags.CullCw,
            RenderCullMode.Back => bgfx.StateFlags.CullCcw,
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };

        if (state.blend.enabled)
        {
            flags = (bgfx.StateFlags)((ulong)flags | BlendFunction(state.blend) | BlendEquation(state.blend));
        }

        if (state.blend.alphaToCoverage)
            flags |= bgfx.StateFlags.BlendAlphaToCoverage;

        if (state.multisampling)
        {
            flags |= bgfx.StateFlags.Msaa;
        }

        flags |= state.topology switch
        {
            RenderPrimitiveTopology.TriangleList => bgfx.StateFlags.None,
            RenderPrimitiveTopology.TriangleStrip => bgfx.StateFlags.PtTristrip,
            RenderPrimitiveTopology.LineList => bgfx.StateFlags.PtLines,
            RenderPrimitiveTopology.LineStrip => bgfx.StateFlags.PtLinestrip,
            RenderPrimitiveTopology.PointList => bgfx.StateFlags.PtPoints,
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };

        return (ulong)flags;
    }

    private static bgfx.Access StorageAccess(RenderStorageAccess access)
        => access switch
        {
            RenderStorageAccess.Read => bgfx.Access.Read,
            RenderStorageAccess.Write => bgfx.Access.Write,
            RenderStorageAccess.ReadWrite => bgfx.Access.ReadWrite,
            _ => throw new ArgumentOutOfRangeException(nameof(access))
        };

    private static uint SamplerFlags(RenderSamplerState state)
    {
        bgfx.SamplerFlags flags = state.filter switch
        {
            RenderSamplerFilter.Point => bgfx.SamplerFlags.Point,
            RenderSamplerFilter.Linear => bgfx.SamplerFlags.None,
            RenderSamplerFilter.Anisotropic => bgfx.SamplerFlags.MinAnisotropic
                | bgfx.SamplerFlags.MagAnisotropic,
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };
        flags |= AddressFlags(state.addressU, 'U');
        flags |= AddressFlags(state.addressV, 'V');
        flags |= AddressFlags(state.addressW, 'W');
        return (uint)flags;
    }

    private static bgfx.SamplerFlags AddressFlags(
        RenderSamplerAddressMode mode,
        char axis
    )
        => (axis, mode) switch
        {
            (_, RenderSamplerAddressMode.Repeat) => bgfx.SamplerFlags.None,
            ('U', RenderSamplerAddressMode.Mirror) => bgfx.SamplerFlags.UMirror,
            ('U', RenderSamplerAddressMode.Clamp) => bgfx.SamplerFlags.UClamp,
            ('U', RenderSamplerAddressMode.Border) => bgfx.SamplerFlags.UBorder,
            ('V', RenderSamplerAddressMode.Mirror) => bgfx.SamplerFlags.VMirror,
            ('V', RenderSamplerAddressMode.Clamp) => bgfx.SamplerFlags.VClamp,
            ('V', RenderSamplerAddressMode.Border) => bgfx.SamplerFlags.VBorder,
            ('W', RenderSamplerAddressMode.Mirror) => bgfx.SamplerFlags.WMirror,
            ('W', RenderSamplerAddressMode.Clamp) => bgfx.SamplerFlags.WClamp,
            ('W', RenderSamplerAddressMode.Border) => bgfx.SamplerFlags.WBorder,
            _ => throw new ArgumentOutOfRangeException(nameof(mode))
        };

    private static uint StencilFlags(
        RenderStencilState state,
        RenderStencilFaceState face
    ) {
        bgfx.StencilFlags flags = (bgfx.StencilFlags)(
            state.reference
            | (uint)(state.readMask << (int)bgfx.StencilFlags.FuncRmaskShift));
        flags |= face.compare switch
        {
            RenderStencilCompare.Never => bgfx.StencilFlags.TestNever,
            RenderStencilCompare.Less => bgfx.StencilFlags.TestLess,
            RenderStencilCompare.Equal => bgfx.StencilFlags.TestEqual,
            RenderStencilCompare.LessEqual => bgfx.StencilFlags.TestLequal,
            RenderStencilCompare.Greater => bgfx.StencilFlags.TestGreater,
            RenderStencilCompare.NotEqual => bgfx.StencilFlags.TestNotequal,
            RenderStencilCompare.GreaterEqual => bgfx.StencilFlags.TestGequal,
            RenderStencilCompare.Always => bgfx.StencilFlags.TestAlways,
            _ => throw new ArgumentOutOfRangeException(nameof(face))
        };
        flags |= StencilOperation(face.fail, 0);
        flags |= StencilOperation(face.depthFail, 1);
        flags |= StencilOperation(face.pass, 2);
        return (uint)flags;
    }

    private static bgfx.StencilFlags StencilOperation(
        RenderStencilOperation operation,
        int field
    ) {
        int value = operation switch
        {
            RenderStencilOperation.Zero => 0,
            RenderStencilOperation.Keep => 1,
            RenderStencilOperation.Replace => 2,
            RenderStencilOperation.IncrementWrap => 3,
            RenderStencilOperation.IncrementClamp => 4,
            RenderStencilOperation.DecrementWrap => 5,
            RenderStencilOperation.DecrementClamp => 6,
            RenderStencilOperation.Invert => 7,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };
        int shift = field switch
        {
            0 => (int)bgfx.StencilFlags.OpFailSShift,
            1 => (int)bgfx.StencilFlags.OpFailZShift,
            2 => (int)bgfx.StencilFlags.OpPassZShift,
            _ => throw new ArgumentOutOfRangeException(nameof(field))
        };
        return (bgfx.StencilFlags)((uint)value << shift);
    }

    private static ulong BlendFunction(RenderBlendState state)
    {
        ulong colorSource = (ulong)BlendFactor(state.colorSource);
        ulong colorDestination = (ulong)BlendFactor(state.colorDestination);
        ulong alphaSource = (ulong)BlendFactor(state.alphaSource);
        ulong alphaDestination = (ulong)BlendFactor(state.alphaDestination);
        return colorSource
            | (colorDestination << 4)
            | ((alphaSource | (alphaDestination << 4)) << 8);
    }

    private static ulong BlendEquation(RenderBlendState state)
    {
        ulong color = (ulong)BlendEquation(state.colorEquation);
        ulong alpha = (ulong)BlendEquation(state.alphaEquation);
        return color | (alpha << 3);
    }

    private static bgfx.StateFlags BlendFactor(RenderBlendFactor factor)
        => factor switch
        {
            RenderBlendFactor.Zero => bgfx.StateFlags.BlendZero,
            RenderBlendFactor.One => bgfx.StateFlags.BlendOne,
            RenderBlendFactor.SourceColor => bgfx.StateFlags.BlendSrcColor,
            RenderBlendFactor.InverseSourceColor => bgfx.StateFlags.BlendInvSrcColor,
            RenderBlendFactor.SourceAlpha => bgfx.StateFlags.BlendSrcAlpha,
            RenderBlendFactor.InverseSourceAlpha => bgfx.StateFlags.BlendInvSrcAlpha,
            RenderBlendFactor.DestinationAlpha => bgfx.StateFlags.BlendDstAlpha,
            RenderBlendFactor.InverseDestinationAlpha => bgfx.StateFlags.BlendInvDstAlpha,
            RenderBlendFactor.DestinationColor => bgfx.StateFlags.BlendDstColor,
            RenderBlendFactor.InverseDestinationColor => bgfx.StateFlags.BlendInvDstColor,
            RenderBlendFactor.SourceAlphaSaturate => bgfx.StateFlags.BlendSrcAlphaSat,
            RenderBlendFactor.Constant => bgfx.StateFlags.BlendFactor,
            RenderBlendFactor.InverseConstant => bgfx.StateFlags.BlendInvFactor,
            _ => throw new ArgumentOutOfRangeException(nameof(factor))
        };

    private static bgfx.StateFlags BlendEquation(RenderBlendEquation equation)
        => equation switch
        {
            RenderBlendEquation.Add => bgfx.StateFlags.BlendEquationAdd,
            RenderBlendEquation.Subtract => bgfx.StateFlags.BlendEquationSub,
            RenderBlendEquation.ReverseSubtract => bgfx.StateFlags.BlendEquationRevsub,
            RenderBlendEquation.Minimum => bgfx.StateFlags.BlendEquationMin,
            RenderBlendEquation.Maximum => bgfx.StateFlags.BlendEquationMax,
            _ => throw new ArgumentOutOfRangeException(nameof(equation))
        };

}
