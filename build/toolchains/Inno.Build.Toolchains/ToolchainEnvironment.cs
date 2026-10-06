using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    /// Executes a declared absolute tool with an explicitly resolved SDK environment and records the operation cost.
    /// </summary>
    /// <param name="context">
    /// The operation owning execution statistics and frozen recipe inputs.
    /// </param>
    /// <param name="fileName">
    /// The absolute executable declared by the recipe; no host-native SDK is inferred.
    /// </param>
    /// <param name="arguments">
    /// Individual arguments passed without shell interpretation.
    /// </param>
    /// <param name="workingDirectory">
    /// The owned source or intermediate directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels execution and drains the complete process tree.
    /// </param>
    /// <param name="environment">
    /// The frozen environment supplied by the selected SDK resolver.
    /// </param>
    /// <returns>
    /// Completion after successful exit and output delivery.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The executable is not an absolute declared tool path.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The process fails to start or exits unsuccessfully.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Execution was canceled.
    /// </exception>
    public static Task RunAsync(
        NativeBuildContext context,
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string> environment
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(environment);
        if (!Path.IsPathFullyQualified(fileName))
            throw new ArgumentException("An explicit SDK tool must have an absolute path.", nameof(fileName));
        context.inputState.RecordProcess();
        return RunAsync(fileName, arguments, workingDirectory, cancellationToken, environment);
    }

    /// <summary>
    /// Runs a host-native process with the context's frozen compiler and SDK selection.
    /// </summary>
    /// <param name="context">
    /// A context resolved by HostNativeToolchain before artifact fingerprinting.
    /// </param>
    /// <param name="fileName">
    /// A selected build command or an absolute executable declared as a component input.
    /// </param>
    /// <param name="arguments">
    /// Individual arguments passed without shell interpretation.
    /// </param>
    /// <param name="workingDirectory">
    /// The source snapshot or intermediate directory owned by the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels and retires the complete process tree.
    /// </param>
    /// <returns>
    /// Completion after successful exit and complete output delivery.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Host tools were not resolved, a command is unavailable or a process fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static Task RunAsync(
        NativeBuildContext context,
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        CancellationToken cancellationToken
    ) {
        HostNativeToolchain tools = context.hostToolchain
            ?? throw new InvalidOperationException("Host native tools must be resolved before execution.");
        string executable = Path.IsPathFullyQualified(fileName) ? fileName : tools.ResolveExecutable(fileName);
        IReadOnlyList<string> selectedArguments = fileName == "cmake" && arguments.Contains("-S")
            ? arguments.Concat(tools.cmakeArguments).ToArray() : arguments;
        context.inputState.RecordProcess();
        return RunAsync(executable, selectedArguments, workingDirectory, cancellationToken, tools.environment);
    }

    /// <summary>
    /// Runs a quoted host command with a frozen compiler and SDK environment.
    /// </summary>
    /// <param name="context">
    /// The context resolved before artifact fingerprinting.
    /// </param>
    /// <param name="fileName">
    /// A selected command or an absolute executable declared in the component inputs.
    /// </param>
    /// <param name="arguments">
    /// Arguments quoted for the selected executable's command-line parser.
    /// </param>
    /// <param name="workingDirectory">
    /// The operation's source snapshot or intermediate directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels and retires the complete process tree.
    /// </param>
    /// <returns>
    /// Completion after the command exits successfully and its streams drain.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Tools are unresolved or execution fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static Task RunAsync(
        NativeBuildContext context,
        string fileName,
        string arguments,
        string workingDirectory,
        CancellationToken cancellationToken
    ) {
        HostNativeToolchain tools = context.hostToolchain
            ?? throw new InvalidOperationException("Host native tools must be resolved before execution.");
        string executable = Path.IsPathFullyQualified(fileName) ? fileName : tools.ResolveExecutable(fileName);
        if (fileName == "cmake" && arguments.StartsWith("-S ", StringComparison.Ordinal))
            arguments += " " + string.Join(" ", tools.cmakeArguments.Select(static argument => "\"" + argument + "\""));
        var start = new ProcessStartInfo(executable) { Arguments = arguments, WorkingDirectory = workingDirectory };
        context.inputState.RecordProcess();
        return RunProcessAsync(start, cancellationToken, tools.environment, Console.Out);
    }

    /// <summary>
    /// Resolves one explicitly selected executable before it becomes a build input.
    /// </summary>
    /// <param name="name">
    /// An executable path or a command name available on the current process PATH.
    /// </param>
    /// <returns>
    /// The absolute executable path used for process execution and artifact identity.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The name is empty.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The executable is unavailable at the supplied path or on PATH.
    /// </exception>
    public static string ResolveExecutable(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (File.Exists(name))
            return Path.GetFullPath(name);
        if (Path.IsPathRooted(name) || name.Contains(Path.DirectorySeparatorChar)
            || name.Contains(Path.AltDirectorySeparatorChar))
            throw new FileNotFoundException($"Cannot locate toolchain executable '{name}'.", name);

        string fileName = OperatingSystem.IsWindows() && !Path.HasExtension(name) ? name + ".exe" : name;
        foreach (string directory in (Environment.GetEnvironmentVariable("PATH") ?? string.Empty).Split(Path.PathSeparator))
        {
            if (string.IsNullOrWhiteSpace(directory))
                continue;
            string candidate = Path.Combine(directory.Trim('"'), fileName);
            if (File.Exists(candidate))
                return Path.GetFullPath(candidate);
        }
        throw new FileNotFoundException($"Cannot locate toolchain executable '{name}'.", name);
    }

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
    /// Runs a child build with caller-owned output destinations and the shared process retirement protocol.
    /// </summary>
    /// <param name="fileName">
    /// The executable selected by the build composition.
    /// </param>
    /// <param name="arguments">
    /// Structured arguments passed without shell interpretation.
    /// </param>
    /// <param name="workingDirectory">
    /// The project directory controlling SDK and workload resolution.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the complete process tree and drains both output streams.
    /// </param>
    /// <param name="environment">
    /// Optional child-only toolchain variables.
    /// </param>
    /// <param name="standardOutput">
    /// The caller-owned standard output destination; the runner does not dispose it.
    /// </param>
    /// <param name="standardError">
    /// The caller-owned standard error destination; the runner does not dispose it.
    /// </param>
    /// <returns>
    /// Completion after successful exit and complete output delivery; failures propagate.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The executable cannot start or exits unsuccessfully.
    /// </exception>
    /// <exception cref="IOException">
    /// An output destination fails; the owned process tree is terminated before failure returns.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled after retiring its owned process tree.
    /// </exception>
    public static Task RunAsync(
        string fileName,
        IReadOnlyList<string> arguments,
        string workingDirectory,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string>? environment,
        TextWriter standardOutput,
        TextWriter standardError
    ) {
        ArgumentNullException.ThrowIfNull(standardOutput);
        ArgumentNullException.ThrowIfNull(standardError);
        var start = new ProcessStartInfo(fileName) { WorkingDirectory = workingDirectory };
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        return RunProcessAsync(start, cancellationToken, environment, standardOutput, standardError);
    }

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
        TextWriter standardOutput,
        TextWriter? standardError = null
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
        Task error = ForwardAsync(process.StandardError, standardError ?? Console.Error);
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
