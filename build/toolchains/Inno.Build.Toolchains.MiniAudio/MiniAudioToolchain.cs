using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains.MiniAudio.Platforms;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.MiniAudio;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class MiniAudioToolchain
{
    private static readonly string[] S_LIBRARY_TOKENS = ["miniaudio"];
    private static readonly string[] S_SHARED_EXTENSIONS = [".dll", ".dylib", ".so"];

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
        var builder = MiniAudioBuilderFactory.CreateForCurrentPlatform();
        string miniAudioDirectory = Path.Combine(context.engineRoot, "extern", "miniaudio");
        MiniAudioBuildUtils.ValidateSource(miniAudioDirectory);
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, typeof(MiniAudioToolchain).Assembly, "miniaudio", builder.OutputPlatform, [miniAudioDirectory], []);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                await builder.BuildAsync(miniAudioDirectory, scoped, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                CopyArtifacts(output, builder.OutputPlatform, scoped);
                string expected = Path.Combine(output, GetExpectedOutputFileName(builder.OutputPlatform, scoped.configuration));
                if (!File.Exists(expected))
                    throw new FileNotFoundException("The audio build did not produce its required shared library.", expected);
            }, cancellationToken).ConfigureAwait(false);
    }

    private static void CopyArtifacts(
        string outputDirectory,
        string outputPlatform,
        NativeBuildContext context
    ) {
        string config = context.configuration;
        var options = new BuildArtifactOptions(
            Path.Combine(outputPlatform, config),
            S_LIBRARY_TOKENS,
            S_SHARED_EXTENSIONS,
            [$"/{outputPlatform}/{config}/"],
            ToolchainEnvironment.NormalizeOutputName);
        BuildArtifactCopier.CopyArtifacts(context.GetNativeBuildRoot(typeof(MiniAudioToolchain).Assembly), outputDirectory, config, options);
    }

    private static string GetExpectedOutputFileName(
        string outputPlatform,
        string config
    ) {
        return outputPlatform switch
        {
            "osx-arm64" => $"libminiaudio-{config}.dylib",
            "windows-x64" => $"miniaudio-{config}.dll",
            "linux-x64" or "linux-arm64" => $"libminiaudio-{config}.so",
            _ => throw new PlatformNotSupportedException(
                $"No miniaudio output name is defined for '{outputPlatform}'.")
        };
    }
}
