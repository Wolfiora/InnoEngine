using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Sdl3;

/// <summary>
/// Owns this backend's native recipe while SDK selection remains with the platform provider.
/// </summary>
public static class Sdl3Toolchain
{
    /// <summary>
    /// Gets the component's sole source, binding and intermediate owner.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "sdl3",
        "backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj",
        "backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Inno.Build.Toolchains.Sdl3.csproj",
        new NativeStaticBuildDefinition(
            "backends/Sdl3/build/Inno.Build.Toolchains.Sdl3/Native/StaticLibraries.cmake",
            ["extern/SDL"],
            ["libSDL3.a"],
            ["browser-wasm"]),
        bindingDefinition: "backends/Sdl3/native/Inno.Native.Sdl3/Bindings/bindings.props");

    /// <summary>
    /// Builds the component using the explicitly selected target SDK and validates the complete native product.
    /// </summary>
    /// <param name="context">
    /// The operation-owned checkout, configuration, tools and target.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels and drains active tool processes before publication.
    /// </param>
    /// <returns>
    /// The immutable validated product; a failed or canceled candidate is not published.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The SDK is unassigned or native compilation or export validation fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled before publication.
    /// </exception>
    public static async Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        NativeToolchainSelection selection = context.RequireToolchain();
        if (context.RequireComponentOptions(componentDescriptor).libraryKind != NativeLibraryKind.Shared)
            throw new NotSupportedException("This standalone producer requires shared linkage; static products use the declared aggregate recipe.");
        string source = Path.Combine(context.engineRoot, "extern", "SDL");
        string[] arguments = ["-DSDL_SHARED=ON", "-DSDL_STATIC=OFF", "-DSDL_TESTS=OFF", "-DSDL_EXAMPLES=OFF"];
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, componentDescriptor,
            "sdl3", selection.targetId, [source], arguments);
        return await NativeArtifactPublisher.PublishAsync(context, recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    source, "SDL3-shared", arguments, token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*SDL3" + selection.sharedLibraryExtension);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(library), scoped.configuration);
                File.Copy(library, Path.Combine(output, name));

            }, cancellationToken).ConfigureAwait(false);
    }
}
