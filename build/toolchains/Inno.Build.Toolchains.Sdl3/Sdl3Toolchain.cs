using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains.Sdl3.Platforms;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Sdl3;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class Sdl3Toolchain
{
    private static readonly string[] LIBRARY_TOKENS = ["SDL3"];
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
        var builder = Sdl3BuilderFactory.CreateForCurrentPlatform();
        var repoRoot = context.engineRoot;
        var externDir = Path.Combine(repoRoot, ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME);
        var sdlDir = Path.Combine(externDir, Sdl3BuildConstants.SDL_DIR_NAME);
        var outputDir = Path.Combine(repoRoot, ToolchainLayout.C_OUTPUT_DIRECTORY_NAME, Sdl3BuildConstants.OUTPUT_PRODUCT_DIR_NAME, builder.OutputPlatform);

        Directory.CreateDirectory(externDir);
        Directory.CreateDirectory(outputDir);

        Sdl3BuildUtils.ValidateSource(sdlDir);

        await builder.BuildAsync(sdlDir, context, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        CopyArtifacts(outputDir, builder.OutputPlatform, context);

        Console.WriteLine($"SDL3 build complete. Output: {outputDir}");
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

        BuildArtifactCopier.CopyArtifacts(context.GetNativeBuildRoot(typeof(Sdl3Toolchain).Assembly), outputDir, config, options);
    }

    private static string NormalizeOutputName(
        string fileName,
        string config
    ) {
        var ext = Path.GetExtension(fileName);
        return $"{Sdl3BuildConstants.OUTPUT_DLL_NAME}-{config}{ext}";
    }
}
