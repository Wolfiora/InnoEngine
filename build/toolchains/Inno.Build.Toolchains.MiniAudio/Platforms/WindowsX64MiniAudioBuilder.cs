using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.MiniAudio.Platforms;

internal sealed class WindowsX64MiniAudioBuilder : MiniAudioBuilder
{
    private const string OUTPUT_PLATFORM = "windows-x64";
    private const string BUILD_DIR_NAME = "windows-x64";
    private const string GENERATOR = "Visual Studio 17 2022";
    private const string PLATFORM = "x64";

    /// <summary>
    /// Gets the Windows x64 runtime identifier produced by this builder.
    /// </summary>
    public override string OutputPlatform => OUTPUT_PLATFORM;

    /// <summary>
    /// Determines whether the current process is running on Windows x64.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when this builder can execute on the current host.
    /// </returns>
    public override bool IsSupported()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && RuntimeInformation.ProcessArchitecture == Architecture.X64;
    }

    /// <summary>
    /// Builds the pinned miniaudio source as a Windows x64 dynamic library.
    /// </summary>
    /// <param name="miniAudioDirectory">
    /// The absolute path of the validated miniaudio source checkout.
    /// </param>
    /// <param name="context">
    /// The selected checkout and native configuration.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the native process tree.
    /// </param>
    /// <returns>
    /// Completion after native compilation succeeds; failures and cancellation propagate.
    /// </returns>
    public override async Task BuildAsync(
        string miniAudioDirectory,
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string config = context.configuration;
        string buildDirectory = Path.Combine(
            context.GetNativeBuildRoot(typeof(MiniAudioToolchain).Assembly),
            BUILD_DIR_NAME,
            config);
        string buildType = GetBuildType(config);
        string commonOptions = GetCommonCMakeOptions("/DMA_DLL");

        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"-S . -B \"{buildDirectory}\" -G \"{GENERATOR}\" -A {PLATFORM} {commonOptions}",
            miniAudioDirectory, cancellationToken);
        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"--build \"{buildDirectory}\" --config {buildType}",
            miniAudioDirectory, cancellationToken);
    }
}
