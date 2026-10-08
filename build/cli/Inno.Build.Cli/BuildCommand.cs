using System;
using System.Collections.Generic;
using System.IO;
using Inno.Assets;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Core.Settings;

namespace Inno.Build.Cli;

internal enum BuildCommandKind
{
    Game,
    Plugin,
    ImportSample,
    Scripts
}

internal sealed class BuildCommand
{
    private readonly IReadOnlyDictionary<string, string> m_values;

    private BuildCommand(
        BuildCommandKind kind,
        IReadOnlyDictionary<string, string> values
    ) {
        this.kind = kind;
        m_values = values;
        if (kind == BuildCommandKind.Game)
            _ = Require(values, "target");
        projectDirectory = Require(values, "project");
        supportPackRoot = kind == BuildCommandKind.Game
            ? Require(values, "support-packs")
            : values.GetValueOrDefault("support-packs", Path.Combine(AppContext.BaseDirectory, "SupportPacks"));
    }

    internal BuildCommandKind kind { get; }

    internal string projectDirectory { get; }

    internal string supportPackRoot { get; }

    internal BuildTargetId toolsTarget => new(Require(m_values, "tools-target"));

    internal BuildTargetId gameTarget => new(Require(m_values, "target"));

    internal string? profilePath => m_values.GetValueOrDefault("profile");

    internal AssetPath sampleSource => AssetPath.Parse(Require(m_values, "source"));

    internal string outputDirectory => Require(m_values, "output");

    internal static BuildCommand Parse(IReadOnlyList<string> args)
    {
        if (args.Count == 0 || args[0] is "help" or "--help" or "-h")
            throw new ArgumentException(Usage());
        BuildCommandKind kind = args[0] switch
        {
            "game" => BuildCommandKind.Game,
            "plugin" => BuildCommandKind.Plugin,
            "import-sample" => BuildCommandKind.ImportSample,
            "scripts" => BuildCommandKind.Scripts,
            _ => throw new ArgumentException($"Unknown command '{args[0]}'.{Environment.NewLine}{Usage()}")
        };
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 1; index < args.Count; index++)
        {
            string argument = args[index];
            if (!argument.StartsWith("--", StringComparison.Ordinal))
                throw new ArgumentException($"Unexpected argument '{argument}'.");
            string key = argument[2..];
            if (key == "include-dependencies")
            {
                values.Add(key, bool.TrueString);
                continue;
            }
            if (++index >= args.Count)
                throw new ArgumentException($"Argument '--{key}' requires a value.");
            values.Add(key, args[index]);
        }
        return new BuildCommand(kind, values);
    }

    internal GameBuildRequest CreateGameRequest(BuildProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        if (m_values.TryGetValue("startup-scene", out string? scene))
            profile.startupScene = AssetPath.Parse(scene).ToString();
        if (m_values.TryGetValue("target", out string? target))
            profile.target = new BuildTargetId(target);
        if (m_values.TryGetValue("deployment", out string? deployment))
            profile.managedDeployment = new ManagedDeploymentId(deployment);
        return new GameBuildRequest
        {
            profile = profile,
            outputDirectory = Require(m_values, "output")
        };
    }

    internal PluginBuildRequest CreatePluginRequest(ProjectId projectId)
        => new()
        {
            pluginId = projectId.value,
            displayName = Require(m_values, "display-name"),
            outputPath = Require(m_values, "output"),
            dependencies = m_values.TryGetValue("dependencies", out string? dependencyList)
                ? dependencyList.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                : Array.Empty<string>(),
            includeDependencies = m_values.ContainsKey("include-dependencies")
        };

    private static string Require(
        IReadOnlyDictionary<string, string> values,
        string key
    )
        => values.TryGetValue(key, out string? value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException($"Required argument '--{key}' is missing.");

    private static string Usage()
        => "Usage:\n"
           + "  Inno.Build.Cli game --tools-target <native-target> --project <dir> --support-packs <dir> --output <dir> [--profile <BuildProfile.inno>] --target <target> [--deployment <provider-id>] [--startup-scene <scene>]\n"
           + "  Inno.Build.Cli plugin --tools-target <native-target> --project <dir> --output <package.iplugin> --display-name <name> [--dependencies <id,id>] [--include-dependencies]\n"
           + "  Inno.Build.Cli import-sample --tools-target <native-target> --project <dir> --source <plugin-id::~Sample>\n"
           + "  Inno.Build.Cli scripts --tools-target <native-target> --project <dir> --output <dir>";
}
