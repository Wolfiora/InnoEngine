using Inno.Build.Composition;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Integration.MacOS.Bgfx;

/// <summary>
/// Connects platform publication facts to BGFX compilation without owning a product component list.
/// </summary>
public static class MacOSBgfxIntegration
{
    /// <summary>
    /// Gets the immutable SDK invocation for this platform and backend pairing.
    /// </summary>
    public static BgfxNativeBuildProfile nativeProfile { get; } = new MacOSBgfxBuildProfile();

    /// <summary>
    /// Gets the explicit shader configuration used by this integration.
    /// </summary>
    public static BgfxShaderTargetProfile shaderProfile => MacOSBgfxShaderProfiles.target;

    /// <summary>
    /// Gets the scoped factory for the currently supported Player graphics API closure.
    /// </summary>
    public static GameContentCompilerFactory contentCompilerFactory { get; } = static (
        assets,
        serialization,
        types
    ) => new BgfxGameContentCompiler(assets, serialization, types, shaderProfile, [GraphicsApi.Metal]);
}
