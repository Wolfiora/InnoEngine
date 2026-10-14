using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentProcess
{
    internal static string ResolveDotnet(string? selected)
    {
        if (selected is not null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(selected);
            if (Path.IsPathFullyQualified(selected) || selected.Contains(Path.DirectorySeparatorChar)
                || selected.Contains(Path.AltDirectorySeparatorChar))
            {
                string explicitHost = Path.GetFullPath(selected);
                return File.Exists(explicitHost) ? explicitHost
                    : throw new FileNotFoundException("The selected .NET SDK executable is absent.", explicitHost);
            }
            foreach (string directory in (Environment.GetEnvironmentVariable("PATH") ?? string.Empty).Split(Path.PathSeparator))
            {
                if (directory.Length == 0)
                    continue;
                string executable = Path.Combine(directory, selected);
                if (File.Exists(executable))
                    return Path.GetFullPath(executable);
                if (OperatingSystem.IsWindows() && Path.GetExtension(selected).Length == 0 && File.Exists(executable + ".exe"))
                    return Path.GetFullPath(executable + ".exe");
            }
            throw new FileNotFoundException("The selected .NET SDK executable is not on PATH.", selected);
        }
        string? configured = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!string.IsNullOrWhiteSpace(configured) && Path.IsPathFullyQualified(configured) && File.Exists(configured))
            return configured;
        string root = Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory());
        string host = Path.Combine(root, OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        return File.Exists(host) ? host : throw new FileNotFoundException("The active .NET installation has no CLI host.", host);
    }

    internal static async Task<string> RunAsync(
        string executable,
        string[] arguments,
        string root,
        bool capture,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = root,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        // Product builds have their own SDK and project graph, independent of the parent IDE.
        foreach (string name in new[] { "MSBUILD_EXE_PATH", "MSBuildSDKsPath", "MSBuildExtensionsPath",
            "MSBuildExtensionsPath32", "MSBuildExtensionsPath64",
            "DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR", "DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR",
            "DOTNET_MSBUILD_SDK_RESOLVER_SDKS_VER", "MSBUILDENSURESTDOUTFORTASKPROCESSES" })
            start.Environment.Remove(name);
        if (Path.GetFileNameWithoutExtension(executable) == "dotnet")
        {
            start.Environment["DOTNET_HOST_PATH"] = executable;
            start.Environment["DOTNET_ROOT"] = Path.GetDirectoryName(executable)!;
        }
        using var process = new Process { StartInfo = start };
        if (!process.Start())
            throw new InvalidOperationException($"Could not start '{executable}'.");
        Task<string> output = capture ? process.StandardOutput.ReadToEndAsync()
            : ForwardAsync(process.StandardOutput, Console.Out);
        Task<string> errors = ForwardAsync(process.StandardError, Console.Error);
        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            await output.ConfigureAwait(false);
            await errors.ConfigureAwait(false);
            throw;
        }
        string result = await output.ConfigureAwait(false);
        await errors.ConfigureAwait(false);
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"'{executable}' failed with exit code {process.ExitCode}.{Environment.NewLine}{result}");
        return result.Trim();
    }

    private static async Task<string> ForwardAsync(
        StreamReader source,
        TextWriter destination
    ) {
        while (await source.ReadLineAsync().ConfigureAwait(false) is string line)
            await destination.WriteLineAsync(line).ConfigureAwait(false);
        return string.Empty;
    }
}
