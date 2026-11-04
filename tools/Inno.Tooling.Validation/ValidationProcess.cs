using System;
using System.Collections.Generic;
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
        CancellationToken cancellation
    ) {
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = directory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
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
        Task<string> output = DrainAsync(process.StandardOutput);
        Task<string> errors = DrainAsync(process.StandardError);
        try
        {
            await process.WaitForExitAsync(cancellation).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            throw;
        }
        finally
        {
            await Task.WhenAll(output, errors).ConfigureAwait(false);
        }
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
                if (writer is not null)
                    await writer.WriteAsync(buffer.AsMemory(0, count), CancellationToken.None).ConfigureAwait(false);
            }
            return content.ToString();
        }
    }
}
