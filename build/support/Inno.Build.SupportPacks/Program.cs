using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.SupportPacks;

internal static class Program
{
    private static async Task<int> Main(string[] arguments)
    {
        try
        {
            SupportPackCommand command = SupportPackCommand.Parse(arguments);
            using var cancellation = new CancellationTokenSource();
            ConsoleCancelEventHandler handler = (
                _,
                eventArguments
            ) =>
            {
                eventArguments.Cancel = true;
                cancellation.Cancel();
            };
            Console.CancelKeyPress += handler;
            try
            {
                string installed = await PlayerSupportPackPublisher.PublishAsync(command.engineRoot, command.outputRoot, command.target, command.dotnetHost, cancellation.Token)
                    .ConfigureAwait(false);
                Console.WriteLine(installed);
                return 0;
            }
            finally
            {
                Console.CancelKeyPress -= handler;
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Support Pack generation was canceled.");
            return 2;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 1;
        }
    }
}

internal sealed record SupportPackCommand(
    string engineRoot,
    string outputRoot,
    BuildTargetId target,
    string dotnetHost
) {
    internal static SupportPackCommand Parse(IReadOnlyList<string> arguments)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Count; index += 2)
        {
            if (index + 1 >= arguments.Count || !arguments[index].StartsWith("--", StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "Usage: --engine-root <path> --output <path> --target <macos-arm64|windows-x64> [--dotnet <path>].");
            }
            if (!values.TryAdd(arguments[index], arguments[index + 1]))
                throw new ArgumentException($"Argument '{arguments[index]}' was supplied more than once.");
        }
        string engineRoot = Require(values, "--engine-root");
        string outputRoot = Require(values, "--output");
        BuildTargetId target = new(Require(values, "--target"));
        if (target != BuildTargetId.macOSArm64 && target != BuildTargetId.windowsX64)
            throw new ArgumentException($"Support Pack target '{target}' is not implemented.");
        string dotnetHost = values.GetValueOrDefault("--dotnet")
                            ?? Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
                            ?? "dotnet";
        return new SupportPackCommand(
            Path.GetFullPath(engineRoot),
            Path.GetFullPath(outputRoot),
            target,
            dotnetHost);
    }

    private static string Require(
        IReadOnlyDictionary<string, string> values,
        string name
    )
        => values.TryGetValue(name, out string? value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException($"Required argument '{name}' is missing.");
}
