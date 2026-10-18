using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.TaskHosting;

internal sealed record TaskRuntimeRequestSnapshot(
    string key,
    bool needsRestore,
    IReadOnlyDictionary<string, string> inputs,
    IReadOnlyList<string> declarations,
    IReadOnlyList<TaskRuntimeRestoreRequest> restoreRequests
) {
    internal string DescribeChanges(TaskRuntimeRequestSnapshot current)
    {
        string[] changed = inputs.Keys.Union(current.inputs.Keys, StringComparer.Ordinal)
            .Where(path => !inputs.TryGetValue(path, out string? planned)
                || !current.inputs.TryGetValue(path, out string? actual) || planned != actual).Take(8).ToArray();
        if (changed.Length > 0)
            return "Changed files: " + string.Join("; ", changed);
        return "Changed declarations: " + string.Join("; ", declarations.Except(current.declarations)
            .Concat(current.declarations.Except(declarations)).Take(8));
    }
}

internal sealed record TaskRuntimeRestoreRequest(
    string project,
    IReadOnlyDictionary<string, string> properties,
    IReadOnlyList<string> removedProperties
);
