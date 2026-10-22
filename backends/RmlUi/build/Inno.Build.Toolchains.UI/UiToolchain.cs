using System;
using System.IO;
using System.Linq;
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
            ["libinno-ui", "librmlui"],
            nativeSourceDirectory: "backends/RmlUi/native/Inno.Native.UI"),
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
        string source = Path.Combine(componentDescriptor.GetNativeRoot(context.engineRoot), "Native");
        NativeBuildRecipe recipe = CreateRecipe(context);
        return await NativeArtifactPublisher.PublishAsync(context, recipe,
            async (
                @scoped,
                output,
                token
            ) => {
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    source, "inno-ui", [], token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*inno-ui" + selection.sharedLibraryExtension);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(library), scoped.configuration);
                File.Copy(library, Path.Combine(output, name));

            }, cancellationToken, describeOutputs: static (
                @scoped,
                output
            ) =>
                Directory.EnumerateFiles(output, "*", SearchOption.AllDirectories).Select(file =>
                    new NativeArtifact(Path.GetRelativePath(output, file), file,
                        Path.GetRelativePath(output, file).StartsWith("Link" + Path.DirectorySeparatorChar, StringComparison.Ordinal)
                            ? NativeArtifactRole.ImportLibrary : NativeArtifactRole.SharedLibrary)).ToArray()).ConfigureAwait(false);
    }

    /// <summary>
    /// Declares handwritten source, SDK and bridge requirements without reading generated output.
    /// </summary>
    /// <param name="context">
    /// The operation with explicit target tools and this component's shared-library options.
    /// </param>
    /// <returns>
    /// The producer's frozen source declaration; the publisher adds selected bridge content and location.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Target tools or component options have not been selected.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The request uses static rather than standalone shared linkage.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The declared build project is absent from the checkout.
    /// </exception>
    public static NativeBuildRecipe CreateRecipe(NativeBuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        NativeToolchainSelection selection = context.RequireToolchain();
        if (context.RequireComponentOptions(componentDescriptor).libraryKind != NativeLibraryKind.Shared)
            throw new NotSupportedException("This standalone producer requires shared linkage; static products use the declared aggregate recipe.");
        return NativeBuildRecipe.CreateForComponent(context, componentDescriptor, "ui", selection.targetId,
            componentDescriptor.staticBuild!.inputPaths.Select(path => Path.GetFullPath(path, context.engineRoot)),
            [], [Path.Combine(componentDescriptor.GetToolchainRoot(context.engineRoot), "UiToolchain.cs")]);
    }
}
