using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.MiniAudio.Platforms;

internal sealed class OsxArm64MiniAudioBuilder : MiniAudioBuilder
{
    private const string OUTPUT_PLATFORM = "osx-arm64";
    private const string BUILD_DIR_NAME = "osx-arm64";

    /// <summary>
    /// Gets the macOS ARM64 runtime identifier produced by this builder.
    /// </summary>
    public override string OutputPlatform => OUTPUT_PLATFORM;

    /// <summary>
    /// Determines whether the current process is running on macOS ARM64.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when this builder can execute on the current host.
    /// </returns>
    public override bool IsSupported()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            && RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
    }

    /// <summary>
    /// Builds the pinned miniaudio source as a macOS ARM64 dynamic library.
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
        string commonOptions = GetCommonCMakeOptions("-DMA_DLL");

        await ToolchainEnvironment.RunAsync(context,
            "cmake",
            $"-S . -B \"{buildDirectory}\" -DCMAKE_BUILD_TYPE={buildType} -DCMAKE_OSX_ARCHITECTURES=arm64 {commonOptions}",
            miniAudioDirectory, cancellationToken);
        await ToolchainEnvironment.RunAsync(context,
            "cmake",
            $"--build \"{buildDirectory}\" --config {buildType}",
            miniAudioDirectory, cancellationToken);
    }
}
