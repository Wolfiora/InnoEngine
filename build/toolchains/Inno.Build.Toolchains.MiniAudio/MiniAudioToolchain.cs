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
        MiniAudioBuilder builder = MiniAudioBuilderFactory.CreateForCurrentPlatform();
        string repositoryRoot = context.engineRoot;
        string miniAudioDirectory = Path.Combine(
            repositoryRoot,
            ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME,
            MiniAudioBuildConstants.MINIAUDIO_DIR_NAME);
        string outputDirectory = Path.Combine(
            repositoryRoot,
            ToolchainLayout.C_OUTPUT_DIRECTORY_NAME,
            MiniAudioBuildConstants.OUTPUT_PRODUCT_DIR_NAME,
            builder.OutputPlatform);

        MiniAudioBuildUtils.ValidateSource(miniAudioDirectory);
        Directory.CreateDirectory(outputDirectory);
        string expectedOutput = Path.Combine(
            outputDirectory,
            GetExpectedOutputFileName(builder.OutputPlatform, configuration));
        await builder.BuildAsync(miniAudioDirectory, context, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        CopyArtifacts(
            outputDirectory,
            builder.OutputPlatform,
            context);
        if (!File.Exists(expectedOutput))
        {
            throw new FileNotFoundException(
                "The miniaudio build completed without producing the required shared library.",
                expectedOutput);
        }

        Console.WriteLine($"miniaudio build complete. Output: {outputDirectory}");
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
