using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Validation;

internal static class ValidationProcess
{
    internal static async Task<string> RunAsync(
        string executable,
        IEnumerable<string> arguments,
        string directory,
        string? log,
        CancellationToken cancellation,
        string? input = null
    ) {
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = directory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = input is not null,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };
        if (input is not null)
            start.StandardInputEncoding = new UTF8Encoding(false);
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        if (Path.GetFileNameWithoutExtension(executable).Equals("dotnet", StringComparison.OrdinalIgnoreCase))
            foreach (var pair in DotNetSdkEnvironment.Create(executable))
                if (pair.Value is null)
                    start.Environment.Remove(pair.Key);
                else
                    start.Environment[pair.Key] = pair.Value;
        using TextWriter? writer = log is null ? null : TextWriter.Synchronized(new StreamWriter(
            new FileStream(log, FileMode.CreateNew, FileAccess.Write, FileShare.Read), new UTF8Encoding(false)) { AutoFlush = true });
        using var process = Process.Start(start) ?? throw new InvalidOperationException("Validation process did not start.");
        var elapsed = Stopwatch.StartNew();
        long characters = 0;
        long lastOutput = Stopwatch.GetTimestamp();
        string excerpt = string.Empty;
        object progressSync = new();
        using var progressStop = new CancellationTokenSource();
        Task progress = log is null ? Task.CompletedTask : ReportProgressAsync();
        Task<string> output = DrainAsync(process.StandardOutput);
        Task<string> errors = DrainAsync(process.StandardError);
        try
        {
            if (input is not null)
            {
                await process.StandardInput.WriteAsync(input.AsMemory(), cancellation).ConfigureAwait(false);
                process.StandardInput.Close();
            }
            await process.WaitForExitAsync(cancellation).ConfigureAwait(false);
        }
        catch
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            throw;
        }
        finally
        {
            try
            {
                await Task.WhenAll(output, errors).ConfigureAwait(false);
            }
            finally
            {
                progressStop.Cancel();
                await progress.ConfigureAwait(false);
            }
        }
        if (log is not null)
            Console.WriteLine($"Process exited: {Path.GetFileName(log)}; pid={process.Id}; exit={process.ExitCode}; elapsed={elapsed.Elapsed.TotalSeconds:F1}s.");
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"{executable} exited with {process.ExitCode}: {errors.Result}\n{output.Result}");
        return output.Result;

        async Task<string> DrainAsync(StreamReader reader)
        {
            var content = new StringBuilder();
            char[] buffer = new char[4096];
            int count;
            while ((count = await reader.ReadAsync(buffer, CancellationToken.None).ConfigureAwait(false)) != 0)
            {
                content.Append(buffer, 0, count);
                if (log is not null)
                {
                    lock (progressSync)
                    {
                        characters += count;
                        lastOutput = Stopwatch.GetTimestamp();
                        var tail = new StringBuilder(Math.Min(count, 200));
                        for (int index = Math.Max(0, count - 200); index < count; index++)
                            tail.Append(char.IsControl(buffer[index]) ? ' ' : buffer[index]);
                        excerpt = tail.ToString().Trim();
                    }
                }
                if (writer is not null)
                    await writer.WriteAsync(buffer.AsMemory(0, count), CancellationToken.None).ConfigureAwait(false);
            }
            return content.ToString();
        }

        async Task ReportProgressAsync()
        {
            Console.WriteLine($"Process started: {Path.GetFileName(log)}; executable={Path.GetFileName(executable)}; pid={process.Id}; log={log}.");
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            try
            {
                while (await timer.WaitForNextTickAsync(progressStop.Token).ConfigureAwait(false))
                {
                    string activity;
                    lock (progressSync)
                    {
                        activity = $"outputCharacters={characters}; lastOutputAge={Stopwatch.GetElapsedTime(lastOutput).TotalSeconds:F1}s; lastOutputExcerpt={excerpt}";
                    }
                    string cpu;
                    try
                    {
                        process.Refresh();
                        cpu = process.TotalProcessorTime.TotalSeconds.ToString("F1", System.Globalization.CultureInfo.InvariantCulture) + "s";
                    }
                    catch (Exception failure) when (failure is InvalidOperationException or Win32Exception or NotSupportedException)
                    {
                        cpu = "unavailable";
                    }
                    // A quiet parent may be waiting for active children. Activity is diagnostic, never a completion deadline.
                    Console.WriteLine($"Process active: {Path.GetFileName(log)}; pid={process.Id}; elapsed={elapsed.Elapsed.TotalSeconds:F1}s; parentCpu={cpu}; {activity}");
                }
            }
            catch (OperationCanceledException) when (progressStop.IsCancellationRequested) { }
        }
    }
}
