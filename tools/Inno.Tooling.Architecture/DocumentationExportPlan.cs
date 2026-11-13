using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

internal static class DocumentationExportPlan
{
    internal static async Task<int> ExecuteAsync(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (key is not ("--engine-root" or "--dotnet" or "--output" or "--configuration" or "--artifacts-path")
                || index + 1 >= arguments.Length || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown, duplicate or incomplete API export planning option: " + key);
        }
        string Required(string key) => options.GetValueOrDefault(key)
            ?? throw new ArgumentException("Missing API export planning option: " + key);
        string root = Path.GetFullPath(Required("--engine-root"));
        string dotnet = Path.GetFullPath(Required("--dotnet"));
        string output = Path.GetFullPath(Required("--output"));
        string configuration = options.GetValueOrDefault("--configuration", "Debug");
        if (configuration is not ("Debug" or "SourceDebug" or "Release"))
            throw new ArgumentException("Unsupported API export configuration.");
        string[] projects = RepositorySourceInventory.Projects(root).Where(project =>
        {
            string relative = Path.GetRelativePath(root, project).Replace('\\', '/');
            return (relative.StartsWith("src/", StringComparison.Ordinal) || relative.StartsWith("build/", StringComparison.Ordinal))
                && !RepositorySourceInventory.IsDeclaredTestProject(project);
        }).Order(StringComparer.Ordinal).ToArray();
        if (projects.Length == 0) throw new InvalidDataException("API export owner discovery is empty.");
        var properties = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["Configuration"] = configuration,
            ["DesignTimeBuild"] = "true"
        };
        if (options.TryGetValue("--artifacts-path", out string? artifacts))
            properties["ArtifactsPath"] = Path.GetFullPath(artifacts);
        // The operational Build entry supplies the existing SDK host selection. It is not
        // an API owner and must not acquire a documentation page merely to run extraction.
        string entry = Path.Combine(root, "entry", "InnoEngine.Build", "InnoEngine.Build.csproj");
        string[] graphProjects = File.Exists(entry) ? [..projects, entry] : projects;
        string graphText = await ManagedProjectGraphCapture.CaptureAsync(root, dotnet, graphProjects, properties,
            cancellationToken).ConfigureAwait(false);
        using JsonDocument graph = JsonDocument.Parse(graphText);
        var nodes = graph.RootElement.EnumerateArray().ToDictionary(node => Path.GetFullPath(node.GetProperty("Path").GetString()!),
            node => node, OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        string[] hosts = nodes.Values.Select(node => Property(node, "InnoDevelopmentTarget")).Where(value => value.Length != 0)
            .Distinct(StringComparer.Ordinal).ToArray();
        if (hosts.Length != 1) throw new InvalidDataException("The actual development entry must resolve one SDK host target.");
        string host = hosts[0];
        string[] runtimes = nodes.Values.Select(node => Property(node, "NETCoreSdkRuntimeIdentifier"))
            .Distinct(StringComparer.Ordinal).ToArray();
        if (runtimes.Length != 1 || runtimes[0].Length == 0)
            throw new InvalidDataException("The evaluated API graph does not identify one selected SDK runtime.");
        var owners = new List<object>();
        var builds = new SortedSet<string>(StringComparer.Ordinal);
        var captures = new SortedDictionary<string, List<string>>(StringComparer.Ordinal);
        var foreign = new List<object>();
        foreach (string project in projects)
        {
            cancellationToken.ThrowIfCancellationRequested();
            JsonElement node = nodes[project];
            string relative = Path.GetRelativePath(root, project).Replace('\\', '/');
            string target = Property(node, "InnoProductTarget");
            string language = Property(node, "LangVersion");
            if (!LanguageVersionFacts.TryParse(language, out LanguageVersion version))
                throw new InvalidDataException("Unsupported evaluated API language version: " + language);
            string[] sources = node.GetProperty("Items").GetProperty("Compile").EnumerateArray()
                .Select(item => item.GetProperty("FullPath").GetString()!).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var parse = new CSharpParseOptions(version,
                preprocessorSymbols: Property(node, "DefineConstants").Split(';', StringSplitOptions.RemoveEmptyEntries));
            bool empty = DocumentationProjectReferences.HasNoPublicTypes(sources, parse, cancellationToken);
            bool local = target.Length == 0 || target == host;
            string context = target.Length == 0 ? host : target;
            owners.Add(new { name = Path.GetFileNameWithoutExtension(project), path = relative, target,
                authoredSurfaceEmpty = empty, preparation = local ? "CurrentHost" : empty ? "EvaluatedEmptySurface" : "OtherHostRequired" });
            if (local || empty)
            {
                if (!captures.TryGetValue(context, out List<string>? batch)) captures.Add(context, batch = []);
                batch.Add(relative);
                if (!empty) builds.Add(relative);
            }
            else foreign.Add(new { path = relative, target, reason = "Requires its actual target SDK producer; another host cannot certify this nonempty API." });
        }
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(output, JsonSerializer.SerializeToUtf8Bytes(new
        {
            schema = "inno-api-export-plan/1", configuration, hostTarget = host, sdkRuntime = runtimes[0],
            projects = owners, buildProjects = builds,
            captures = captures.Select(pair => new { target = pair.Key, projects = pair.Value }),
            foreignOwners = foreign,
            scope = "Evaluated source export preparation only. Empty authored surfaces do not certify generators or product execution. Closing input checks and complete multi-host reconciliation remain required."
        }, new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Planned {projects.Length} eligible API owners for {host}; {builds.Count} managed preparations and {foreign.Count} nonempty owners require another host.");
        return 0;
    }

    private static string Property(
        JsonElement node,
        string name
    ) => node.GetProperty("Properties").GetProperty(name).GetString() ?? "";
}
