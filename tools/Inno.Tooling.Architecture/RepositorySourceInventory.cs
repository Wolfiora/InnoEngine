using System;
using System.Collections.Generic;
using System.IO;

namespace Inno.Tooling.Architecture;

internal static class RepositorySourceInventory
{
    private static readonly HashSet<string> ExcludedDirectories = new(StringComparer.OrdinalIgnoreCase)
    {
        "bin", "obj", "Generated", "extern", "artifacts", ".git"
    };

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
