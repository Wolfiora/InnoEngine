using System;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Toolchains;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Architecture;

namespace Inno.Build.Cli;

/// <summary>
/// Executes explicit engine and project build commands for a caller-owned development lifetime.
/// </summary>
public static partial class BuildApplication
{
    /// <summary>
    /// Inspects the selected SDK's evaluated Build-tool input closure without compiling or preparing Native products.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute source checkout.
    /// </param>
    /// <param name="configuration">
    /// The explicit Debug or Release compiler configuration.
    /// </param>
    /// <param name="dotnetHost">
    /// The selected managed SDK executable.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels evaluation and input hashing.
    /// </param>
    /// <returns>
    /// The content identity of the actual source graph, restored dependencies, compiler and SDK.
    /// </returns>
    /// <exception cref="IOException">
    /// A required source, SDK or restored dependency cannot be read.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The SDK cannot evaluate the current graph.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Inspection was canceled.
    /// </exception>
    public static async Task<string> ReadInputIdentityAsync(
        string engineRoot,
        string configuration,
        string dotnetHost,
        CancellationToken cancellationToken
    ) {
        string project = Path.Combine(engineRoot, "build", "cli", "Inno.Build.Cli", "Inno.Build.Cli.csproj");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnetHost, project, cancellationToken).ConfigureAwait(false);
        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(
            new NativeBuildContext(engineRoot, configuration.ToLowerInvariant()), sdk, project,
            new Dictionary<string, string> { ["Configuration"] = configuration }, cancellationToken).ConfigureAwait(false);
        return snapshot.fingerprint;
    }

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
    public static async Task<int> Run(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Length == 0 || arguments[0] is "help" or "--help" or "-h")
        {
            Console.WriteLine("InnoEngine.Build <editor|clean|bindings|support-pack|game|plugin|scripts|import-sample|verify|verify-native|shader> [options]");
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
                    return await ProjectBuildWorkflow.RunAsync(arguments, cancellationToken);
                case "verify":
                    return ArchitectureValidator.Execute(options, cancellationToken);
                case "verify-native":
                    return await NativeBindingsVerification.RunAsync(options, cancellationToken);
                case "editor":
                case "clean":
                case "bindings":
                case "support-pack":
                case "shader":
                    BuildCommandLine parsed = BuildCommandLine.Parse(options);
                    string[] specific = command switch
                    {
                        "editor" => ["clean"],
                        "clean" => ["scope", "dry-run"],
                        "bindings" => ["product"],
                        "support-pack" => ["output"],
                        "shader" => ["assets", "shader", "renderer", "output"],
                        _ => []
                    };
                    parsed.Validate(["engine-root", "dotnet", "target", "tools-target", "configuration",
                        "cache-budget-gib", "cache-min-free-gib", "cache-time-seconds", ..specific]);
                    var context = new BuildCommandContext(parsed);
                    switch (command)
                    {
                        case "editor": await EditorBuildCommand.RunAsync(context, parsed, cancellationToken); break;
                        case "clean": await CleanCommand.RunAsync(context, parsed, cancellationToken); break;
                        case "bindings": await BindingCommand.RunAsync(context, parsed, cancellationToken); break;
                        case "support-pack": await SupportPackCommand.RunAsync(context, parsed, cancellationToken); break;
                        case "shader": await ShaderCommand.RunAsync(context, parsed, cancellationToken); break;
                    }
                    if (command is "editor" or "support-pack" or "bindings" or "shader")
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
