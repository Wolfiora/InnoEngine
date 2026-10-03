using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGui.Platforms;

internal sealed class OsxArm64CimguiBuilder : CimguiBuilder
{
    private const string OUTPUT_PLATFORM = "osx-arm64";
    private const string BUILD_DIR_NAME = "osx-arm64";

    /// <summary>
    /// Gets the native platform identifier produced by this builder.
    /// </summary>
    public override string outputPlatform => OUTPUT_PLATFORM;

    /// <summary>
    /// Determines whether the current host can execute this implementation.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool IsSupported()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
            && RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
    }

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
        var buildDir = Path.Combine(context.GetNativeBuildRoot(typeof(ImGuiToolchain).Assembly), BUILD_DIR_NAME, config);
        var buildType = GetBuildType(config);
        string sourceDir = CimguiSourceOverlay.Prepare(context, cimguiDir);

        await ToolchainEnvironment.RunAsync("cmake", $"-S \"{sourceDir}\" -B \"{buildDir}\" -DINNO_CIMGUI_SOURCE_DIR=\"{cimguiDir}\" -DCMAKE_BUILD_TYPE={buildType} -DBUILD_SHARED_LIBS=ON -DCIMGUI_VARGS0=ON", cimguiDir, cancellationToken);
        await ToolchainEnvironment.RunAsync("cmake", $"--build \"{buildDir}\" --config {buildType}", cimguiDir, cancellationToken);
    }
}
