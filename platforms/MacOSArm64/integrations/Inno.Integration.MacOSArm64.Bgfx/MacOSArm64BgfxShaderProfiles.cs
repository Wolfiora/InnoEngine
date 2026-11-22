using Inno.Build.MacOSArm64;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Integration.MacOSArm64.Bgfx;

/// <summary>
/// Owns explicit MacOS shader dialects and offline product capabilities.
/// </summary>
public static class MacOSArm64BgfxShaderProfiles
{
    /// <summary>
    /// Gets the immutable configuration shared by this platform's products and publication module.
    /// </summary>
    public static BgfxShaderTargetProfile target { get; } = new(
        MacOSArm64BuildModule.target.id.value,
        [
            new(BgfxTargetCapabilities.Create(GraphicsApi.metal), "osx", "metal", "metal", "metal"),
            new(BgfxTargetCapabilities.Create(GraphicsApi.vulkan), "osx", "spirv", "spirv", "spirv", ["BGFX_SHADER_LANGUAGE_SPIRV=1"]),
            new(BgfxTargetCapabilities.Create(GraphicsApi.openGL), "osx", "430", "430", "430")
        ]
    );
}
