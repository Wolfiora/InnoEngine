using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.MiniAudio.Platforms;

internal sealed class LinuxMiniAudioBuilder : MiniAudioBuilder
{
    /// <summary>
    /// Gets the output platform text used by the current instance.
    /// </summary>
    public override string OutputPlatform => RuntimeInformation.ProcessArchitecture switch
    {
        Architecture.X64 => "linux-x64",
        Architecture.Arm64 => "linux-arm64",
        _ => throw new PlatformNotSupportedException("miniaudio supports Linux x64 and ARM64 hosts.")
    };

    /// <summary>
    /// Determines whether the current host can execute this implementation.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the documented condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool IsSupported() =>
        OperatingSystem.IsLinux() &&
        RuntimeInformation.ProcessArchitecture is Architecture.X64 or Architecture.Arm64;

    /// <summary>
    /// Compiles the component sources using the selected checkout and configuration.
    /// </summary>
    /// <param name="miniAudioDirectory">
    /// The mini audio directory text validated by the build operation.
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
            OutputPlatform,
            config);
        string buildType = GetBuildType(config);
        string commonOptions = GetCommonCMakeOptions("-DMA_DLL");

        await ToolchainEnvironment.RunAsync(context,
            "cmake",
            $"-S . -B \"{buildDirectory}\" -DCMAKE_BUILD_TYPE={buildType} {commonOptions}",
            miniAudioDirectory, cancellationToken);
        await ToolchainEnvironment.RunAsync(context,
            "cmake",
            $"--build \"{buildDirectory}\" --config {buildType}",
            miniAudioDirectory, cancellationToken);
    }
}
