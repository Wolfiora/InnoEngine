using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Integration.Browser.Bgfx;

/// <summary>
/// Connects platform publication facts to BGFX compilation without owning a product component list.
/// </summary>
public static class BrowserBgfxIntegration
{
    /// <summary>
    /// Gets WebGL 2 configuration and Emscripten header mappings for the shared static recipe.
    /// </summary>
    public static NativeComponentBuildOptions nativeOptions { get; } = new(
        NativeLibraryKind.Static,
        ["-DINNO_BGFX_RENDERER_DEFINITIONS=BGFX_CONFIG_RENDERER_OPENGLES=30",
            "-DINNO_BGFX_SDK_DEFINITIONS=__EMSCRIPTEN_MAJOR__=__EMSCRIPTEN_major__;__EMSCRIPTEN_MINOR__=__EMSCRIPTEN_minor__;__EMSCRIPTEN_TINY__=__EMSCRIPTEN_tiny__",
            "-DINNO_BGFX_SDK_OPTIONS=-include;emscripten/version.h"],
        ["platforms/Browser/integrations/Inno.Integration.Browser.Bgfx/BrowserBgfxIntegration.cs"]);

    /// <summary>
    /// Gets the explicit shader configuration used by this integration.
    /// </summary>
    public static BgfxShaderTargetProfile shaderProfile => BrowserBgfxShaderProfiles.target;

    /// <summary>
    /// Gets the scoped factory for the currently supported Player graphics API closure.
    /// </summary>
    public static GameContentCompilerFactory contentCompilerFactory { get; } = static (
        assets,
        serialization,
        types
    ) => new BgfxGameContentCompiler(assets, serialization, types, shaderProfile, [GraphicsApi.OpenGLES]);
}
