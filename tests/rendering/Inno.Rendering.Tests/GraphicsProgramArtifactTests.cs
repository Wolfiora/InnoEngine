using System;
using System.Buffers.Binary;
using System.IO;
using Xunit;

namespace Inno.Rendering.Tests;

public sealed class GraphicsProgramArtifactTests
{
    [Fact]
    public void DeviceProgramRoundTripPreservesBinariesBindingsAndRasterWithoutAuthoredAssets()
    {
        byte[] vertex = [1, 2, 3];
        byte[] fragment = [4, 5];
        var raster = new RenderRasterState(RenderCullMode.None, RenderFrontFace.CounterClockwise,
            RenderDepthCompare.Always, false, RenderBlendState.premultiplied, 0xF, true, RenderPrimitiveTopology.TriangleList);
        var descriptor = new GraphicsPipelineDescriptor(vertex, fragment,
            [new RenderShaderBindingDescriptor(new RenderBindingId("s_tex"), RenderShaderBindingKind.Texture,
                slot: 3, nativeName: "native_sampler")], null, raster);
        byte[] bytes = GraphicsProgramArtifactCodec.Encode(descriptor);
        Assert.Equal(bytes, GraphicsProgramArtifactCodec.Encode(descriptor));
        GraphicsPipelineDescriptor decoded = GraphicsProgramArtifactCodec.Decode(bytes);
        Assert.Equal(vertex, decoded.vertexShader.ToArray());
        Assert.Equal(fragment, decoded.fragmentShader.ToArray());
        Assert.Null(decoded.vertexLayout);
        RenderShaderBindingDescriptor binding = Assert.Single(decoded.bindings);
        Assert.Equal("s_tex", binding.id.value);
        Assert.Equal("native_sampler", binding.nativeName);
        Assert.Equal(3, binding.slot);
        Assert.Equal(raster.cull, decoded.rasterState.cull);
        Assert.Equal(raster.frontFace, decoded.rasterState.frontFace);
        Assert.Equal(raster.blend, decoded.rasterState.blend);
        vertex[0] = 99;
        bytes.AsSpan().Fill(99);
        Assert.Equal(1, decoded.vertexShader.Span[0]);
    }

    [Fact]
    public void InvalidBindingAndRasterValuesCannotBeEncodedAsValidArtifacts()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RenderShaderBindingDescriptor(
            new("invalid"), RenderShaderBindingKind.Uniform, uniformType: (RenderUniformType)99));
        var invalidRaster = new RenderRasterState { colorWriteMask = 0xFF };
        var descriptor = new GraphicsPipelineDescriptor([1], [2], [], null, invalidRaster);
        Assert.Throws<InvalidDataException>(() => GraphicsProgramArtifactCodec.Encode(descriptor));
        var invalidBlend = new RenderRasterState { blend = new RenderBlendState { colorSource = (RenderBlendFactor)99 } };
        Assert.Throws<InvalidDataException>(() => GraphicsProgramArtifactCodec.Encode(
            new GraphicsPipelineDescriptor([1], [2], [], null, invalidBlend)));
    }

    [Fact]
    public void TruncationOversizedFieldsBadHeadersAndTrailingBytesFailBeforePublication()
    {
        byte[] bytes = GraphicsProgramArtifactCodec.Encode(new GraphicsPipelineDescriptor([1], [2], [], null));
        for (int length = 0; length < bytes.Length; length++)
            Assert.Throws<InvalidDataException>(() => GraphicsProgramArtifactCodec.Decode(bytes.AsSpan(0, length)));
        byte[] excessive = (byte[])bytes.Clone();
        BinaryPrimitives.WriteInt32LittleEndian(excessive.AsSpan("INNOGPU"u8.Length), int.MaxValue);
        Assert.Throws<InvalidDataException>(() => GraphicsProgramArtifactCodec.Decode(excessive));
        byte[] header = (byte[])bytes.Clone();
        header[0] = 0;
        Assert.Throws<InvalidDataException>(() => GraphicsProgramArtifactCodec.Decode(header));
        byte[] trailing = new byte[bytes.Length + 1];
        bytes.CopyTo(trailing, 0);
        Assert.Throws<InvalidDataException>(() => GraphicsProgramArtifactCodec.Decode(trailing));
    }
}
