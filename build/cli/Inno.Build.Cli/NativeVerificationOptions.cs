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
        string bindGenRoot,
        string configuration,
        string dotnet
    ) {
        this.engineRoot = engineRoot;
        this.bindGenRoot = bindGenRoot;
        this.configuration = configuration;
        this.dotnet = dotnet;
    }

    internal static string usage
        => "Usage: dotnet run --project build/cli/Inno.Build.Cli -- verify-native "
           + "[--engine-root <dir>] [--bindgen-root <dir>] [--configuration <Debug|Release>] [--dotnet <path>]";

    internal string engineRoot { get; }

    internal string bindGenRoot { get; }

    internal string configuration { get; }

    internal string dotnet { get; }

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
        string bindGenRoot = Path.GetFullPath(values.GetValueOrDefault(
            "bindgen-root",
            Path.Combine(Directory.GetParent(engineRoot)!.FullName, "BindGen-CS")));
        string configuration = values.GetValueOrDefault("configuration", C_DEFAULT_CONFIGURATION);
        if (configuration is not ("Debug" or "Release"))
            throw new ArgumentException("--configuration must be either Debug or Release.");
        string dotnet = values.GetValueOrDefault("dotnet", ResolveDotnet());

        string[] knownKeys = ["engine-root", "bindgen-root", "configuration", "dotnet"];
        string? unknownKey = values.Keys.FirstOrDefault(key => !knownKeys.Contains(key, StringComparer.Ordinal));
        if (unknownKey != null)
            throw new ArgumentException($"Unknown argument '--{unknownKey}'.{Environment.NewLine}{usage}");
        return new NativeVerificationOptions(engineRoot, bindGenRoot, configuration, dotnet);
    }

    private static string ResolveDotnet()
    {
        string? hostPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!string.IsNullOrWhiteSpace(hostPath) && File.Exists(hostPath))
            return hostPath;
        string? dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (!string.IsNullOrWhiteSpace(dotnetRoot))
        {
            string rootedHost = Path.Combine(dotnetRoot, OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
            if (File.Exists(rootedHost))
                return rootedHost;
        }
        string userHost = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".dotnet",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        return File.Exists(userHost) ? userHost : "dotnet";
    }
}
