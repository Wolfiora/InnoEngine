using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Inno.Tooling.Architecture;

internal static class DocumentationExportInputs
{
    internal static IReadOnlyDictionary<string, string> Capture(
        string root,
        IReadOnlyDictionary<string, JsonElement> nodes
    ) {
        var result = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var local = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach ((string project, JsonElement node) in nodes)
        {
            var inputs = new SortedDictionary<string, string>(StringComparer.Ordinal);
            void AddFile(string file)
            {
                if (!Path.IsPathFullyQualified(file)) file = Path.GetFullPath(file, Path.GetDirectoryName(project)!);
                string relative = Path.GetRelativePath(root, file).Replace('\\', '/');
                if (!File.Exists(file))
                {
                    inputs[relative] = "Missing";
                    return;
                }
                string content = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(file)));
                inputs[relative.StartsWith("../", StringComparison.Ordinal)
                       ? "external/" + Path.GetFileName(file) + "/" + content
                       : relative] = content;
            }
            AddFile(project);
            AddFile(Path.Combine(root, "global.json"));
            foreach (JsonElement import in node.GetProperty("Imports").EnumerateArray())
            {
                AddFile(import.GetString()!);
            }
            foreach (JsonProperty kind in node.GetProperty("Items").EnumerateObject())
            {
            foreach (JsonElement item in kind.Value.EnumerateArray())
            {
                if (item.TryGetProperty("FullPath", out JsonElement full)) AddFile(full.GetString()!);
            }
            }
            // The evaluated item set detects additions, deletions and glob expansion. Normalize
            // checkout location only; actual SDK/package/target properties remain inputs.
            string graph = node.GetRawText()
                .Replace(root.Replace('\\', '/'), "<engine>", StringComparison.OrdinalIgnoreCase)
                .Replace(JsonEncodedText.Encode(root).ToString(), "<engine>", StringComparison.OrdinalIgnoreCase);
            inputs["evaluation"] = graph;
            local[project] = Convert.ToHexStringLower(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(inputs)));
        }
        foreach (string project in nodes.Keys)
        {
            var closure = new SortedDictionary<string, string>(StringComparer.Ordinal);
            void Visit(string current)
            {
                string relative = Path.GetRelativePath(root, current).Replace('\\', '/');
                if (closure.ContainsKey(relative)) return;
                if (!nodes.TryGetValue(current, out JsonElement node))
                {
                    throw new InvalidDataException("Missing evaluated API dependency: " + relative);
                }
                closure[relative] = local[current];
                foreach (JsonElement reference in node.GetProperty("Items").GetProperty("ProjectReference").EnumerateArray())
                {
                    Visit(Path.GetFullPath(reference.GetProperty("FullPath").GetString()!));
                }
            }
            Visit(project);
            result[Path.GetRelativePath(root, project)
                .Replace('\\', '/')] = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(closure))));
        }
        return result;
    }
}
