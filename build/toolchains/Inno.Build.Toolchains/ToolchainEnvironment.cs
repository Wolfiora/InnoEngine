using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Inno.Build.Toolchains.Platforms;

namespace Inno.Build.Toolchains;

/// <summary>
/// Provides deterministic host-process and workspace operations shared by native dependency toolchains.
/// </summary>
public static class ToolchainEnvironment
{
    /// <summary>
    /// Runs a child build with structured arguments, cancellation and hidden windows.
    /// </summary>
    /// <param name="fileName">
    /// The executable to launch.
    /// </param>
    /// <param name="arguments">
    /// The individual arguments, passed without shell interpretation.
    /// </param>
    /// <param name="workingDirectory">
    /// The repository or staging directory owned by the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the child process and its complete process tree.
    /// </param>
    /// <param name="environment">
    /// Optional process-local toolchain variables; the parent environment is unchanged.
    /// </param>
    /// <returns>
    /// Completion after both output streams have drained and the process succeeds.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The process fails to start or exits unsuccessfully.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    /// <exception cref="IOException">
    /// Redirected output cannot be read or forwarded; the process tree is terminated before failure returns.
    /// </exception>
    public static Task RunAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string>? environment = null
    ) => RunCoreAsync(fileName, arguments, workingDirectory, cancellationToken, environment, Console.Out);

    /// <summary>
    /// Captures a tool's standard output while forwarding errors and preserving the common process lifecycle.
    /// </summary>
    /// <param name="fileName">
    /// The executable to launch.
    /// </param>
    /// <param name="arguments">
    /// Structured arguments passed without shell interpretation.
    /// </param>
    /// <param name="workingDirectory">
    /// The directory used for project and SDK resolution.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the child process tree and waits for its complete exit.
    /// </param>
    /// <param name="environment">
    /// Optional child-only environment variables.
    /// </param>
    /// <returns>
    /// The complete standard output after successful exit; failures and cancellation propagate.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The tool fails to start or exits unsuccessfully.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    /// <exception cref="IOException">
    /// Redirected output cannot be read or forwarded; the process tree is terminated before failure returns.
    /// </exception>
    public static async Task<string> CaptureOutputAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string>? environment = null
    ) {
        using var output = new StringWriter();
        await RunCoreAsync(fileName, arguments, workingDirectory, cancellationToken, environment, output).ConfigureAwait(false);
        return output.ToString();
    }

    /// <summary>
    /// Validates the configuration shared by all native component builds.
    /// </summary>
    /// <param name="configuration">
    /// The debug or release token supplied by the build workflow.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is unsupported.
    /// </exception>
    public static void ValidateConfiguration(string configuration)
    {
        if (configuration is not (ToolchainLayout.C_DEBUG_CONFIGURATION or ToolchainLayout.C_RELEASE_CONFIGURATION))
            throw new ArgumentException("Native configuration must be 'debug' or 'release'.", nameof(configuration));
    }

    /// <summary>
    /// Resolves the repository containing the currently executing toolchain assembly.
    /// </summary>
    /// <returns>
    /// The absolute path of the repository root.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no repository marker exists above the toolchain assembly directory.
    /// </exception>
    public static string FindRepoRoot()
    {
        string location = typeof(ToolchainEnvironment).Assembly.Location;
        string outputRoot = string.IsNullOrEmpty(location) ? AppContext.BaseDirectory : Path.GetDirectoryName(location)!;
        var dir = new DirectoryInfo(outputRoot);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, ToolchainLayout.C_REPOSITORY_MARKER_FILE)))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not locate repo root (InnoEngine.sln not found).");
    }

    /// <summary>
    /// Runs a child build using a complete argument string, without a shell.
    /// </summary>
    /// <param name="fileName">
    /// The executable resolved by the host operating system.
    /// </param>
    /// <param name="arguments">
    /// Arguments quoted for the selected executable's command-line parser.
    /// </param>
    /// <param name="workingDirectory">
    /// The directory owned by the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the complete process tree and waits for output to finish draining.
    /// </param>
    /// <returns>
    /// Completion after successful exit; failures and cancellation propagate.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The process fails to start or exits unsuccessfully.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    /// <exception cref="IOException">
    /// Redirected output cannot be read or forwarded; the process tree is terminated before failure returns.
    /// </exception>
    public static Task RunAsync(
        string fileName,
        string arguments,
        string workingDirectory,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine($"> {fileName} {arguments}");
        var start = new ProcessStartInfo(fileName)
        {
            Arguments = arguments,
            WorkingDirectory = workingDirectory
        };
        return RunProcessAsync(start, cancellationToken, null, Console.Out);
    }

    /// <summary>
    /// Determines whether a value contains at least one token without regard to casing.
    /// </summary>
    /// <param name="value">
    /// The value searched for candidate tokens.
    /// </param>
    /// <param name="needles">
    /// The candidate tokens tested against the value.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when at least one token occurs in the value; otherwise <see langword="false"/>.
    /// </returns>
    public static bool ContainsAny(
        string value,
        params string[] needles
    ) {
        foreach (var needle in needles)
        {
            if (value.Contains(needle, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Normalizes a native artifact name and appends its build configuration.
    /// </summary>
    /// <param name="fileName">
    /// The source artifact file name.
    /// </param>
    /// <param name="config">
    /// The normalized configuration appended to the artifact stem.
    /// </param>
    /// <returns>
    /// The deterministic output file name.
    /// </returns>
    public static string NormalizeOutputName(
        string fileName,
        string config
    ) {
        var ext = Path.GetExtension(fileName);
        var baseName = Path.GetFileNameWithoutExtension(fileName);
        var trimmed = TrimConfigSuffix(baseName);
        return $"{trimmed}-{config}{ext}";
    }

    /// <summary>
    /// Removes a trailing native debug or release token from an artifact stem.
    /// </summary>
    /// <param name="baseName">
    /// The artifact stem to normalize.
    /// </param>
    /// <returns>
    /// The normalized artifact stem without a configuration suffix.
    /// </returns>
    public static string TrimConfigSuffix(string baseName)
    {
        if (baseName.EndsWith("Release", StringComparison.OrdinalIgnoreCase))
        {
            baseName = baseName[..^"Release".Length];
        }
        else if (baseName.EndsWith("Debug", StringComparison.OrdinalIgnoreCase))
        {
            baseName = baseName[..^"Debug".Length];
        }

        return baseName.TrimEnd('-', '_', '.');
    }

    /// <summary>
    /// Deletes one explicitly resolved toolchain output directory when it exists.
    /// </summary>
    /// <param name="path">
    /// The exact directory selected by a toolchain command.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="path"/> is empty or does not resolve to an absolute path.
    /// </exception>
    public static void DeleteDirectory(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
        {
            throw new ArgumentException("Toolchain deletion requires an absolute path.", nameof(path));
        }

        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }

    /// <summary>
    /// Captures platform tool discovery through the same cancellable process lifecycle as builds.
    /// </summary>
    /// <param name="start">
    /// The prepared discovery command, including any platform-specific argument quoting.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels discovery and waits for its process tree to exit.
    /// </param>
    /// <returns>
    /// Complete standard output after successful discovery; errors and cancellation propagate.
    /// </returns>
    internal static async Task<string> CaptureOutputAsync(
        ProcessStartInfo start,
        CancellationToken cancellationToken
    ) {
        using var output = new StringWriter();
        await RunProcessAsync(start, cancellationToken, null, output).ConfigureAwait(false);
        return output.ToString();
    }

    private static async Task RunCoreAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string>? environment,
        TextWriter standardOutput
    ) {
        var start = new ProcessStartInfo(fileName) { WorkingDirectory = workingDirectory };
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        await RunProcessAsync(start, cancellationToken, environment, standardOutput).ConfigureAwait(false);
    }

    private static async Task RunProcessAsync(
        ProcessStartInfo start,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string>? environment,
        TextWriter standardOutput
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;
        start.CreateNoWindow = true;
        if (environment is not null)
            foreach ((string name, string value) in environment)
                start.Environment[name] = value;
        await WindowsCppBuildEnvironment.ConfigureAsync(start, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        using var process = new Process { StartInfo = start };
        if (!process.Start())
            throw new InvalidOperationException($"Cannot start '{start.FileName}'.");
        Task output = ForwardAsync(process.StandardOutput, standardOutput);
        Task error = ForwardAsync(process.StandardError, Console.Error);
        Task exited = process.WaitForExitAsync(cancellationToken);
        try
        {
            var pending = new List<Task> { exited, output, error };
            while (pending.Count > 0)
            {
                Task completed = await Task.WhenAny(pending).ConfigureAwait(false);
                await completed.ConfigureAwait(false);
                pending.Remove(completed);
            }
        }
        catch
        {
            try
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException) when (process.HasExited)
            {
                // The process completed between the exit check and failure cleanup.
            }
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            try
            {
                await Task.WhenAll(exited, output, error).ConfigureAwait(false);
            }
            catch
            {
                // Observe all pumps while preserving the failure that initiated process cleanup.
            }
            throw;
        }
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"'{start.FileName}' failed with exit code {process.ExitCode}.");
    }

    private static async Task ForwardAsync(
        StreamReader source,
        TextWriter destination
    ) {
        while (await source.ReadLineAsync().ConfigureAwait(false) is { } line)
            await destination.WriteLineAsync(line).ConfigureAwait(false);
    }
}
