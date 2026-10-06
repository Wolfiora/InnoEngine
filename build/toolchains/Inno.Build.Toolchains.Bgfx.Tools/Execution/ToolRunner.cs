using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Native.LibraryLoading;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Runs bgfx tool executables from the native output with argument-safe process invocation.
/// </summary>
public sealed class ToolRunner
{
    private readonly IReadOnlyDictionary<BgfxTool, string>? m_executables;

    /// <summary>
    /// Creates a runner for tools explicitly deployed with the current application.
    /// Resolution is deferred until execution; constructing a compiler does not start or discover a process.
    /// </summary>
    public ToolRunner() { }

    /// <summary>
    /// Freezes executables selected and validated by the host's toolchain operation.
    /// </summary>
    /// <param name="executables">
    /// Tool identities mapped to existing absolute executable paths; the collection is copied.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity is unsupported or an executable path is relative.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// A declared executable is unavailable.
    /// </exception>
    public ToolRunner(IReadOnlyDictionary<BgfxTool, string> executables)
    {
        ArgumentNullException.ThrowIfNull(executables);
        Dictionary<BgfxTool, string> snapshot = [];
        foreach ((BgfxTool tool, string path) in executables)
        {
            if (!Enum.IsDefined(tool) || !Path.IsPathFullyQualified(path))
                throw new ArgumentException("A tool runner requires supported identities and absolute executable paths.", nameof(executables));
            if (!File.Exists(path))
                throw new FileNotFoundException("A selected BGFX executable is unavailable.", path);
            snapshot.Add(tool, Path.GetFullPath(path));
        }
        m_executables = new ReadOnlyDictionary<BgfxTool, string>(snapshot);
    }

    /// <summary>
    /// Executes the configured workflow and returns its process outcome.
    /// </summary>
    /// <param name="tool">
    /// Tool to execute.
    /// </param>
    /// <param name="arguments">
    /// Individual command-line arguments without shell quoting.
    /// </param>
    /// <param name="workingDirectory">
    /// Optional working directory; defaults to <see cref="AppContext.BaseDirectory"/>.
    /// </param>
    /// <returns>
    /// The exit code and captured output.
    /// </returns>
    public ToolRunResult Run(
        BgfxTool tool,
        IReadOnlyList<string> arguments,
        string? workingDirectory = null
    )
        => RunAsync(tool, arguments, workingDirectory).AsTask().GetAwaiter().GetResult();

    /// <summary>
    /// Runs the external tool asynchronously and captures its complete process outcome.
    /// </summary>
    /// <param name="tool">
    /// Tool to execute.
    /// </param>
    /// <param name="arguments">
    /// Individual command-line arguments without shell quoting.
    /// </param>
    /// <param name="workingDirectory">
    /// Optional working directory; defaults to <see cref="AppContext.BaseDirectory"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation that terminates the child process tree.
    /// </param>
    /// <returns>
    /// The exit code and captured output.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the requested bgfx tool cannot be resolved.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the native process cannot be started.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Cancellation stopped the process tree and all redirected output has been drained.
    /// </exception>
    public async ValueTask<ToolRunResult> RunAsync(
        BgfxTool tool,
        IReadOnlyList<string> arguments,
        string? workingDirectory = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        cancellationToken.ThrowIfCancellationRequested();
        string toolPath = ResolveToolPath(tool);
        var startInfo = new ProcessStartInfo
        {
            FileName = toolPath,
            WorkingDirectory = workingDirectory ?? AppContext.BaseDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        foreach (string argument in arguments)
        {
            ArgumentNullException.ThrowIfNull(argument);
            startInfo.ArgumentList.Add(argument);
        }

        using Process process = Process.Start(startInfo)
            ?? throw new InvalidOperationException($"Failed to start tool: {toolPath}");
        Task<string> standardOutput = process.StandardOutput.ReadToEndAsync();
        Task<string> standardError = process.StandardError.ReadToEndAsync();
        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            return new ToolRunResult(
                process.ExitCode,
                await standardOutput.ConfigureAwait(false),
                await standardError.ConfigureAwait(false));
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
                // Exit can race with cancellation before the process tree is stopped.
            }
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            try
            {
                await Task.WhenAll(standardOutput, standardError).ConfigureAwait(false);
            }
            catch
            {
                // Observe both readers before preserving the original operation failure.
            }
            throw;
        }
    }

    private string ResolveToolPath(BgfxTool tool)
    {
        if (m_executables is not null)
            return m_executables.TryGetValue(tool, out string? executable)
                ? executable : throw new FileNotFoundException($"The selected distribution has no '{tool}' executable.");
        string name = tool.ToString().ToLowerInvariant() + GetConfigSuffix();
        return NativeDllLoader.FindNativeFile(OperatingSystem.IsWindows() ? name + ".exe" : name);
    }

    private static string GetConfigSuffix()
    {
#if DEBUG
        return "-debug";
#else
        return "-release";
#endif
    }

}
