using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Build.WindowsX64;
using Inno.Rendering;

namespace Inno.Integration.WindowsX64.Bgfx;

/// <summary>
/// Owns explicit Windows shader dialects and offline product capabilities.
/// </summary>
public static class WindowsX64BgfxShaderProfiles
{
    /// <summary>
    /// Gets the immutable configuration shared by this platform's products and publication module.
    /// </summary>
    public static BgfxShaderTargetProfile target { get; } = new(
        WindowsX64BuildModule.target.id.value,
        [
            new(BgfxTargetCapabilities.Create(GraphicsApi.direct3D11), "windows", "s_5_0", "s_5_0", "s_5_0"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.direct3D12), "windows", "s_5_0", "s_5_0", "s_5_0"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.vulkan), "windows", "spirv", "spirv", "spirv"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.openGL), "windows", "430", "430", "430")
        ]
    );
}
