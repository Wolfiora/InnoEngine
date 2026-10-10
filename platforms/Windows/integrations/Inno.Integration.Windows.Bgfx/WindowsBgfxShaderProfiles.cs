using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

using Inno.Build.Windows;

namespace Inno.Integration.Windows.Bgfx;

/// <summary>
/// Owns explicit Windows shader dialects and offline product capabilities.
/// </summary>
public static class WindowsBgfxShaderProfiles
{
    /// <summary>
    /// Gets the immutable configuration shared by this platform's products and publication module.
    /// </summary>
    public static BgfxShaderTargetProfile target { get; } = new(WindowsBuildModule.target.id.value,
    [
        new(BgfxTargetCapabilities.Create(GraphicsApi.Direct3D11), "windows", "s_5_0", "s_5_0", "s_5_0"),
        new(BgfxTargetCapabilities.Create(GraphicsApi.Direct3D12), "windows", "s_5_0", "s_5_0", "s_5_0"),
        new(BgfxTargetCapabilities.Create(GraphicsApi.Vulkan), "windows", "spirv", "spirv", "spirv"),
        new(BgfxTargetCapabilities.Create(GraphicsApi.OpenGL), "windows", "430", "430", "430")
    ]);
}
