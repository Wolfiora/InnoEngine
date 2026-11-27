using System;
using Inno.Rendering;

namespace Inno.Benchmark.Rendering.Fixtures;

public static class BenchmarkGraphicsCapabilities
{
    public static GraphicsCapabilities Create()
        => new(
            GraphicsApi.metal,
            GraphicsCapability.None,
            new GraphicsLimits(256, 8, 8192, 16),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            false,
            false
        );
}
