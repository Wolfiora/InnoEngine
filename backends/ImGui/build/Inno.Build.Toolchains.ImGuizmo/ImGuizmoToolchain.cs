using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class ImGuizmoToolchain
{
    /// <summary>
    /// Gets the unique native and recipe owners used for sources and target-scoped intermediates.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "cimguizmo",
        "backends/ImGui/native/Inno.Native.ImGuizmo/Inno.Native.ImGuizmo.csproj",
        "backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/Inno.Build.Toolchains.ImGuizmo.csproj",
        bindingConfig: "backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/bindgen.json");

    /// <summary>
    /// Builds and installs the component for the current native host.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose sources and outputs belong to this operation.
    /// </param>
    /// <param name="imGui">
    /// The matching published ImGui product providing the native link dependency.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A native build process fails.
    /// </exception>
    /// <param name="cancellationToken">
    /// Cancels child processes and prevents artifact installation after cancellation.
    /// </param>
    /// <returns>
    /// The validated gizmo library linked against the explicitly supplied ImGui product.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        NativeBuildProduct imGui,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(imGui);
        NativeToolchainSelection selection = context.RequireToolchain();
        if (context.RequireComponentOptions(componentDescriptor).libraryKind != NativeLibraryKind.Shared)
            throw new NotSupportedException("This presentation recipe supports shared linkage only.");
        if (imGui.component != "cimgui" || imGui.targetId != selection.targetId)
            throw new ArgumentException("The supplied ImGui product does not match this gizmo target.", nameof(imGui));
        string extension = selection.sharedLibraryExtension == ".dll" ? ".lib" : selection.sharedLibraryExtension;
        string[] libraries = imGui.files.Where(file => Path.GetExtension(file) == extension).ToArray();
        if (libraries.Length != 1)
            throw new InvalidOperationException("The ImGui product must provide exactly one link library.");
        string cimguizmoDir = Path.Combine(context.engineRoot, "extern", "cimguizmo");
        string cimguiDir = Path.Combine(context.engineRoot, "extern", "cimgui");
        CImguizmoBuildUtils.ValidateSource(cimguizmoDir, cimguiDir);
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, componentDescriptor,
            "cimguizmo", selection.targetId,
            [cimguizmoDir, cimguiDir, libraries[0], Path.Combine(componentDescriptor.GetToolchainRoot(context.engineRoot), "CMakeLists.txt")],
            [imGui.fingerprint]);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    componentDescriptor.GetToolchainRoot(scoped.engineRoot), "cimguizmo",
                    ["-DINNO_CIMGUI_SOURCE_DIR=" + cimguiDir,
                        "-DINNO_CIMGUI_LIBRARY=" + libraries[0]], token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*cimguizmo" + selection.sharedLibraryExtension);
                string name = "libcimguizmo-" + scoped.configuration + selection.sharedLibraryExtension;
                File.Copy(library, Path.Combine(output, name));
                token.ThrowIfCancellationRequested();

            }, cancellationToken).ConfigureAwait(false);
    }

}
