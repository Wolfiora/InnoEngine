using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class ToolchainProcessTests : IDisposable
{
    private const int C_RETIREMENT_WINDOW_MILLISECONDS = 2000;

    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoToolchainProcessTests", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        string root = Path.GetFullPath(m_root);
        string owner = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "InnoToolchainProcessTests"))
            + Path.DirectorySeparatorChar;
        if (!root.StartsWith(owner, StringComparison.Ordinal))
            throw new InvalidOperationException("The process fixture directory escaped its temporary owner.");
        long started = Stopwatch.GetTimestamp();
        while (Directory.Exists(root))
        {
            try
            {
                Directory.Delete(root, recursive: true);
                return;
            }
            catch (IOException exception) when ((exception.HResult is unchecked((int)0x80070020)
                or unchecked((int)0x80070021))
                && Stopwatch.GetElapsedTime(started).TotalMilliseconds < C_RETIREMENT_WINDOW_MILLISECONDS)
            {
                // Process termination does not promise that filesystem observers have released the directory.
                // Retry only sharing/lock violations; persistent failures and all other errors remain visible.
                Thread.Sleep(25);
            }
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CancellationTerminatesTheActiveProcessAndItsChild(bool rawArguments)
    {
        Directory.CreateDirectory(m_root);
        string marker = Path.Combine(m_root, "processes.txt");
        string script = Path.Combine(m_root, OperatingSystem.IsWindows() ? "wait.ps1" : "wait.sh");
        string[] arguments;
        string executable;
        if (OperatingSystem.IsWindows())
        {
            File.WriteAllText(script, """
                $child = Start-Process -FilePath 'ping.exe' -ArgumentList '-n 120 127.0.0.1' -PassThru -WindowStyle Hidden
                Set-Content -LiteralPath (Join-Path $PSScriptRoot 'processes.pending') -Value "$PID,$($child.Id)"
                Move-Item -LiteralPath (Join-Path $PSScriptRoot 'processes.pending') -Destination (Join-Path $PSScriptRoot 'processes.txt')
                $child.WaitForExit()
                """);
            executable = "powershell.exe";
            arguments = ["-NoProfile", "-NonInteractive", "-ExecutionPolicy", "Bypass", "-File", script];
        }
        else
        {
            File.WriteAllText(script, """
                sleep 120 &
                child=$!
                printf '%s,%s' $$ $child > "$(dirname "$0")/processes.pending"
                mv "$(dirname "$0")/processes.pending" "$(dirname "$0")/processes.txt"
                wait $child
                """);
            executable = "/bin/sh";
            arguments = [script];
        }

        using var cancellation = new CancellationTokenSource();
        Task running = rawArguments
            ? ToolchainEnvironment.RunAsync(executable, string.Join(' ', arguments.Select(Quote)), m_root, cancellation.Token)
            : ToolchainEnvironment.RunAsync(executable, arguments, m_root, cancellation.Token);
        int[] processes = [];
        try
        {
            DateTime deadline = DateTime.UtcNow.AddSeconds(15);
            while (!File.Exists(marker) && !running.IsCompleted && DateTime.UtcNow < deadline)
                await Task.Delay(25);
            Assert.True(File.Exists(marker), "The process tree did not reach its waiting stage.");
            processes = File.ReadAllText(marker).Trim().Split(',').Select(int.Parse).ToArray();
            Assert.Equal(2, processes.Length);
            Assert.All(processes, processId => Assert.True(IsRunning(processId)));

            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => running.WaitAsync(TimeSpan.FromSeconds(10)));

            Assert.All(processes, processId => Assert.False(IsRunning(processId)));
        }
        finally
        {
            cancellation.Cancel();
            foreach (int processId in processes)
            {
                if (!IsRunning(processId))
                    continue;
                using Process process = Process.GetProcessById(processId);
                process.Kill(entireProcessTree: true);
            }
        }
    }

    [Fact]
    public async Task RawRunnerRejectsFailedAndAlreadyCanceledTools()
    {
        Directory.CreateDirectory(m_root);
        await Assert.ThrowsAsync<InvalidOperationException>(() => ToolchainEnvironment.RunAsync(
            "dotnet", "--inno-invalid-option", m_root, CancellationToken.None));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ToolchainEnvironment.RunAsync(
            "inno-missing-tool", string.Empty, m_root, cancellation.Token));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task OutputFailureTerminatesTheActiveProcessAndItsChild(bool standardError)
    {
        Directory.CreateDirectory(m_root);
        string marker = Path.Combine(m_root, "processes.txt");
        string script = Path.Combine(m_root, OperatingSystem.IsWindows() ? "output.ps1" : "output.sh");
        string executable;
        string[] arguments;
        if (OperatingSystem.IsWindows())
        {
            string emission = standardError ? "[Console]::Error.WriteLine('ready')" : "[Console]::Out.WriteLine('ready')";
            File.WriteAllText(script, $$"""
                $child = Start-Process -FilePath 'ping.exe' -ArgumentList '-n 120 127.0.0.1' -PassThru -WindowStyle Hidden
                Set-Content -LiteralPath (Join-Path $PSScriptRoot 'processes.pending') -Value "$PID,$($child.Id)"
                Move-Item -LiteralPath (Join-Path $PSScriptRoot 'processes.pending') -Destination (Join-Path $PSScriptRoot 'processes.txt')
                {{emission}}
                $child.WaitForExit()
                """);
            executable = "powershell.exe";
            arguments = ["-NoProfile", "-NonInteractive", "-ExecutionPolicy", "Bypass", "-File", script];
        }
        else
        {
            string emission = standardError ? "printf 'ready\\n' >&2" : "printf 'ready\\n'";
            File.WriteAllText(script, $$"""
                sleep 120 &
                child=$!
                printf '%s,%s' $$ $child > "$(dirname "$0")/processes.pending"
                mv "$(dirname "$0")/processes.pending" "$(dirname "$0")/processes.txt"
                {{emission}}
                wait $child
                """);
            executable = "/bin/sh";
            arguments = [script];
        }

        TextWriter original = standardError ? Console.Error : Console.Out;
        using var failing = new FailingWriter();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        Task? running = null;
        try
        {
            if (standardError)
                Console.SetError(failing);
            else
                Console.SetOut(failing);
            running = ToolchainEnvironment.RunAsync(executable, arguments, m_root, cancellation.Token);

            IOException failure = await Assert.ThrowsAsync<IOException>(() => running.WaitAsync(TimeSpan.FromSeconds(10)));

            Assert.Equal("Output destination failed.", failure.Message);
            Assert.True(File.Exists(marker), "The process tree did not reach its output stage.");
            int[] processes = File.ReadAllText(marker).Trim().Split(',').Select(int.Parse).ToArray();
            Assert.Equal(2, processes.Length);
            Assert.All(processes, processId => Assert.False(IsRunning(processId)));
        }
        finally
        {
            if (standardError)
                Console.SetError(original);
            else
                Console.SetOut(original);
            cancellation.Cancel();
            if (running is not null)
            {
                try
                {
                    await running.WaitAsync(TimeSpan.FromSeconds(10));
                }
                catch
                {
                    // Observe the failure after requesting cleanup, including a timed-out assertion.
                }
            }
        }
    }

    private static string Quote(string argument) => '"' + argument.Replace("\"", "\\\"", StringComparison.Ordinal) + '"';

    private static bool IsRunning(int processId)
    {
        try
        {
            using Process process = Process.GetProcessById(processId);
            return !process.HasExited;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private sealed class FailingWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string? value) => throw new IOException("Output destination failed.");

        public override Task WriteLineAsync(string? value) => Task.FromException(new IOException("Output destination failed."));
    }
}
