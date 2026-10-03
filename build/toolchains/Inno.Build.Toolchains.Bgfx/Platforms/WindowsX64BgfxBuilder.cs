using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Bgfx.Platforms;

internal sealed class WindowsX64BgfxBuilder : BgfxBuilder
{
    /// <summary>
    /// The output platform value used as part of this type's public representation.
    /// </summary>
    public const string OUTPUT_PLATFORM = "windows-x64";
    private const string DEBUG_TARGET = "vs2022-debug64";
    private const string RELEASE_TARGET = "vs2022-release64";
    private const string GENIE_RELATIVE_PATH = @"..\bx\tools\bin\windows\genie.exe";
    private const string VS2022_SOLUTION_RELATIVE_PATH = @".build\projects\vs2022\bgfx.sln";
    private const string PLATFORM = "x64";

    /// <summary>
    /// Gets the native platform identifier produced by this builder.
    /// </summary>
    public override string outputPlatform => OUTPUT_PLATFORM;
    /// <summary>
    /// Gets the artifact path token text used by the current instance.
    /// </summary>
    public override string artifactPathToken => "/win64_vs2022/bin/";
    /// <summary>
    /// Gets the native make target used for debug output.
    /// </summary>
    protected override string debugMakeTarget => DEBUG_TARGET;
    /// <summary>
    /// Gets the native make target used for optimized output.
    /// </summary>
    protected override string releaseMakeTarget => RELEASE_TARGET;

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
    /// <param name="bgfxDir">
    /// The bgfx dir text validated by the build operation.
    /// </param>
    /// <param name="context">
    /// The selected checkout and native configuration.
    /// </param>
    /// <param name="makeTargetOverride">
    /// The make target override text validated by the build operation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the native process tree.
    /// </param>
    /// <returns>
    /// Completion after native compilation succeeds; failures and cancellation propagate.
    /// </returns>
    public override async Task BuildAsync(
        string bgfxDir,
        NativeBuildContext context,
        string? makeTargetOverride,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string config = context.configuration;
        if (!string.IsNullOrWhiteSpace(makeTargetOverride))
        {
            await ToolchainEnvironment.RunAsync("make", makeTargetOverride, bgfxDir, cancellationToken);
            return;
        }

        await RunGenieAsync(bgfxDir, "--with-shared-lib", cancellationToken);
        await RunMsBuildAsync(bgfxDir, config, cancellationToken);
    }

    /// <summary>
    /// Builds the native offline tools required by the selected configuration.
    /// </summary>
    /// <param name="bgfxDir">
    /// The bgfx dir text validated by the build tools operation.
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
    public override async Task BuildToolsAsync(
        string bgfxDir,
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string config = context.configuration;
        await RunGenieAsync(bgfxDir, "--with-tools --with-shared-lib", cancellationToken);
        await RunMsBuildAsync(bgfxDir, config, cancellationToken);
    }

    private static async Task RunGenieAsync(
        string bgfxDir,
        string args,
        CancellationToken cancellationToken
    ) {
        await ToolchainEnvironment.RunAsync(Path.GetFullPath(Path.Combine(bgfxDir, GENIE_RELATIVE_PATH)), $"{args} vs2022", bgfxDir, cancellationToken);
    }

    private static async Task RunMsBuildAsync(
        string bgfxDir,
        string config,
        CancellationToken cancellationToken
    ) {
        var vsConfig = config == ToolchainLayout.C_DEBUG_CONFIGURATION ? "Debug" : "Release";
        var args = $"{VS2022_SOLUTION_RELATIVE_PATH} /m:1 /nodeReuse:false /p:Configuration={vsConfig} /p:Platform={PLATFORM}";
        await ToolchainEnvironment.RunAsync("msbuild", args, bgfxDir, cancellationToken);
    }
}
