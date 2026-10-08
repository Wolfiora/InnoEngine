using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

internal static class ShaderProfileFixture
{
    internal static BgfxShaderTargetProfile windows { get; } = new("fixture-windows",
        [new(BgfxTargetCapabilities.Create(GraphicsApi.Direct3D11), "windows", "s_5_0", "s_5_0", "s_5_0"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.OpenGL), "windows", "430", "430", "430"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.Vulkan), "windows", "spirv", "spirv", "spirv")]);
    internal static BgfxShaderTargetProfile mac { get; } = new("fixture-mac",
        [new(BgfxTargetCapabilities.Create(GraphicsApi.Metal), "osx", "metal", "metal", "metal"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.OpenGL), "osx", "430", "430", "430"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.Vulkan), "osx", "spirv", "spirv", "spirv", ["BGFX_SHADER_LANGUAGE_SPIRV=1"])]);
    internal static BgfxShaderTargetProfile browser { get; } = new("fixture-browser",
        [new(new GraphicsCapabilities(GraphicsApi.OpenGLES, GraphicsCapability.None,
            new GraphicsLimits(256, 4, 2048, 0), [RenderTextureFormat.RGBA8], [RenderTextureFormat.RGBA8],
            [], [], true, true), "asm.js", "300_es", "300_es", "")]);
}
