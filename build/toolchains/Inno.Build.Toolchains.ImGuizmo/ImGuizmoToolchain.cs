using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains.ImGui;
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
    /// Completion after the component has been built and installed in the selected checkout.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string configuration = context.configuration;
        var builder = CImguizmoBuilderFactory.CreateForCurrentPlatform();
        var repoRoot = context.engineRoot;
        var externDir = Path.Combine(repoRoot, ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME);
        var cimguiDir = Path.Combine(externDir, CImguizmoBuildConstants.CIMGUI_DIR_NAME);
        var cimguizmoDir = Path.Combine(externDir, CImguizmoBuildConstants.CIMGUIZMO_DIR_NAME);
        var outputDir = Path.Combine(repoRoot, ToolchainLayout.C_OUTPUT_DIRECTORY_NAME, CImguizmoBuildConstants.OUTPUT_PRODUCT_DIR_NAME, builder.outputPlatform);
        var cimguiOutputDir = Path.Combine(repoRoot, ToolchainLayout.C_OUTPUT_DIRECTORY_NAME, "cimgui", builder.outputPlatform);
        var cimguiBuildDir = Path.Combine(context.GetNativeBuildRoot(typeof(ImGuiToolchain).Assembly), builder.outputPlatform, configuration, "upstream");

        Directory.CreateDirectory(externDir);
        Directory.CreateDirectory(outputDir);

        CImguizmoBuildUtils.ValidateSource(cimguizmoDir, cimguiDir);

        await builder.BuildAsync(cimguizmoDir, cimguiDir, cimguiBuildDir, cimguiOutputDir, context, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        CopyArtifacts(outputDir, builder.outputPlatform, context);

        Console.WriteLine($"cimguizmo build complete. Output: {outputDir}");
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
