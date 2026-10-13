using System;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

namespace Inno.Integration.BrowserWasm32.Bgfx;

/// <summary>
/// Connects platform publication facts to BGFX compilation without owning a product component list.
/// </summary>
public static class BrowserWasm32BgfxIntegration
{
    /// <summary>
    /// Gets WebGL 2 configuration and Emscripten header mappings for the shared static recipe.
    /// </summary>
    public static NativeComponentBuildOptions nativeOptions { get; } = new(
        NativeLibraryKind.Static,
        ["-DINNO_BGFX_RENDERER_DEFINITIONS=BGFX_CONFIG_RENDERER_OPENGLES=30",
            "-DINNO_BGFX_SDK_DEFINITIONS=__EMSCRIPTEN_MAJOR__=__EMSCRIPTEN_major__;__EMSCRIPTEN_MINOR__=__EMSCRIPTEN_minor__;__EMSCRIPTEN_TINY__=__EMSCRIPTEN_tiny__",
            "-DINNO_BGFX_SDK_OPTIONS=-include;emscripten/version.h"],
        ["platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Bgfx/BrowserWasm32BgfxIntegration.cs"]);

    /// <summary>
    /// Gets the explicit shader configuration used by this integration.
    /// </summary>
    public static BgfxShaderTargetProfile shaderProfile => BrowserWasm32BgfxShaderProfiles.target;

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
        ) => new BgfxGameContentCompiler(assets, serialization, types, shaderProfile, [GraphicsApi.OpenGLES], tools);
    }
}
