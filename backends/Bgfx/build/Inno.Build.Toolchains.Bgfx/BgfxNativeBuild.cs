using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Publishes pinned graphics runtime libraries from an isolated source snapshot.
/// </summary>
public static class BgfxNativeBuild
{
    /// <summary>
    /// Gets the unique native and recipe owners used for sources and target-scoped intermediates.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "bgfx",
        "backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj",
        "backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/Inno.Build.Toolchains.Bgfx.csproj",
        new NativeStaticBuildDefinition(
            "backends/Bgfx/build/Inno.Build.Toolchains.Bgfx/Native/StaticLibraries.cmake",
            ["extern/bgfx", "extern/bx", "extern/bimg"],
            ["bgfxRelease", "bxRelease", "bimgRelease", "bimg_decodeRelease"]),
        bindingDefinition: "backends/Bgfx/native/Inno.Native.Bgfx/Bindings/bindings.props");

    /// <summary>
    /// Builds and installs graphics artifacts using explicit platform-owned SDK configuration.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose sources and outputs belong to this operation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is not debug or release.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A native build process fails.
    /// </exception>
    /// <param name="profile">
    /// The platform-owned project generation, SDK invocation and output selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels child processes and prevents artifact installation after cancellation.
    /// </param>
    /// <returns>
    /// The validated product with an exact graphics library or offline-tool output closure.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        BgfxNativeBuildProfile profile,
        CancellationToken cancellationToken = default
    ) => BgfxBuildSession.PublishAsync(context, profile, false, cancellationToken);

    /// <summary>
    /// Freezes runtime source, selected tools and profile arguments without generating or compiling.
    /// </summary>
    /// <param name="context">
    /// The operation with an explicit SDK target and shared component options.
    /// </param>
    /// <param name="profile">
    /// The borrowed immutable integration profile supplying tools, invocation and output layout.
    /// </param>
    /// <returns>
    /// The same source declaration consumed by the runtime producer, without selected binding output.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context or profile is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The profile target or required arguments are invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// SDK tools or component options have not been selected.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The request uses unsupported linkage or component CMake options.
    /// </exception>
    /// <exception cref="System.IO.IOException">
    /// A selected tool, build owner or required source checkout is unavailable.
    /// </exception>
    public static NativeBuildRecipe CreateRecipe(
        NativeBuildContext context,
        BgfxNativeBuildProfile profile
    ) => new BgfxBuildRequest(context, profile, false).recipe;
}
