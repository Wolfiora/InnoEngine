using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Inno.Tooling.Validation;

internal static class ValidationCompiledGraph
{
    internal static IEnumerable<JsonElement> Select(
        ValidationPlan plan,
        ValidationCheck check
    ) {
        JsonElement[] snapshots = plan.contexts[check.context ?? "managed"].sdkGraph.EnumerateArray().ToArray();
        var instances = snapshots.SelectMany(static snapshot => snapshot.GetProperty("Instances").EnumerateArray())
            .ToDictionary(static instance => instance.GetProperty("Identity").GetString()!, StringComparer.Ordinal);
        string[] roots = check.kind is "build" or "test" ? [check.owner]
            : check.projects ?? check.inputs.Where(static input => input.StartsWith('@') && input is not ("@tooling" or "@native-sources"))
                .Select(static input => input[1..]).ToArray();
        var pending = new Queue<string>();
        foreach (string root in roots)
        {
            JsonElement snapshot = snapshots.Single(snapshot =>
                Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == root);
            JsonElement properties = snapshot.GetProperty("GlobalProperties");
            JsonElement entry = snapshot.GetProperty("Instances").EnumerateArray().Single(instance =>
            {
                JsonElement effective = instance.GetProperty("GlobalProperties");
                return effective.EnumerateObject().Count(static pair => pair.Name != "IsGraphBuild") == properties.EnumerateObject().Count()
                    && properties.EnumerateObject().All(pair => effective.TryGetProperty(pair.Name, out JsonElement value)
                        && value.GetString() == pair.Value.GetString());
            });
            pending.Enqueue(entry.GetProperty("Identity").GetString()!);
        }
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (pending.TryDequeue(out string? identity))
        {
            if (!visited.Add(identity))
                continue;
            if (!instances.TryGetValue(identity, out JsonElement instance))
                throw new InvalidDataException("An actual compiled project instance has no captured owner: " + identity);
            yield return instance;
            foreach (JsonElement reference in instance.GetProperty("References").EnumerateArray())
                pending.Enqueue(reference.GetString()!);
        }
    }
}
