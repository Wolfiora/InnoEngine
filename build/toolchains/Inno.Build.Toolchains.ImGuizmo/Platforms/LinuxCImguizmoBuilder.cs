using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo.Platforms;

internal sealed class LinuxCImguizmoBuilder : CImguizmoBuilder
{
    private const string THIRD_PARTY_WARNING_POLICY = "-Werror -Wno-deprecated-declarations";

    /// <summary>
    /// Gets the native platform identifier produced by this builder.
    /// </summary>
    public override string outputPlatform => RuntimeInformation.ProcessArchitecture switch
    {
        Architecture.X64 => "linux-x64",
        Architecture.Arm64 => "linux-arm64",
        _ => throw new PlatformNotSupportedException("cimguizmo supports Linux x64 and ARM64 hosts.")
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
    /// <param name="cimguizmoDir">
    /// The cimguizmo dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiDir">
    /// The cimgui dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiLibraryFile">
    /// The exact published import or shared library selected by the parent operation.
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
        string cimguizmoDir,
        string cimguiDir,
        string cimguiLibraryFile,
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string config = context.configuration;
        string buildDir = Path.Combine(context.GetNativeBuildRoot(typeof(ImGuizmoToolchain).Assembly), outputPlatform, config);
        Directory.CreateDirectory(buildDir);

        string cimguizmoCpp = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.CIMGUIMO_CPP_FILE);
        string imguizmoCpp = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.IMGUIZMO_DIR_NAME, CImguizmoBuildConstants.IMGUIZMO_CPP_FILE);
        string cimguiLib = cimguiLibraryFile;
        string outputLib = Path.Combine(buildDir, $"{CImguizmoBuildConstants.OUTPUT_DLL_NAME}.so");
        string[] includes =
        [
            cimguizmoDir,
            Path.Combine(cimguizmoDir, CImguizmoBuildConstants.IMGUIZMO_DIR_NAME),
            cimguiDir,
            Path.Combine(cimguiDir, "imgui")
        ];
        string includeArgs = string.Join(" ", includes.Select(path => $"-I\"{path}\""));
        string cflags = config == ToolchainLayout.C_DEBUG_CONFIGURATION ? "-O0 -g" : "-O3";
        string relativeRPath = $"$ORIGIN/../../cimgui/{outputPlatform}";
        string args = $"{cflags} {THIRD_PARTY_WARNING_POLICY} -std=c++11 -fPIC -shared {includeArgs} \"{cimguizmoCpp}\" \"{imguizmoCpp}\" \"{cimguiLib}\" -Wl,-soname,{CImguizmoBuildConstants.OUTPUT_DLL_NAME}.so -Wl,-rpath,{relativeRPath} -o \"{outputLib}\"";
        await ToolchainEnvironment.RunAsync(context, "clang++", args, cimguizmoDir, cancellationToken);
    }

}
