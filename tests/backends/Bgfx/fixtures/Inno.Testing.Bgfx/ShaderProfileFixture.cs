using System;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Testing.Bgfx;

/// <summary>Defines explicit shader target fixtures shared by metadata and real compiler tests.</summary>
public static class ShaderProfileFixture
{
    /// <summary>Gets the explicit windows shader profile used by the tests.</summary>
    public static BgfxShaderTargetProfile windows { get; } = new("fixture-windows",
        [new(BgfxTargetCapabilities.Create(GraphicsApi.Direct3D11), "windows", "s_5_0", "s_5_0", "s_5_0"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.OpenGL), "windows", "430", "430", "430"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.Vulkan), "windows", "spirv", "spirv", "spirv")]);
    /// <summary>Gets the explicit mac shader profile used by the tests.</summary>
    public static BgfxShaderTargetProfile mac { get; } = new("fixture-mac",
        [new(BgfxTargetCapabilities.Create(GraphicsApi.Metal), "osx", "metal", "metal", "metal"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.OpenGL), "osx", "430", "430", "430"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.Vulkan), "osx", "spirv", "spirv", "spirv", ["BGFX_SHADER_LANGUAGE_SPIRV=1"])]);
    /// <summary>Gets the explicit browser shader profile used by the tests.</summary>
    public static BgfxShaderTargetProfile browser { get; } = new("fixture-browser",
        [new(new GraphicsCapabilities(GraphicsApi.OpenGLES, GraphicsCapability.None,
            new GraphicsLimits(256, 4, 2048, 0), [RenderTextureFormat.RGBA8], [RenderTextureFormat.RGBA8],
            [], [], true, true), "asm.js", "300_es", "300_es", "")]);

    /// <summary>Creates the declared rendering capabilities without loading a native runtime.</summary>
    /// <param name="backend">The explicit graphics API.</param>
    /// <param name="features">The capabilities required by the test.</param>
    /// <returns>The immutable capability description.</returns>

    public static GraphicsCapabilities CreateCapabilities(
        GraphicsApi backend,
        GraphicsCapability features
    ) => new(
            backend,
            features,
            new GraphicsLimits(256, 8, 8192, 16),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            originBottomLeft: false,
            homogeneousDepth: false);
}
