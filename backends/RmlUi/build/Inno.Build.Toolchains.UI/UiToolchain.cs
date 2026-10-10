using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.UI;

/// <summary>
/// Owns this backend's native recipe while SDK selection remains with the platform provider.
/// </summary>
public static class UiToolchain
{
    /// <summary>
    /// Gets the component's sole source, binding and intermediate owner.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "ui",
        "backends/RmlUi/native/Inno.Native.UI/Inno.Native.UI.csproj",
        "backends/RmlUi/build/Inno.Build.Toolchains.UI/Inno.Build.Toolchains.UI.csproj",
        new NativeStaticBuildDefinition(
            "backends/RmlUi/build/Inno.Build.Toolchains.UI/Native/StaticLibraries.cmake",
            ["extern/freetype", "extern/harfbuzz", "extern/RmlUi", "backends/RmlUi/native/Inno.Native.UI/Native/include", "backends/RmlUi/native/Inno.Native.UI/Native/src", "backends/RmlUi/native/Inno.Native.UI/Native/CMakeLists.txt"],
            ["libinno-ui.a", "librmlui.a"],
            ["browser-wasm"]),
        bindingDefinition: "backends/RmlUi/native/Inno.Native.UI/Bindings/bindings.props");

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
        string source = Path.Combine(componentDescriptor.GetNativeRoot(context.engineRoot), "Native");
        string[] arguments = ["-DINNO_UI_BRIDGE_ROOT=" + context.RequireBindings(componentDescriptor).bridgeDirectory];
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, componentDescriptor,
            "ui", selection.targetId, [source, Path.Combine(context.engineRoot, "extern", "freetype"), Path.Combine(context.engineRoot, "extern", "harfbuzz"), Path.Combine(context.engineRoot, "extern", "RmlUi")], arguments);
        return await NativeArtifactPublisher.PublishAsync(context, recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    source, "inno-ui", arguments, token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*inno-ui" + selection.sharedLibraryExtension);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(library), scoped.configuration);
                File.Copy(library, Path.Combine(output, name));

            }, cancellationToken).ConfigureAwait(false);
    }
}
