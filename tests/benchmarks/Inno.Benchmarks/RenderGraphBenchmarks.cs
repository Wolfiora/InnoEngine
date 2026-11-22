using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Inno.Rendering;

namespace Inno.Benchmarks;

[MemoryDiagnoser]
public class RenderGraphBenchmarks
{
    private static readonly RenderPhaseID PHASE = new("benchmarks.geometry");
    private GraphicsCapabilities m_capabilities = null!;
    private string[] m_passNames = [];
    private string[] m_textureNames = [];

    [Params(8, 64, 200)]
    public int passCount;

    [GlobalSetup]
    public void Setup()
    {
        m_passNames = new string[passCount];
        m_textureNames = new string[passCount];
        for (int index = 0; index < passCount; index++)
        {
            m_passNames[index] = "Pass " + index;
            m_textureNames[index] = "Texture " + index;
        }
        m_capabilities = new GraphicsCapabilities(
        GraphicsApi.noop,
        GraphicsCapability.Compute | GraphicsCapability.StorageBuffer | GraphicsCapability.StorageTexture
            | GraphicsCapability.TextureBlit | GraphicsCapability.Texture2DArray | GraphicsCapability.Index32,
        new GraphicsLimits(256, 8, 16_384, 16),
        Enum.GetValues<RenderTextureFormat>(),
        Enum.GetValues<RenderTextureFormat>(),
        (IReadOnlyList<RenderTextureFormat>)[RenderTextureFormat.RGBA8, RenderTextureFormat.RGBA16Float, RenderTextureFormat.R32Float],
        (IReadOnlyList<RenderTextureFormat>)[RenderTextureFormat.RGBA8, RenderTextureFormat.RGBA16Float, RenderTextureFormat.R32Float],
        false,
        false,
        null,
        null
    );
    }

    [Benchmark(Description = "Build+Compile chained raster graph")]
    public RenderGraphCompileResult BuildAndCompile()
    {
        var graph = new RenderGraphBuilder(1, m_capabilities);
        var descriptor = new RenderTextureDescriptor(256, 256, RenderTextureFormat.RGBA8, RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled);
        RenderTextureHandle previous = default;
        for (int index = 0; index < passCount; index++)
        {
            RenderTextureHandle target = graph.CreateTexture(m_textureNames[index], descriptor);
            RasterPassBuilder pass = graph.AddRasterPass(m_passNames[index], PHASE, index, static (_, _) => {});
            pass.UseColorAttachment(target, 0, RenderLoadAction.Clear);
            if (index > 0)
            {
                pass.ReadTexture(previous);
            }
            previous = target;
        }
        graph.MarkOutput(previous);
        RenderGraphCompileResult result = graph.Compile();
        if (!result.succeeded)
        {
            throw new InvalidOperationException(result.diagnostics.Count > 0 ? result.diagnostics[0].ToString() : "compile failed");
        }
        return result;
    }
}
