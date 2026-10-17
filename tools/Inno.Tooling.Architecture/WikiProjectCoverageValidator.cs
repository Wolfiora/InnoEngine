using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Tooling.Architecture;

internal static class WikiProjectCoverageValidator
{
    internal static IReadOnlyList<WikiProjectPage> Validate(
        string root,
        ICollection<string> failures
    ) {
        string[] pages = RepositorySourceInventory.Files(Path.Combine(root, "docs"), "*.md").ToArray();
        var result = new List<WikiProjectPage>();
        foreach (string project in new[] { "entry", "src", "backends", "platforms", "build", "tools", "tests" }
            .SelectMany(folder => RepositorySourceInventory.Files(Path.Combine(root, folder), "*.csproj")))
        {
            string name = Path.GetFileNameWithoutExtension(project);
            string[] matches = pages.Where(page => Path.GetFileNameWithoutExtension(page) == name).ToArray();
            string relative = Path.GetRelativePath(root, project).Replace('\\', '/');
            bool test = relative.StartsWith("tests/", StringComparison.Ordinal)
                || relative.Contains("/tests/", StringComparison.Ordinal);
            if (matches.Length != 1)
            {
                failures.Add($"Wiki project '{relative}' requires one authoritative page; found {matches.Length}.");
                continue;
            }
            string page = matches[0];
            string index = Path.Combine(Path.GetDirectoryName(page)!, "README.md");
            if (!File.Exists(index) || !File.ReadAllText(index).Contains(Path.GetFileName(page), StringComparison.Ordinal))
                failures.Add($"Wiki page '{Path.GetRelativePath(root, page)}' is absent from its category index.");
            result.Add(new WikiProjectPage(project, page, test));
        }
        return result;
    }
}

internal sealed record WikiProjectPage(
    string project,
    string page,
    bool isTest
);
