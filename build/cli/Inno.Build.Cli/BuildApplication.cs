using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Architecture;
using Inno.Tooling.Validation;

namespace Inno.Build.Cli;

/// <summary>
/// Executes explicit engine and project build commands for a caller-owned development lifetime.
/// </summary>
public static partial class BuildApplication
{
    /// <summary>
    /// Runs one build command without taking ownership of the caller's cancellation source.
    /// </summary>
    /// <param name="arguments">
    /// The command and its documented options; help prints the supported command names.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels owned work and preserves completed publications according to their pipeline contracts.
    /// </param>
    /// <returns>
    /// Zero on success, two on cancellation, or one on a command or execution failure.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The argument array is absent.
    /// </exception>
    public static async Task<int> RunAsync(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Length == 0 || arguments[0] is "help" or "--help" or "-h")
        {
            Console.WriteLine(
                "InnoEngine.Build <docs|rules select|validate|native-dependencies|support|editor|clean|bindings|support-pack|game|plugin|scripts|import-sample|verify|style|verify-native|shader> [options]"
            );
            Console.WriteLine("Default entry command: support --product editor (current SDK host and entry configuration).");
            Console.WriteLine(
                "support --product editor|player --target TARGET: prepare reusable target support without building the Editor's managed source."
            );
            Console.WriteLine("editor [--clean]: build the complete Editor entry and its dependencies.");
            Console.WriteLine(
                "style check|fix ROOT [--rule ID]... [--paths-from FILE] [--report FILE]: check handwritten C# against the style rulebook or apply its automatic fixes."
            );
            return 0;
        }
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            string command = arguments[0];
            string[] options = arguments[1..];
            switch (command)
            {
                case "game":
                case "plugin":
                case "scripts":
                case "import-sample":
                    return await ProjectBuildWorkflow.RunAsync(arguments, cancellationToken).ConfigureAwait(false);
                case "verify":
                    return ArchitectureValidator.Execute(options, cancellationToken);
                case "style":
                    return StyleCommand.Execute(options);
                case "docs":
                    return await DocumentationReferences.ExecuteAsync(options, cancellationToken).ConfigureAwait(false);
                case "rules" when options is ["select", .. var selection]:
                    return RepositoryRuleSelection.Execute(selection);
                case "validate":
                    return await ValidationCommand.ExecuteAsync(options, cancellationToken).ConfigureAwait(false);
                case "verify-native":
                    return await NativeBindingsVerification.RunAsync(options, cancellationToken).ConfigureAwait(false);
                case "native-dependencies":
                case "support":
                case "editor":
                case "clean":
                case "bindings":
                case "support-pack":
                case "shader":
                    BuildCommandLine parsed = BuildCommandLine.Parse(options);
                    string[] specific = command switch
                    {
                        "native-dependencies" => ["product", "release", "input", "output", "operation"],
                        "support" => ["product"],
                        "editor" => ["clean"],
                        "clean" => ["scope", "dry-run"],
                        "bindings" => ["product"],
                        "support-pack" => ["output", "archive"],
                        "shader" => ["assets", "shader", "renderer", "output"],
                        _ => []
                    };
                    parsed.Validate([
                        "engine-root",
                        "dotnet",
                        "target",
                        "tools-target",
                        "configuration",
                        "cache-budget-gib",
                        "cache-min-free-gib",
                        "cache-time-seconds",
                        "native-dependencies",
                        ..specific
                    ]);
                    var context = new BuildCommandContext(parsed);
                    switch (command)
                    {
                        case "native-dependencies":
                            await NativeDependenciesCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                        case "support":
                            await SupportBuildCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                        case "editor":
                            await EditorBuildCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                        case "clean":
                            await CleanCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                        case "bindings":
                            await BindingCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                        case "support-pack":
                            await SupportPackCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                        case "shader":
                            await ShaderCommand.RunAsync(context, parsed, cancellationToken).ConfigureAwait(false);
                            break;
                    }
                    if (command is "support" or "editor" or "support-pack" or "bindings" or "shader")
                    {
                        try
                        {
                            await CleanCommand.CleanCachesAsync(context, parsed, automatic: true, cancellationToken).ConfigureAwait(false);
                        }
                        catch (Exception cleanupFailure)
                        {
                            Console.Error.WriteLine($"Build succeeded; automatic cache cleanup was deferred: {cleanupFailure.Message}");
                        }
                    }
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
    }
}
