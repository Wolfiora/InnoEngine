using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes the host compiler, SDK, build executables and child environment before native publication.
/// </summary>
public sealed class HostNativeToolchain
{
    private static readonly string[] AmbientCompilerVariables =
    [
        "CL", "_CL_", "LINK", "_LINK_", "CFLAGS", "CXXFLAGS", "CPPFLAGS", "LDFLAGS",
        "CPATH", "C_INCLUDE_PATH", "CPLUS_INCLUDE_PATH", "LIBRARY_PATH",
        "CMAKE_GENERATOR", "CMAKE_GENERATOR_PLATFORM", "CMAKE_GENERATOR_TOOLSET", "CMAKE_GENERATOR_INSTANCE"
    ];

    private readonly IReadOnlyDictionary<string, string> m_tools;

    private HostNativeToolchain(
        string targetId,
        Dictionary<string, string> tools,
        Dictionary<string, string> environment,
        IEnumerable<string> inputs,
        IEnumerable<string> cmakeArguments
    ) {
        this.targetId = targetId;
        m_tools = new ReadOnlyDictionary<string, string>(tools);
        this.environment = new ReadOnlyDictionary<string, string>(environment);
        inputPaths = Array.AsReadOnly(inputs.Distinct(StringComparer.Ordinal).ToArray());
        this.cmakeArguments = Array.AsReadOnly(cmakeArguments.ToArray());
        declarations = Array.AsReadOnly(environment.OrderBy(static entry => entry.Key, StringComparer.Ordinal)
            .Select(static entry => entry.Key + "=" + entry.Value)
            .Concat(tools.Select(static entry => entry.Key + "=" + entry.Value))
            .Concat(this.cmakeArguments).Prepend(targetId).ToArray());
    }

    /// <summary>
    /// Gets the native ABI selected from the operating system and process architecture.
    /// </summary>
    public string targetId { get; }

    /// <summary>
    /// Gets compiler, SDK and tool files whose bytes participate in native product identity.
    /// </summary>
    public IReadOnlyList<string> inputPaths { get; }

    /// <summary>
    /// Gets the frozen compiler and SDK selection used alongside file fingerprints.
    /// </summary>
    public IReadOnlyList<string> declarations { get; }

    /// <summary>
    /// Gets configuration arguments that pin CMake to the selected compiler and SDK.
    /// </summary>
    public IReadOnlyList<string> cmakeArguments { get; }

    /// <summary>
    /// Gets variables applied only to owned child processes, without changing the parent environment.
    /// Ambient compiler flags, include paths and CMake generator overrides are cleared; declared inputs select the build.
    /// </summary>
    public IReadOnlyDictionary<string, string> environment { get; }

    /// <summary>
    /// Resolves an executable from this operation's frozen selection.
    /// </summary>
    /// <param name="name">
    /// The compiler or build command selected during discovery.
    /// </param>
    /// <returns>
    /// The absolute executable path; unsupported commands fail instead of consulting a changed PATH.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The command is absent from this toolchain.
    /// </exception>
    public string ResolveExecutable(string name) => m_tools.TryGetValue(name, out string? executable)
        ? executable
        : throw new InvalidOperationException($"Host toolchain '{targetId}' has no '{name}' executable.");

    /// <summary>
    /// Selects the host tools once, or preserves the selection already attached to the build context.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose native components will share this selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels SDK and compiler discovery processes.
    /// </param>
    /// <returns>
    /// A new context carrying an immutable selection, or the already resolved context.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// This host ABI has no configured toolchain.
    /// </exception>
    /// <exception cref="IOException">
    /// A required compiler, SDK or build executable is unavailable.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Tool discovery fails or returns an incomplete installation.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Discovery was canceled.
    /// </exception>
    public static async Task<NativeBuildContext> ResolveAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        if (context.hostToolchain is not null)
            return context;

