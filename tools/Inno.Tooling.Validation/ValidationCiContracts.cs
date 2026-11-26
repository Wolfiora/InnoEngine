using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inno.Tooling.Validation;

internal static class ValidationCiContracts
{
    internal const string C_RULE_LOADER = "tools/ci/Test-AgentRules.ps1";
    internal const string C_RULE_EXAMPLES = "tests/tools/Inno.Tooling.Architecture.Integration.Tests/fixtures/rule-selection.json";

    internal static Dictionary<string, string[]> Select(
        string root,
        IEnumerable<string> files,
        HashSet<string> changes,
        bool full,
        out HashSet<string> owned
    ) {
        string[] paths = files
            .Where(path => (
                path.StartsWith("tools/ci/", StringComparison.Ordinal)
                || path.StartsWith("tests/ci/", StringComparison.Ordinal) || path.StartsWith(".github/", StringComparison.Ordinal)
            ))
            .Where(path => File.Exists(Path.Combine(root, path)))
            .ToArray();
        ILookup<string, string> scripts = paths.Where(path => path.EndsWith(".ps1", StringComparison.Ordinal))
            .ToLookup(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase);
        owned = new HashSet<string>(StringComparer.Ordinal)
        {
            ".github/scripts/github_artifacts.py",
            C_RULE_LOADER,
            C_RULE_EXAMPLES,
            ValidationRunner.C_BUILD_CACHE,
            ValidationRunner.C_SUPPORT_PACK,
            ValidationRunner.C_TEST_EXECUTOR
        };
        var references = new Dictionary<string, string[]>(StringComparer.Ordinal);
        var selected = new Dictionary<string, string[]>(StringComparer.Ordinal);
        foreach (string test in paths.Where(path => (
            path.EndsWith(".Tests.ps1", StringComparison.Ordinal)
            || path.StartsWith(".github/", StringComparison.Ordinal) && path.EndsWith(".yml", StringComparison.Ordinal)
        ))
        ) {
            var visited = new HashSet<string>(StringComparer.Ordinal);
            var pending = new Stack<string>();
            pending.Push(test);
            while (pending.TryPop(out string? script))
            {
                if (!visited.Add(script))
                {
                    continue;
                }
                if (!references.TryGetValue(script, out string[]? dependencies))
                {
                    string source = File.ReadAllText(Path.Combine(root, script));
                    dependencies = Regex.Matches(source, @"[\w.-]+\.ps1\b", RegexOptions.IgnoreCase)
                        .SelectMany(match => scripts[match.Value])
                        .Concat(
                            paths.Where(path => (
                                path.StartsWith(".github/", StringComparison.Ordinal)
                                && source.Contains(path, StringComparison.Ordinal)
                            ))
                        )
                        .Distinct(StringComparer.Ordinal)
                        .ToArray();
                    references.Add(script, dependencies);
                }
                foreach (string dependency in dependencies)
                {
                    pending.Push(dependency);
                }
            }
            owned.UnionWith(visited);
            if (test.StartsWith("tests/ci/", StringComparison.Ordinal) && (full || visited.Overlaps(changes)))
            {
                selected.Add(test, visited.Order(StringComparer.Ordinal).ToArray());
            }
        }
        return selected;
    }
}
