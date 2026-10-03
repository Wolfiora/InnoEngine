using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Sdl3.Platforms;

internal sealed class LinuxSdl3Builder : Sdl3Builder
{
    /// <summary>
    /// Gets the output platform text used by the current instance.
    /// </summary>
    public override string OutputPlatform => RuntimeInformation.ProcessArchitecture switch
    {
        Architecture.X64 => "linux-x64",
        Architecture.Arm64 => "linux-arm64",
        _ => throw new PlatformNotSupportedException("SDL3 supports Linux x64 and ARM64 hosts.")
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
    /// <param name="sdlDir">
    /// The sdl dir text validated by the build operation.
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
        string sdlDir,
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string config = context.configuration;
        string buildDir = Path.Combine(context.GetNativeBuildRoot(typeof(Sdl3Toolchain).Assembly), OutputPlatform, config);
        string buildType = GetBuildType(config);
        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"-S . -B \"{buildDir}\" -DCMAKE_BUILD_TYPE={buildType} -DSDL_SHARED=ON -DSDL_STATIC=OFF -DSDL_TESTS=OFF -DSDL_EXAMPLES=OFF",
            sdlDir, cancellationToken);
        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"--build \"{buildDir}\" --config {buildType} --target SDL3-shared",
            sdlDir, cancellationToken);
    }
}
