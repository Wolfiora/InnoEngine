using System;
using System.IO;
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
        bindingConfig: "backends/ImGui/native/Inno.Native.ImGui/Bindings/bindgen.json");

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
            throw new NotSupportedException("This presentation recipe supports shared linkage only.");
        string source = componentDescriptor.GetToolchainRoot(context.engineRoot);
        string upstream = Path.Combine(context.engineRoot, "extern", "cimgui");
        string[] arguments = ["-DINNO_CIMGUI_SOURCE_DIR=" + upstream, "-DBUILD_SHARED_LIBS=ON", "-DCIMGUI_VARGS0=ON"];
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, componentDescriptor,
            "cimgui", selection.targetId, [upstream, Path.Combine(source, "CMakeLists.txt")], arguments);
        return await NativeArtifactPublisher.PublishAsync(context, recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    CimguiSourceOverlay.Prepare(scoped, upstream), "cimgui", arguments, token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*cimgui-" + scoped.configuration + selection.sharedLibraryExtension);
                string name = ToolchainEnvironment.NormalizeOutputName(Path.GetFileName(library), scoped.configuration);
                File.Copy(library, Path.Combine(output, name));
                if (selection.sharedLibraryExtension == ".dll")
                {
                    string import = NativeCMakeExecutor.FindOutput(scoped, build, "*cimgui-" + scoped.configuration + ".lib");
                    string link = Path.Combine(output, "Link");
                    Directory.CreateDirectory(link);
                    File.Copy(import, Path.Combine(link, Path.GetFileName(import)));
                }
            }, cancellationToken).ConfigureAwait(false);
    }
}
