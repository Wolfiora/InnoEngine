using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;
using Microsoft.Build.Execution;

namespace Inno.Build.TaskHosting;

internal static class TaskRuntimePackageInputs
{
    private static readonly string[] S_ASSET_GROUPS =
    [
        "compile", "runtime", "native", "resource", "runtimeTargets",
        "build", "buildMultiTargeting", "buildTransitive", "contentFiles"
    ];

    internal static void Collect(
        ProjectInstance project,
        ISet<string> inputs,
        CancellationToken cancellation
    ) {
        string assets = project.GetPropertyValue("ProjectAssetsFile");
        if (assets.Length == 0)
            return;
        assets = Path.GetFullPath(assets, Path.GetDirectoryName(project.FullPath)!);
        if (!File.Exists(assets))
            return;
        inputs.Add(assets);
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(assets));
        JsonElement root = document.RootElement;
        JsonElement targets = root.GetProperty("targets");
        string framework = project.GetPropertyValue("TargetFramework");
        string moniker = project.GetPropertyValue("TargetFrameworkMoniker");
        string runtime = project.GetPropertyValue("RuntimeIdentifier");
        string suffix = runtime.Length == 0 ? string.Empty : "/" + runtime;
        if (!targets.TryGetProperty(framework + suffix, out JsonElement target)
            && !targets.TryGetProperty(moniker + suffix, out target))
            throw new InvalidDataException($"Task runtime restore does not contain the selected target '{framework}{suffix}' for '{project.FullPath}'.");
        JsonElement libraries = root.GetProperty("libraries");
        string[] folders = root.GetProperty("packageFolders").EnumerateObject()
            .Select(static folder => folder.Name).ToArray();
        foreach (JsonProperty dependency in target.EnumerateObject())
        {
            cancellation.ThrowIfCancellationRequested();
            if (dependency.Value.GetProperty("type").GetString() != "package")
                continue;
            JsonElement library = libraries.GetProperty(dependency.Name);
            string relative = library.GetProperty("path").GetString()
                ?? throw new InvalidDataException("A restored task package has no location.");
            string? directory = folders.Select(folder => PathBoundary.Resolve(folder, relative))
                .FirstOrDefault(Directory.Exists);
            if (directory is null)
                throw new DirectoryNotFoundException($"The task runtime package '{dependency.Name}' is absent from its restored package roots.");
            var selected = new HashSet<string>(StringComparer.Ordinal);
            foreach (string group in S_ASSET_GROUPS)
                if (dependency.Value.TryGetProperty(group, out JsonElement entries))
                    foreach (JsonProperty entry in entries.EnumerateObject())
                        selected.Add(entry.Name);
            // Analyzer assemblies are selected from the package file inventory by the SDK,
            // rather than represented as a separate target asset group.
            foreach (JsonElement entry in library.GetProperty("files").EnumerateArray())
            {
                string path = entry.GetString()!;
                if (path.StartsWith("analyzers/", StringComparison.Ordinal)
                    && !path.Contains("/vb/", StringComparison.Ordinal)
                    && !path.Contains("/fs/", StringComparison.Ordinal))
                    selected.Add(path);
            }
            foreach (string relativeInput in selected)
            {
                cancellation.ThrowIfCancellationRequested();
                if (Path.GetFileName(relativeInput) == "_._")
                    continue;
                string input = PathBoundary.Resolve(directory, relativeInput);
                if (!File.Exists(input))
                    throw new FileNotFoundException($"The restored task runtime input '{dependency.Name}/{relativeInput}' is absent.", input);
                inputs.Add(input);
            }
        }
    }
}
