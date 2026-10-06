using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains.ImGui.Platforms;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGui;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class ImGuiToolchain
{
    private static readonly string[] LIBRARY_TOKENS = ["cimgui"];
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
    /// The validated product containing the exact runtime and link inputs for this operation.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        context = await HostNativeToolchain.ResolveAsync(context, cancellationToken).ConfigureAwait(false);
        var builder = CimguiBuilderFactory.CreateForCurrentPlatform();
        string cimguiDir = Path.Combine(context.engineRoot, "extern", "cimgui");
        CimguiBuildUtils.ValidateSource(cimguiDir);
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, typeof(ImGuiToolchain).Assembly, "cimgui", builder.outputPlatform, [cimguiDir, Path.Combine(context.engineRoot, "build", "toolchains", "Inno.Build.Toolchains.ImGui", "CMakeLists.txt")], []);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                await builder.BuildAsync(cimguiDir, scoped, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                CopyArtifacts(output, builder.outputPlatform, scoped);
                if (OperatingSystem.IsWindows())
                {
                    string buildType = scoped.configuration == "debug" ? "Debug" : "Release";
                    string library = Path.Combine(scoped.GetNativeBuildRoot(typeof(ImGuiToolchain).Assembly),
                        builder.outputPlatform, scoped.configuration, "upstream", buildType, $"libcimgui-{scoped.configuration}.lib");
                    string link = Path.Combine(output, "Link");
                    Directory.CreateDirectory(link);
                    File.Copy(library, Path.Combine(link, Path.GetFileName(library)));
                }
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
            OperatingSystem.IsWindows() ? [$"libcimgui-{config}"] : LIBRARY_TOKENS,
            SHARED_EXTENSIONS,
            null,
            NormalizeOutputName);

        BuildArtifactCopier.CopyArtifacts(context.GetNativeBuildRoot(typeof(ImGuiToolchain).Assembly), outputDir, config, options);
    }

    private static string NormalizeOutputName(
        string fileName,
        string config
    ) {
        var ext = Path.GetExtension(fileName);
        return OperatingSystem.IsWindows() ? fileName : $"{CimguiBuildConstants.OUTPUT_DLL_NAME}-{config}{ext}";
    }
}
