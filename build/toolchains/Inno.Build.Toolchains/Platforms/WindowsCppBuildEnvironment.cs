using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Platforms;

internal static class WindowsCppBuildEnvironment
{
    internal static async Task ConfigureAsync(
        ProcessStartInfo start,
        CancellationToken cancellationToken
    ) {
        if (!OperatingSystem.IsWindows() || Path.IsPathFullyQualified(start.FileName))
            return;

        string tool = Path.GetFileNameWithoutExtension(start.FileName);
        if (!tool.Equals("msbuild", StringComparison.OrdinalIgnoreCase)
            && !tool.Equals("cl", StringComparison.OrdinalIgnoreCase))
            return;

        string installerRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "Microsoft Visual Studio", "Installer");
        string locator = Path.Combine(installerRoot, "vswhere.exe");
        if (!File.Exists(locator))
            throw new FileNotFoundException("Visual Studio C++ Build Tools are required for Windows native builds.", locator);

        var query = new ProcessStartInfo(locator);
        foreach (string argument in new[] { "-latest", "-products", "*", "-requires",
            "Microsoft.VisualStudio.Component.VC.Tools.x86.x64", "-property", "installationPath" })
            query.ArgumentList.Add(argument);
        string installation = (await ToolchainEnvironment.CaptureOutputAsync(query, cancellationToken).ConfigureAwait(false)).Trim();
        if (!Directory.Exists(installation))
            throw new InvalidOperationException("No Visual Studio installation with the x64 C++ tools was found.");

        if (tool.Equals("msbuild", StringComparison.OrdinalIgnoreCase))
        {
            start.FileName = Path.Combine(installation, "MSBuild", "Current", "Bin", "MSBuild.exe");
            if (!File.Exists(start.FileName))
                throw new FileNotFoundException("The selected Visual Studio installation has no MSBuild executable.", start.FileName);
            return;
        }

        string setup = Path.Combine(installation, "Common7", "Tools", "VsDevCmd.bat");
        if (!File.Exists(setup))
            throw new FileNotFoundException("The selected Visual Studio installation has no C++ environment initializer.", setup);
        var environmentQuery = new ProcessStartInfo(Path.Combine(Environment.SystemDirectory, "cmd.exe"))
        {
            Arguments = $"/d /s /c \"call \"{setup}\" -arch=x64 -host_arch=x64 >nul && set\""
        };
        string environment = await ToolchainEnvironment.CaptureOutputAsync(environmentQuery, cancellationToken).ConfigureAwait(false);
        foreach (string line in environment.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            int separator = line.IndexOf('=');
            if (separator <= 0)
                continue;
            string name = line[..separator];
            if (name.Equals("PATH", StringComparison.OrdinalIgnoreCase)
                || name.Equals("INCLUDE", StringComparison.OrdinalIgnoreCase)
                || name.Equals("LIB", StringComparison.OrdinalIgnoreCase)
                || name.Equals("LIBPATH", StringComparison.OrdinalIgnoreCase))
                start.Environment[name] = line[(separator + 1)..];
        }

        foreach (string directory in start.Environment["PATH"]!.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            string candidate = Path.Combine(directory, "cl.exe");
            if (!File.Exists(candidate))
                continue;
            start.FileName = candidate;
            return;
        }
        throw new InvalidOperationException("The selected Visual Studio environment did not provide the x64 C++ compiler.");
    }

}
