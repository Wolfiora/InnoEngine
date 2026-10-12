using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Text;

/// <summary>
/// Owns this backend's native recipe while SDK selection remains with the platform provider.
/// </summary>
public static class TextToolchain
{
    /// <summary>
    /// Gets the component's sole source, binding and intermediate owner.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "text",
        "backends/Text/native/Inno.Native.Text/Inno.Native.Text.csproj",
        "backends/Text/build/Inno.Build.Toolchains.Text/Inno.Build.Toolchains.Text.csproj",
        new NativeStaticBuildDefinition(
            "backends/Text/build/Inno.Build.Toolchains.Text/Native/StaticLibraries.cmake",
            ["extern/freetype", "extern/harfbuzz", "backends/Text/native/Inno.Native.Text/Native/include", "backends/Text/native/Inno.Native.Text/Native/src", "backends/Text/native/Inno.Native.Text/Native/CMakeLists.txt"],
            ["libinno-text.a", "libfreetype.a", "libharfbuzz.a"],
            ["browser-wasm"]),
        bindingDefinition: "backends/Text/native/Inno.Native.Text/Bindings/bindings.props");

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
                scoped,
                output,
                token
            ) => {
                string[] arguments = ["-DINNO_TEXT_BRIDGE_ROOT=" + scoped.RequireBindings(componentDescriptor).bridgeDirectory];
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    source, "inno-text", arguments, token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*inno-text" + selection.sharedLibraryExtension);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(library), scoped.configuration);
                File.Copy(library, Path.Combine(output, name));

            }, cancellationToken).ConfigureAwait(false);
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
        return NativeBuildRecipe.CreateForComponent(context, componentDescriptor, "text", selection.targetId,
            componentDescriptor.staticBuild!.inputPaths.Select(path => Path.GetFullPath(path, context.engineRoot)),
            ["-DINNO_TEXT_BRIDGE_ROOT"], [Path.Combine(componentDescriptor.GetToolchainRoot(context.engineRoot), "TextToolchain.cs")]);
    }
}