        var tools = new Dictionary<string, string>(StringComparer.Ordinal);
        var environment = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string variable in AmbientCompilerVariables)
            environment[variable] = string.Empty;
        var inputs = new List<string>();
        var cmakeArguments = new List<string>();
        Architecture architecture = RuntimeInformation.ProcessArchitecture;
        string target;
        if (OperatingSystem.IsWindows() && architecture == Architecture.X64)
        {
            target = "windows-x64";
            string locator = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Microsoft Visual Studio", "Installer", "vswhere.exe");
            string installation = (await ToolchainEnvironment.CaptureOutputAsync(locator,
                ["-latest", "-products", "*", "-requires", "Microsoft.VisualStudio.Component.VC.Tools.x86.x64",
                    "-property", "installationPath"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
            if (!Directory.Exists(installation))
                throw new InvalidOperationException("No Visual Studio installation with the x64 C++ tools was found.");
            string setup = Path.Combine(installation, "Common7", "Tools", "VsDevCmd.bat");
            var query = new ProcessStartInfo(Path.Combine(Environment.SystemDirectory, "cmd.exe"))
            {
                Arguments = $"/d /s /c \"call \"{setup}\" -arch=x64 -host_arch=x64 >nul && set\""
            };
            string output = await ToolchainEnvironment.CaptureOutputAsync(query, cancellationToken).ConfigureAwait(false);
            var selected = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string line in output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
            {
                int separator = line.IndexOf('=');
                if (separator > 0)
                    selected[line[..separator]] = line[(separator + 1)..];
            }
            foreach (string name in new[] { "PATH", "INCLUDE", "LIB", "LIBPATH", "VCToolsInstallDir",
                "VCToolsVersion", "WindowsSdkDir", "WindowsSDKVersion" })
                environment[name] = RequireVariable(selected, name);
            string compilerDirectory = Path.Combine(environment["VCToolsInstallDir"], "bin", "Hostx64", "x64");
            tools["cl"] = Path.Combine(compilerDirectory, "cl.exe");
            tools["msbuild"] = Path.Combine(installation, "MSBuild", "Current", "Bin", "MSBuild.exe");
            tools["cmake"] = Path.Combine(installation, "Common7", "IDE", "CommonExtensions", "Microsoft",
                "CMake", "CMake", "bin", "cmake.exe");
            inputs.AddRange([locator, setup, compilerDirectory, Path.GetDirectoryName(tools["msbuild"])!]);
            foreach (string name in new[] { "INCLUDE", "LIB" })
                inputs.AddRange(environment[name].Split(';', StringSplitOptions.RemoveEmptyEntries));
            cmakeArguments.AddRange([
                "-DCMAKE_GENERATOR_INSTANCE=" + installation,
                "-T", "version=" + environment["VCToolsVersion"],
                "-DCMAKE_SYSTEM_VERSION=" + environment["WindowsSDKVersion"].TrimEnd('\\', '/')]);
        }
        else if (OperatingSystem.IsMacOS() && architecture == Architecture.Arm64)
        {
            target = "osx-arm64";
            string xcrun = ToolchainEnvironment.ResolveExecutable("xcrun");
            tools["clang"] = (await ToolchainEnvironment.CaptureOutputAsync(xcrun,
                ["--find", "clang"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
            tools["clang++"] = (await ToolchainEnvironment.CaptureOutputAsync(xcrun,
                ["--find", "clang++"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
            string sdk = (await ToolchainEnvironment.CaptureOutputAsync(xcrun,
                ["--sdk", "macosx", "--show-sdk-path"], context.engineRoot, cancellationToken).ConfigureAwait(false)).Trim();
            environment["SDKROOT"] = sdk;
            inputs.AddRange([xcrun, Path.Combine(sdk, "usr", "include"), Path.Combine(sdk, "usr", "lib")]);
            cmakeArguments.AddRange(["-DCMAKE_OSX_SYSROOT=" + sdk, "-DCMAKE_OSX_ARCHITECTURES=arm64"]);
            SelectUnixTools(tools, environment, cmakeArguments);
        }
        else if (OperatingSystem.IsLinux() && architecture is Architecture.X64 or Architecture.Arm64)
        {
            target = architecture == Architecture.X64 ? "linux-x64" : "linux-arm64";
            tools["clang"] = ToolchainEnvironment.ResolveExecutable("clang");
            tools["clang++"] = ToolchainEnvironment.ResolveExecutable("clang++");
            tools["gcc"] = ToolchainEnvironment.ResolveExecutable("gcc");
            tools["g++"] = ToolchainEnvironment.ResolveExecutable("g++");
            SelectUnixTools(tools, environment, cmakeArguments);
        }
        else
            throw new PlatformNotSupportedException($"No host native toolchain is defined for {RuntimeInformation.OSDescription}/{architecture}.");

        if (!tools.ContainsKey("cmake"))
            tools["cmake"] = ToolchainEnvironment.ResolveExecutable("cmake");
        string cmakeRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(tools["cmake"])!, ".."));
        string modules = Path.Combine(cmakeRoot, "share");
        if (Directory.Exists(modules))
            inputs.Add(modules);
        inputs.AddRange(tools.Values);
        foreach (string input in inputs)
            if (!File.Exists(input) && !Directory.Exists(input))
                throw new FileNotFoundException("A selected native toolchain input is unavailable.", input);
        return context.WithHostToolchain(new(target, tools, environment, inputs, cmakeArguments));
    }

    private static void SelectUnixTools(
        Dictionary<string, string> tools,
        Dictionary<string, string> environment,
        List<string> cmakeArguments
    ) {
        tools["make"] = ToolchainEnvironment.ResolveExecutable("make");
        environment["CC"] = tools.GetValueOrDefault("gcc", tools["clang"]);
        environment["CXX"] = tools.GetValueOrDefault("g++", tools["clang++"]);
        cmakeArguments.AddRange(["-DCMAKE_C_COMPILER=" + environment["CC"],
            "-DCMAKE_CXX_COMPILER=" + environment["CXX"]]);
    }

    private static string RequireVariable(
        IReadOnlyDictionary<string, string> selected,
        string name
    ) => selected.TryGetValue(name, out string? value) && !string.IsNullOrWhiteSpace(value)
        ? value
        : throw new InvalidOperationException($"The selected C++ toolchain did not define '{name}'.");
}
