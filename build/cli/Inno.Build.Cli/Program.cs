using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Architecture;

namespace Inno.Build.Cli;

internal static class Program
{
    private static async Task<int> Main(string[] arguments)
    {
        if (arguments.Length == 0 || arguments[0] is "help" or "--help" or "-h")
        {
            Console.WriteLine("Inno.Build.Cli <engine|clean|bindings|support-pack|game|plugin|scripts|import-sample|verify|verify-native|shader> [options]");
            return 0;
        }
        using var cancellation = new CancellationTokenSource();
        ConsoleCancelEventHandler onCancel = (
            _,
            eventArguments
        ) =>
        {
            eventArguments.Cancel = true;
            cancellation.Cancel();
        };
        Console.CancelKeyPress += onCancel;
        try
        {
            string command = arguments[0];
            string[] options = arguments[1..];
            switch (command)
            {
                case "game":
                case "plugin":
                case "scripts":
                case "import-sample":
                    return await ProjectBuildWorkflow.RunAsync(arguments, cancellation.Token);
                case "verify":
                    return ArchitectureValidator.Execute(options, cancellation.Token);
                case "verify-native":
                    return await NativeBindingsVerification.RunAsync(options, cancellation.Token);
                case "engine":
                case "clean":
                case "bindings":
                case "support-pack":
                case "shader":
                    await EngineBuildWorkflow.RunAsync(command, CliOptions.Parse(options), cancellation.Token);
                    return 0;
                default:
                    throw new ArgumentException($"Unknown build command '{command}'.");
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Build canceled; incomplete stages were not committed.");
            return 2;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
        finally
        {
            Console.CancelKeyPress -= onCancel;
        }
    }
}
