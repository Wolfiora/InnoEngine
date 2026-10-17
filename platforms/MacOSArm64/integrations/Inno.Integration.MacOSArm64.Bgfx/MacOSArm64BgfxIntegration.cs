using System;
using Inno.Build.Composition;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Integration.MacOSArm64.Bgfx;

/// <summary>
/// Connects platform publication facts to BGFX compilation without owning a product component list.
/// </summary>
public static class MacOSArm64BgfxIntegration
{
    /// <summary>
    /// Gets the immutable SDK invocation for this platform and backend pairing.
    /// </summary>
    public static BgfxNativeBuildProfile nativeProfile { get; } = new MacOSArm64BgfxBuildProfile();

    /// <summary>
    /// Gets the explicit shader configuration used by this integration.
    /// </summary>
    public static BgfxShaderTargetProfile shaderProfile => MacOSArm64BgfxShaderProfiles.target;

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
        ) => new BgfxGameContentCompiler(assets, serialization, types, shaderProfile, [GraphicsApi.Metal], tools);
    }
}
