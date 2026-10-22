using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGui;

/// <summary>
/// Owns this backend's native recipe while SDK selection remains with the platform provider.
/// </summary>
public static class ImGuiToolchain
{
    /// <summary>
    /// Gets the component's sole source, binding and intermediate owner.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "cimgui",
        "backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj",
        "backends/ImGui/build/Inno.Build.Toolchains.ImGui/Inno.Build.Toolchains.ImGui.csproj",
        bindingDefinition: "backends/ImGui/native/Inno.Native.ImGui/Bindings/bindings.props");

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
        string upstream = Path.Combine(context.engineRoot, "extern", "cimgui");
        string[] arguments = ["-DINNO_CIMGUI_SOURCE_DIR=" + upstream, "-DBUILD_SHARED_LIBS=ON", "-DCIMGUI_VARGS0=ON"];
        NativeBuildRecipe recipe = CreateRecipe(context);
        return await NativeArtifactPublisher.PublishAsync(context, recipe,
            async (
                @scoped,
                output,
                token
            ) => {
                NativeCMakeSource sources = await NativeCMakeExecutor.PrepareSourceAsync(scoped, componentDescriptor, token).ConfigureAwait(false);
                string windowSource = CimguiSourceOverlay.Prepare(scoped, sources.ResolvePath(upstream));
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    componentDescriptor.GetToolchainRoot(scoped.engineRoot), "cimgui",
                    [.. arguments, "-DINNO_IMGUI_WINDOW_SOURCE=" + windowSource], token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*cimgui-" + scoped.configuration + selection.sharedLibraryExtension);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(library), scoped.configuration);
                File.Copy(library, Path.Combine(output, name));
                if (selection.linkPlan.importLibraryExtension is string importExtension)
                {
                    string import = NativeCMakeExecutor.FindOutput(scoped, build, "*cimgui-" + scoped.configuration + importExtension);
                    string link = Path.Combine(output, "Link");
                    Directory.CreateDirectory(link);
                    File.Copy(import, Path.Combine(link, Path.GetFileName(import)));
                }
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
    /// Declares the selected source, tools and parameters without generating bindings or compiling native code.
    /// </summary>
    /// <param name="context">
    /// The operation with a frozen target SDK and explicit shared component options.
    /// </param>
    /// <returns>
    /// The source recipe; publication adds the selected binding outputs before computing artifact identity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The target SDK or component options have not been assigned.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The component requests a linkage mode this recipe cannot produce.
    /// </exception>
    public static NativeBuildRecipe CreateRecipe(NativeBuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        NativeToolchainSelection selection = context.RequireToolchain();
        if (context.RequireComponentOptions(componentDescriptor).libraryKind != NativeLibraryKind.Shared)
            throw new NotSupportedException("This presentation recipe supports shared linkage only.");
        string source = componentDescriptor.GetToolchainRoot(context.engineRoot);
        string upstream = Path.Combine(context.engineRoot, "extern", "cimgui");
        return NativeBuildRecipe.CreateForComponent(context, componentDescriptor,
            "cimgui", selection.targetId, [upstream, Path.Combine(source, "CMakeLists.txt")],
            ["-DINNO_CIMGUI_SOURCE_DIR=" + upstream, "-DBUILD_SHARED_LIBS=ON", "-DCIMGUI_VARGS0=ON", "window-source=continuous-background"],
            [Path.Combine(source, "CimguiSourceOverlay.cs"), Path.Combine(source, "ImGuiToolchain.cs")]);
    }
}
