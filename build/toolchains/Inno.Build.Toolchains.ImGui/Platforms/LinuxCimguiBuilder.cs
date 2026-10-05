using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGui.Platforms;

internal sealed class LinuxCimguiBuilder : CimguiBuilder
{
    /// <summary>
    /// Gets the native platform identifier produced by this builder.
    /// </summary>
    public override string outputPlatform => RuntimeInformation.ProcessArchitecture switch
    {
        Architecture.X64 => "linux-x64",
        Architecture.Arm64 => "linux-arm64",
        _ => throw new PlatformNotSupportedException("cimgui supports Linux x64 and ARM64 hosts.")
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
    /// <param name="cimguiDir">
    /// The cimgui dir text validated by the build operation.
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
        string cimguiDir,
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string config = context.configuration;
        string buildDir = Path.Combine(context.GetNativeBuildRoot(typeof(ImGuiToolchain).Assembly), outputPlatform, config);
        string buildType = GetBuildType(config);
        string sourceDir = CimguiSourceOverlay.Prepare(context, cimguiDir);

        await ToolchainEnvironment.RunAsync(context,
            "cmake",
            $"-S \"{sourceDir}\" -B \"{buildDir}\" -DINNO_CIMGUI_SOURCE_DIR=\"{cimguiDir}\" -DCMAKE_BUILD_TYPE={buildType} -DBUILD_SHARED_LIBS=ON -DCIMGUI_VARGS0=ON",
            cimguiDir, cancellationToken);
        await ToolchainEnvironment.RunAsync(context,
            "cmake",
            $"--build \"{buildDir}\" --config {buildType}",
            cimguiDir, cancellationToken);
    }
}
