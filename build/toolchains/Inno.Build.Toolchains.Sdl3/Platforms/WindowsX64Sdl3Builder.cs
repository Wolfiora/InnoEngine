using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Sdl3.Platforms;

internal sealed class WindowsX64Sdl3Builder : Sdl3Builder
{
    private const string OUTPUT_PLATFORM = "windows-x64";
    private const string BUILD_DIR_NAME = "windows-x64";
    private const string GENERATOR = "Visual Studio 17 2022";
    private const string PLATFORM = "x64";

    /// <summary>
    /// Gets text used for stable identity, presentation, or diagnostics by this contract.
    /// </summary>
    public override string OutputPlatform => OUTPUT_PLATFORM;

    /// <summary>
    /// Determines whether the current host can execute this implementation.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool IsSupported()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && RuntimeInformation.ProcessArchitecture == Architecture.X64;
    }

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
        var buildDir = Path.Combine(context.GetNativeBuildRoot(typeof(Sdl3Toolchain).Assembly), BUILD_DIR_NAME, config);
        var buildType = GetBuildType(config);

        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"-S . -B \"{buildDir}\" -G \"{GENERATOR}\" -A {PLATFORM} -DSDL_SHARED=ON -DSDL_STATIC=OFF -DSDL_TESTS=OFF -DSDL_EXAMPLES=OFF",
            sdlDir, cancellationToken);
        await ToolchainEnvironment.RunAsync(
            "cmake",
            $"--build \"{buildDir}\" --config {buildType} --target SDL3-shared",
            sdlDir, cancellationToken);
    }
}
