using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static class DocumentationPackageReferences
{
    internal static IReadOnlyList<string> Read(string projectDirectory)
    {
        string assets = Path.Combine(projectDirectory, "obj", "project.assets.json");
        if (!File.Exists(assets))
            return [];
        string project = Directory.EnumerateFiles(projectDirectory, "*.csproj").Single();
        string framework = XDocument.Load(project).Descendants("TargetFramework").FirstOrDefault()?.Value ?? "net9.0";
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(assets));
        JsonElement root = document.RootElement;
        JsonElement targets = root.GetProperty("targets");
        if (!targets.TryGetProperty(framework, out JsonElement target))
        {
            if (!root.TryGetProperty("project", out JsonElement restoredProject)
                || !restoredProject.TryGetProperty("frameworks", out JsonElement frameworks))
                throw new InvalidDataException($"Documentation restore inputs do not unambiguously describe '{framework}' for '{project}'.");
            JsonProperty[] matching = frameworks.EnumerateObject().Where(item => item.Name == framework
                || item.Value.TryGetProperty("targetAlias", out JsonElement alias) && alias.GetString() == framework).ToArray();
            if (matching.Length != 1)
                throw new InvalidDataException($"Documentation restore inputs do not select one '{framework}' framework for '{project}'.");
            if (!targets.TryGetProperty(matching[0].Name, out target))
            {
                // NuGet writes long names for netstandard targets. A single declared framework
                // and a single portable target establish the mapping without guessing a RID.
                JsonProperty[] portable = targets.EnumerateObject().Where(static item => !item.Name.Contains('/')).ToArray();
                if (frameworks.EnumerateObject().Count() != 1 || portable.Length != 1)
                    throw new InvalidDataException($"Documentation restore inputs do not select one portable target for '{project}'.");
                target = portable[0].Value;
            }
        }
        JsonElement libraries = root.GetProperty("libraries");
        string[] packageRoots = root.GetProperty("packageFolders").EnumerateObject()
            .Select(static folder => folder.Name).ToArray();
        var paths = new List<string>();
        foreach (JsonProperty dependency in target.EnumerateObject())
        {
            if (dependency.Value.GetProperty("type").GetString() != "package"
                || !dependency.Value.TryGetProperty("compile", out JsonElement compile))
                continue;
            string library = libraries.GetProperty(dependency.Name).GetProperty("path").GetString()!;
            foreach (JsonProperty input in compile.EnumerateObject())
            {
                if (!input.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    continue;
                string? path = packageRoots.Select(folder => Path.GetFullPath(Path.Combine(folder, library, input.Name)))
                    .FirstOrDefault(File.Exists);
                if (path is null)
                    throw new FileNotFoundException($"Documentation requires the restored compile asset '{dependency.Name}/{input.Name}'.");
                paths.Add(path);
            }
        }
        return paths;
    }
}
