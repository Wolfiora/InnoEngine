using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static class RepositorySourceInventory
{
    private static readonly HashSet<string> ExcludedDirectories = new(StringComparer.OrdinalIgnoreCase)
    {
        "bin", "obj", "Generated", "extern", "dependencies", "artifacts", ".git"
    };

    internal static IEnumerable<string> Projects(string root)
    {
        foreach (string owner in new[] { "entry", "src", "backends", "platforms", "build", "tools", "tests" })
            foreach (string project in Files(Path.Combine(root, owner), "*.csproj"))
                yield return project;
    }

    internal static bool IsDeclaredTestProject(string project)
    {
        XElement? declaration = XDocument.Load(project).Descendants("IsTestProject").LastOrDefault();
        // Only explicit unconditional test runners exempt their public test methods; support libraries retain API contracts.
        return declaration?.Attribute("Condition") is null && declaration?.Parent?.Attribute("Condition") is null
            && declaration?.Value.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
    }

    internal static IEnumerable<string> Files(
        string root,
        string pattern
    ) {
        if (!Directory.Exists(root))
            yield break;
        foreach (string file in Directory.EnumerateFiles(root, pattern))
            yield return file;
        foreach (string directory in Directories(root))
            foreach (string file in Directory.EnumerateFiles(directory, pattern))
                yield return file;
    }

    internal static IEnumerable<string> Directories(string root)
    {
        var pending = new Stack<string>();
        pending.Push(root);
        while (pending.Count != 0)
        {
            foreach (string child in Directory.EnumerateDirectories(pending.Pop()))
            {
                if (ExcludedDirectories.Contains(Path.GetFileName(child)))
                    continue;
                yield return child;
                pending.Push(child);
            }
        }
    }
}
