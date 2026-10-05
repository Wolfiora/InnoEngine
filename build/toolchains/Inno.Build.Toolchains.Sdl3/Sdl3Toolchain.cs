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
        var builder = Sdl3BuilderFactory.CreateForCurrentPlatform();
        string sdlDir = Path.Combine(context.engineRoot, "extern", "SDL");
        Sdl3BuildUtils.ValidateSource(sdlDir);
        return await NativeArtifactPublisher.PublishAsync(context, typeof(Sdl3Toolchain).Assembly,
            "sdl3", builder.OutputPlatform, [sdlDir], [], async (
                scoped,
                output,
                token
            ) => {
                await builder.BuildAsync(sdlDir, scoped, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                CopyArtifacts(output, builder.OutputPlatform, scoped);
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
