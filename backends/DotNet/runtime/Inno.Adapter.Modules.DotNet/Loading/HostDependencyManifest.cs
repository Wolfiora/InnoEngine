using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Inno.Adapter.Modules.DotNet;

internal static class HostDependencyManifest
{
    internal static IReadOnlyList<AssemblyName> GetInnoRuntimeAssemblies(IReadOnlyList<Assembly> rootAssemblies)
    {
        ArgumentNullException.ThrowIfNull(rootAssemblies);

        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string dependencyFile in GetDependencyFiles(rootAssemblies))
            ReadRuntimeAssemblyNames(dependencyFile, names);
        return names
            .OrderBy(static name => name, StringComparer.OrdinalIgnoreCase)
            .Select(static name => new AssemblyName(name))
            .ToArray();
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000", Justification =
        "Bundled assemblies have no sidecar manifest; empty locations are skipped and CLR references remain available.")]
    private static IReadOnlyList<string> GetDependencyFiles(IReadOnlyList<Assembly> rootAssemblies)
    {
        var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Assembly rootAssembly in rootAssemblies)
        {
            string location = rootAssembly.Location;
            if (string.IsNullOrWhiteSpace(location))
                continue;
            string dependencyFile = Path.ChangeExtension(location, ".deps.json");
            if (File.Exists(dependencyFile))
                paths.Add(Path.GetFullPath(dependencyFile));
        }
        return paths.ToArray();
    }

    private static void ReadRuntimeAssemblyNames(
        string dependencyFile,
        ISet<string> names
    ) {
        try
        {
            using FileStream stream = File.OpenRead(dependencyFile);
            using JsonDocument document = JsonDocument.Parse(stream);
            if (!document.RootElement.TryGetProperty("targets", out JsonElement targets))
                return;

            foreach (JsonProperty target in targets.EnumerateObject())
            {
                foreach (JsonProperty library in target.Value.EnumerateObject())
                {
                    if (!library.Value.TryGetProperty("runtime", out JsonElement runtime))
                        continue;
                    foreach (JsonProperty asset in runtime.EnumerateObject())
                    {
                        string fileName = Path.GetFileName(asset.Name);
                        if (!fileName.StartsWith("Inno.", StringComparison.Ordinal) ||
                            !fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        names.Add(Path.GetFileNameWithoutExtension(fileName));
                    }
                }
            }
        }
        catch (IOException)
        {
            // Dependency manifests are an optional host discovery source.
        }
        catch (UnauthorizedAccessException)
        {
            // The regular CLR reference graph remains available when a manifest cannot be read.
        }
        catch (JsonException)
        {
            // Ignore an invalid optional manifest and continue with the CLR reference graph.
        }
    }
}
