using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static class WikiProjectCoverageValidator
{
    internal static IReadOnlyList<WikiProjectPage> Validate(
        string root,
        ICollection<string> failures,
        bool requireCompleteCoverage = true
    ) {
        string[] pages = RepositorySourceInventory.Files(Path.Combine(root, "docs"), "*.md").ToArray();
        var result = new List<WikiProjectPage>();
        foreach (string project in RepositorySourceInventory.Projects(root))
        {
            string name = Path.GetFileNameWithoutExtension(project);
            string[] matches = pages.Where(page => Path.GetFileNameWithoutExtension(page) == name).ToArray();
            string relative = Path.GetRelativePath(root, project).Replace('\\', '/');
            bool test = IsDeclaredTestProject(project);
            if (matches.Length != 1)
            {
                if (requireCompleteCoverage || matches.Length != 0)
                    failures.Add($"Wiki project '{relative}' requires one authoritative page; found {matches.Length}.");
                continue;
            }
            string page = matches[0];
            string index = Path.Combine(Path.GetDirectoryName(page)!, "README.md");
            if (requireCompleteCoverage && (!File.Exists(index) || !File.ReadAllText(index).Contains(Path.GetFileName(page), StringComparison.Ordinal)))
                failures.Add($"Wiki page '{Path.GetRelativePath(root, page)}' is absent from its category index.");
            result.Add(new WikiProjectPage(project, page, test));
        }
        return result;
    }

    internal static bool IsDeclaredTestProject(string project)
    {
        XElement? declaration = XDocument.Load(project).Descendants("IsTestProject").LastOrDefault();
        // Only an explicit unconditional test-runner declaration exempts an assembly's public test methods.
        // Conditional or inherited ambiguity must not silently remove fixture contracts from documentation.
        return declaration?.Attribute("Condition") is null && declaration?.Parent?.Attribute("Condition") is null
            && declaration?.Value.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
    }
}

internal sealed record WikiProjectPage(
    string project,
    string page,
    bool isTest
);
