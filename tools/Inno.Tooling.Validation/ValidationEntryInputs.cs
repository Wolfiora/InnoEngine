using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Inno.Tooling.Validation;

internal sealed class ValidationEntryInputs
{
    private readonly Dictionary<string, string> m_roots = new(StringComparer.Ordinal);
    private readonly Dictionary<string, (string key, JsonElement variant, string[] references)> m_nodes = new(StringComparer.Ordinal);

    internal ValidationEntryInputs(
        string root,
        JsonElement graph,
        string context,
        Dictionary<string, string[]> sets,
        Func<IEnumerable<string>, string[]> capture
    ) {
        foreach (JsonElement snapshot in graph.EnumerateArray())
        {
            string owner = Path.GetRelativePath(root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/');
            m_roots.Add(owner, ValidationCompiledGraph.Entry(snapshot).GetProperty("Identity").GetString()!);
            JsonElement[] variants = snapshot.GetProperty("Variants").EnumerateArray().Prepend(snapshot).ToArray();
            for (int index = 0; index < variants.Length; index++)
            {
                JsonElement variant = variants[index];
                string key = context + "/" + owner + "/" + index;
                sets.Add(key, capture(ValidationProject.Read(root, snapshot, selectedVariant: variant).inputs));
                foreach (JsonElement instance in snapshot.GetProperty("Instances").EnumerateArray())
                {
                    JsonElement properties = instance.GetProperty("GlobalProperties");
                    bool entry = ValidationCompiledGraph.SameProperties(snapshot.GetProperty("GlobalProperties"), properties);
                    if (entry ? index != 0 : !variant.GetProperty("EquivalentGlobalProperties")
                        .EnumerateArray()
                        .Any(candidate => ValidationCompiledGraph.SameProperties(candidate, properties))
                    ) {
                        continue;
                    }
                    m_nodes.Add(
                        instance.GetProperty("Identity").GetString()!,
                        (key, variant, instance.GetProperty("References")
                            .EnumerateArray()
                            .Select(static item => item.GetString()!)
                            .ToArray())
                    );
                }
            }
        }
    }

    internal string Entry(string owner) => m_roots[owner];

    internal (string key, JsonElement variant, string[] references) Instance(string id) => m_nodes[id];

    internal string[] Sources(string owner)
    {
        var keys = new HashSet<string>(StringComparer.Ordinal);
        var visited = new HashSet<string>(StringComparer.Ordinal);
        var pending = new Stack<string>();
        pending.Push(m_roots[owner]);
        while (pending.TryPop(out string? id))
        {
            if (!visited.Add(id))
            {
                continue;
            }
            var node = m_nodes[id];
            keys.Add(node.key);
            foreach (string reference in node.references)
            {
                pending.Push(reference);
            }
        }
        return keys.Order(StringComparer.Ordinal).ToArray();
    }
}
