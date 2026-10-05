using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo.Platforms;

internal sealed class OsxArm64CImguizmoBuilder : CImguizmoBuilder
{
    private const string OUTPUT_PLATFORM = "osx-arm64";
    private const string BUILD_DIR_NAME = "osx-arm64";
    private const string THIRD_PARTY_WARNING_POLICY = "-Werror -Wno-deprecated-declarations";

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
        var buildDir = Path.Combine(context.GetNativeBuildRoot(typeof(ImGuizmoToolchain).Assembly), BUILD_DIR_NAME, config);
        Directory.CreateDirectory(buildDir);

        var cimguizmoCpp = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.CIMGUIMO_CPP_FILE);
        var imguizmoCpp = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.IMGUIZMO_DIR_NAME, CImguizmoBuildConstants.IMGUIZMO_CPP_FILE);
        var cimguiLib = cimguiLibraryFile;
        var outputLib = Path.Combine(buildDir, $"{CImguizmoBuildConstants.OUTPUT_DLL_NAME}.dylib");

        var includes = new[]
        {
            cimguizmoDir,
            Path.Combine(cimguizmoDir, CImguizmoBuildConstants.IMGUIZMO_DIR_NAME),
            cimguiDir,
            Path.Combine(cimguiDir, "imgui"),
        };

        var includeArgs = string.Join(" ", includes.Select(path => $"-I\"{path}\""));
        var cflags = config == ToolchainLayout.C_DEBUG_CONFIGURATION ? "-O0 -g" : "-O3";
        var rpath = "@loader_path/../../cimgui/osx-arm64";
        var installName = $"@rpath/{CImguizmoBuildConstants.OUTPUT_DLL_NAME}.dylib";
        // The published C wrapper intentionally retains ImGuizmo_SetID for ABI compatibility even though
        // upstream marks the underlying C++ member deprecated. Keep that single third-party diagnostic quiet
        // while treating every other compiler diagnostic enabled by default as an error.
        var args = $"{cflags} {THIRD_PARTY_WARNING_POLICY} -std=c++11 -fPIC -dynamiclib {includeArgs} \"{cimguizmoCpp}\" \"{imguizmoCpp}\" \"{cimguiLib}\" -Wl,-install_name,{installName} -Wl,-rpath,{rpath} -o \"{outputLib}\"";

        await ToolchainEnvironment.RunAsync(context, "clang++", args, cimguizmoDir, cancellationToken);
    }

}
