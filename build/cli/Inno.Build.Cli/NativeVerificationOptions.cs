using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;

namespace Inno.Build.Cli;

internal sealed class NativeVerificationOptions
{
    private const string C_DEFAULT_CONFIGURATION = "Release";

    private NativeVerificationOptions(
        string engineRoot,
        string configuration,
        string dotnet,
        string target
    ) {
        this.engineRoot = engineRoot;
        this.configuration = configuration;
        this.dotnet = dotnet;
        this.target = target;
    }

    internal static string usage
        => "Usage: dotnet run --project entry/InnoEngine.Build/InnoEngine.Build.csproj -- verify-native "
           + "--target <native-target> [--engine-root <dir>] [--configuration <Debug|Release>] [--dotnet <path>]";

    internal string engineRoot { get; }

    internal string configuration { get; }

    internal string dotnet { get; }

    internal string target { get; }

    internal static NativeVerificationOptions Parse(IReadOnlyList<string> args)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < args.Count; index++)
        {
            string argument = args[index];
            if (!argument.StartsWith("--", StringComparison.Ordinal))
                throw new ArgumentException($"Unexpected argument '{argument}'.{Environment.NewLine}{usage}");
            string key = argument[2..];
            if (++index >= args.Count)
                throw new ArgumentException($"Argument '{argument}' requires a value.");
            if (!values.TryAdd(key, args[index]))
                throw new ArgumentException($"Argument '{argument}' was specified more than once.");
        }

        string selectedRoot = values.GetValueOrDefault("engine-root", string.Empty);
        string engineRoot = Path.GetFullPath(string.IsNullOrWhiteSpace(selectedRoot) ? ToolchainEnvironment.FindRepoRoot() : selectedRoot);
        string configuration = values.GetValueOrDefault("configuration", C_DEFAULT_CONFIGURATION);
        if (configuration is not ("Debug" or "Release"))
            throw new ArgumentException("--configuration must be either Debug or Release.");
        string dotnet = ToolchainEnvironment.ResolveExecutable(values.GetValueOrDefault("dotnet")
            ?? Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet");

        if (!values.TryGetValue("target", out string? target) || string.IsNullOrWhiteSpace(target))
            throw new ArgumentException("--target requires an explicit implemented native target.");
        string[] knownKeys = ["engine-root", "configuration", "dotnet", "target"];
        string? unknownKey = values.Keys.FirstOrDefault(key => !knownKeys.Contains(key, StringComparer.Ordinal));
        if (unknownKey != null)
            throw new ArgumentException($"Unknown argument '--{unknownKey}'.{Environment.NewLine}{usage}");
        return new NativeVerificationOptions(engineRoot, configuration, dotnet, target);
    }

}
