using System;
using Inno.Build.Composition;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Integration.WindowsX64.Bgfx;

/// <summary>
/// Connects platform publication facts to BGFX compilation without owning a product component list.
/// </summary>
public static class WindowsX64BgfxIntegration
{
    /// <summary>
    /// Gets the immutable SDK invocation for this platform and backend pairing.
    /// </summary>
    public static BgfxNativeBuildProfile nativeProfile { get; } = new WindowsX64BgfxBuildProfile();

    /// <summary>
    /// Gets the explicit shader configuration used by this integration.
    /// </summary>
    public static BgfxShaderTargetProfile shaderProfile => WindowsX64BgfxShaderProfiles.target;

    /// <summary>
    /// Creates the scoped content factory using the caller's protected offline tool deployment.
    /// </summary>
    /// <param name="tools">
    /// The borrowed executables and shader includes, retained through every compiler created by this factory.
    /// </param>
    /// <returns>
    /// A factory borrowing current authoring services without owning them or the selected tools.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The selected tools are null.
    /// </exception>
    public static GameContentCompilerFactory CreateContentCompilerFactory(ToolRunner tools)
    {
        ArgumentNullException.ThrowIfNull(tools);
        return (
            assets,
            serialization,
            types
        ) => new BgfxGameContentCompiler(assets, serialization, types, shaderProfile, [GraphicsApi.Direct3D11, GraphicsApi.Direct3D12, GraphicsApi.Vulkan, GraphicsApi.OpenGL], tools);
    }
}
