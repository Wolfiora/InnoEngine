using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Inno.Testing.Processes;

/// <summary>
/// Executes isolated native contracts while retaining output and bounded process cleanup.
/// </summary>
public static class NativeProcessTestHost
{
    /// <summary>
    /// Runs one contract in a separate main-thread host and reports all captured failure output.
    /// </summary>
    /// <param name="assembly">
    /// The absolute path of the executable contract assembly.
    /// </param>
    /// <param name="contract">
    /// The exact contract identifier understood by the child host.
    /// </param>
    /// <param name="timeoutMilliseconds">
    /// The positive execution budget before terminating the process tree.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The assembly or contract is empty, or the assembly path is relative.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The execution budget is not positive.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// The child exceeded its execution budget; the message retains partial output and cleanup results.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The child failed, could not start, or did not drain its redirected output.
    /// </exception>
    public static void Run(
        string assembly,
        string contract,
        int timeoutMilliseconds = 90_000
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(contract);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(timeoutMilliseconds);
        if (!Path.IsPathFullyQualified(assembly))
            throw new ArgumentException("A contract assembly requires an absolute path.", nameof(assembly));
        var start = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        start.ArgumentList.Add(assembly);
        start.ArgumentList.Add(contract);
        var output = new ConcurrentQueue<string>();
        var error = new ConcurrentQueue<string>();
        var outputClosed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var errorClosed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var process = new Process { StartInfo = start };
        process.OutputDataReceived += (
            _,
            data
        ) => Capture(data, output, outputClosed);
        process.ErrorDataReceived += (
            _,
            data
        ) => Capture(data, error, errorClosed);
        Stopwatch elapsed = Stopwatch.StartNew();
        if (!process.Start())
            throw new InvalidOperationException("The native contract host did not start.");
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        bool timedOut = !process.WaitForExit(timeoutMilliseconds);
        string cleanup = string.Empty;
        if (timedOut)
        {
            try
            {
                process.Kill(entireProcessTree: true);
                if (!process.WaitForExit(10_000))
                    cleanup = "The process tree did not exit within the cleanup budget.";
            }
            catch (Exception failure)
            {
                cleanup = "Process cleanup failed: " + failure;
            }
        }
        bool drained = Task.WhenAll(outputClosed.Task, errorClosed.Task).Wait(TimeSpan.FromSeconds(5));
        if (timedOut || !drained || process.ExitCode != 0)
        {
            string diagnostic = $"Native contract '{contract}', PID {process.Id}, elapsed {elapsed.Elapsed}.\n"
                + $"Command: {start.FileName} {assembly} {contract}\n"
                + $"TimedOut={timedOut}; OutputDrained={drained}. {cleanup}\n"
                + "stdout:\n" + string.Join(Environment.NewLine, output)
                + "\nstderr:\n" + string.Join(Environment.NewLine, error);
            if (timedOut)
                throw new TimeoutException(diagnostic);
            throw new InvalidOperationException(diagnostic);
        }
    }

    private static void Capture(
        DataReceivedEventArgs data,
        ConcurrentQueue<string> destination,
        TaskCompletionSource closed
    ) {
        if (data.Data is string line)
            destination.Enqueue(line);
        else
            closed.TrySetResult();
    }
}
