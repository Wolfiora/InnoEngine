using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Windows;

/// <summary>
/// Resolves the Windows SDK for a declared target without deriving publication policy from this process.
/// </summary>
public sealed class WindowsNativeToolchainProvider : INativeToolchainProvider
{
    private readonly string m_dotnetHost;

    /// <summary>
    /// Captures the explicit managed executable used by Native binding generation.
    /// </summary>
    /// <param name="dotnetHost">
    /// The explicitly selected SDK executable path or command.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The host command is empty.
    /// </exception>
    public WindowsNativeToolchainProvider(string dotnetHost)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        m_dotnetHost = dotnetHost;
    }

    /// <inheritdoc />
    public async ValueTask<NativeToolchainSelection> ResolveAsync(
        NativeBuildContext context,
        BuildHostDescriptor host,
        string targetId,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(host);
        cancellationToken.ThrowIfCancellationRequested();
        if (!(host.system == "Windows" && host.architecture == "x64" && targetId == "windows-x64"))
            throw new PlatformNotSupportedException($"The Windows SDK cannot build '{targetId}' on {host.system}/{host.architecture}.");
        var tools = new Dictionary<string, string>(StringComparer.Ordinal) { ["dotnet"] = ToolchainEnvironment.ResolveExecutable(m_dotnetHost) };
        Dictionary<string, string> environment = NativeToolchainPreparation.CreateEnvironment();
        var inputs = new List<string>();
        var cmakeArguments = new List<string>();
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

        cmakeArguments.InsertRange(0, ["-G", "Visual Studio 17 2022", "-A", "x64"]);
        return NativeToolchainPreparation.Freeze(targetId, host, tools, environment, inputs,
            cmakeArguments, ".dll", true);
    }

    private static string RequireVariable(
        IReadOnlyDictionary<string, string> selected,
        string name
    ) => selected.TryGetValue(name, out string? value) && !string.IsNullOrWhiteSpace(value)
        ? value
        : throw new InvalidOperationException($"The selected C++ toolchain did not define '{name}'.");
}
