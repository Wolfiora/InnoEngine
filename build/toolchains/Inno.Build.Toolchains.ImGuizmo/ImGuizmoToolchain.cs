using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains.ImGuizmo.Platforms;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class ImGuizmoToolchain
{
    private static readonly string[] LIBRARY_TOKENS = ["cimguizmo"];
    private static readonly string[] SHARED_EXTENSIONS = { ".dll", ".dylib", ".so" };

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
        context = await HostNativeToolchain.ResolveAsync(context, cancellationToken).ConfigureAwait(false);
        var builder = CImguizmoBuilderFactory.CreateForCurrentPlatform();
        if (imGui.component != "cimgui" || imGui.targetId != builder.outputPlatform)
            throw new ArgumentException("The supplied ImGui product does not match this gizmo target.", nameof(imGui));
        string extension = OperatingSystem.IsWindows() ? ".lib" : OperatingSystem.IsMacOS() ? ".dylib" : ".so";
        string[] libraries = imGui.files.Where(file => Path.GetExtension(file) == extension).ToArray();
        if (libraries.Length != 1)
            throw new InvalidOperationException("The ImGui product must provide exactly one link library.");
        string cimguizmoDir = Path.Combine(context.engineRoot, "extern", "cimguizmo");
        string cimguiDir = Path.Combine(context.engineRoot, "extern", "cimgui");
        CImguizmoBuildUtils.ValidateSource(cimguizmoDir, cimguiDir);
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, typeof(ImGuizmoToolchain).Assembly, "cimguizmo", builder.outputPlatform, [cimguizmoDir, cimguiDir, libraries[0]], [imGui.fingerprint]);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                await builder.BuildAsync(cimguizmoDir, cimguiDir, libraries[0], scoped, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                CopyArtifacts(output, builder.outputPlatform, scoped);
            }, cancellationToken).ConfigureAwait(false);
    }

    private static void CopyArtifacts(
        string outputDir,
        string platform,
        NativeBuildContext context
    ) {
        string config = context.configuration;
        var options = new BuildArtifactOptions(
            Path.Combine(platform, config),
            LIBRARY_TOKENS,
            SHARED_EXTENSIONS,
            null,
            NormalizeOutputName);

        BuildArtifactCopier.CopyArtifacts(context.GetNativeBuildRoot(typeof(ImGuizmoToolchain).Assembly), outputDir, config, options);
    }

    private static string NormalizeOutputName(
        string fileName,
        string config
    ) {
        var ext = Path.GetExtension(fileName);
        return $"{CImguizmoBuildConstants.OUTPUT_DLL_NAME}-{config}{ext}";
    }
}
