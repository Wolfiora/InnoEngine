using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo.Platforms;

internal sealed class WindowsX64CImguizmoBuilder : CImguizmoBuilder
{
    private const string OUTPUT_PLATFORM = "windows-x64";
    private const string BUILD_DIR_NAME = "windows-x64";
    private const string THIRD_PARTY_WARNING_POLICY = "/WX /wd4996 /wd4273";

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
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            && RuntimeInformation.ProcessArchitecture == Architecture.X64;
    }

    /// <summary>
    /// Builds a validated result from the current immutable input snapshot.
    /// </summary>
    /// <param name="cimguizmoDir">
    /// The cimguizmo dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiDir">
    /// The cimgui dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiBuildDir">
    /// The cimgui build dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiOutputDir">
    /// The cimgui output dir text validated by the build operation.
    /// </param>
    /// <param name="config">
    /// The validated configuration that controls this operation.
    /// </param>
    public override void Build(string cimguizmoDir, string cimguiDir, string cimguiBuildDir, string cimguiOutputDir, string config)
    {
        var buildDir = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.BUILD_DIR_NAME, BUILD_DIR_NAME);
        Directory.CreateDirectory(buildDir);

        var cimguizmoCpp = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.CIMGUIMO_CPP_FILE);
        var imguizmoCpp = Path.Combine(cimguizmoDir, CImguizmoBuildConstants.IMGUIZMO_DIR_NAME, CImguizmoBuildConstants.IMGUIZMO_CPP_FILE);
        var cimguiLib = FindCimguiImportLibrary(cimguiBuildDir, config);
        var outputLib = Path.Combine(buildDir, $"{CImguizmoBuildConstants.OUTPUT_DLL_NAME}.dll");

        var includes = new[]
        {
            cimguizmoDir,
            Path.Combine(cimguizmoDir, CImguizmoBuildConstants.IMGUIZMO_DIR_NAME),
            cimguiDir,
            Path.Combine(cimguiDir, "imgui"),
        };

        var includeArgs = string.Join(" ", includes.Select(path => $"/I\"{path}\""));
        var cflags = config == ToolchainLayout.C_DEBUG_CONFIGURATION ? "/Od /Zi" : "/O2";
        // The published C wrapper retains upstream's deprecated SetID, and importing Dear ImGui also marks
        // locally defined ImGuizmo functions as imported through the shared IMGUI_API annotation.
        // Ignore these two third-party diagnostics while treating other compiler warnings as errors.
        var args = $"/LD {cflags} {THIRD_PARTY_WARNING_POLICY} /DIMGUI_API=__declspec(dllimport) {includeArgs} \"{cimguizmoCpp}\" \"{imguizmoCpp}\" /link /OUT:\"{outputLib}\" \"{cimguiLib}\"";

        ToolchainEnvironment.Run("cl", args, buildDir);
    }

    private static string FindCimguiImportLibrary(string cimguiBuildDir, string config)
    {
        if (!Directory.Exists(cimguiBuildDir))
        {
            throw new DirectoryNotFoundException($"cimgui build directory not found: {cimguiBuildDir}");
        }

        string buildType = config == ToolchainLayout.C_DEBUG_CONFIGURATION ? "Debug" : "Release";
        string importLibrary = Path.Combine(cimguiBuildDir, buildType, $"libcimgui-{config}.lib");
        return File.Exists(importLibrary)
            ? importLibrary
            : throw new FileNotFoundException($"cimgui {config} import library not found: {importLibrary}", importLibrary);
    }
}
