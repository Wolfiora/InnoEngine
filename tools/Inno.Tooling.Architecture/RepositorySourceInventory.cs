using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static class RepositorySourceInventory
{
    private static readonly HashSet<string> EXCLUDED_DIRECTORIES = new(StringComparer.OrdinalIgnoreCase)
    {
        "bin", "obj", "Generated", "extern", "artifacts", ".git"
    };

    internal static bool IsTestPath(string relative)
        => relative.Replace('\\', '/').StartsWith("tests/", StringComparison.Ordinal);

    internal static IEnumerable<string> Projects(string root) => Files(root, "*.csproj");

    internal static bool DeclaresTestSupport(XDocument project)
        => project.Descendants().Any(static item => (
            item.Name.LocalName is "IsTestProject" or "IsTestSupportProject"
            && item.Value.Equals("true", StringComparison.OrdinalIgnoreCase)
            || item.Name.LocalName == "PackageReference"
               && IsTestPackage(item.Attribute("Include")?.Value ?? item.Attribute("Update")?.Value ?? string.Empty)
        ));

    internal static bool IsTestPackage(string package)
        => package.Equals("Microsoft.NET.Test.Sdk", StringComparison.OrdinalIgnoreCase)
           || package.Equals("xunit", StringComparison.OrdinalIgnoreCase)
           || package.StartsWith("xunit.v3", StringComparison.OrdinalIgnoreCase)
           || package.Equals("NUnit", StringComparison.OrdinalIgnoreCase)
           || package.Equals("MSTest.TestFramework", StringComparison.OrdinalIgnoreCase);

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
        {
            yield break;
        }
        foreach (string file in Directory.EnumerateFiles(root, pattern))
        {
            yield return file;
        }
        foreach (string directory in Directories(root))
        {
            foreach (string file in Directory.EnumerateFiles(directory, pattern))
            {
                yield return file;
            }
        }
    }

    internal static IEnumerable<string> Directories(string root)
    {
        string? localBenchmarks = File.Exists(Path.Combine(root, "InnoEngine.sln")) ? Path.Combine(root, "benchmarks") : null;
        var pending = new Stack<string>();
        pending.Push(root);
        while (pending.Count != 0)
        {
            foreach (string child in Directory.EnumerateDirectories(pending.Pop()))
            {
                if (EXCLUDED_DIRECTORIES.Contains(Path.GetFileName(child)) || child == localBenchmarks)
                {
                    continue;
                }
                yield return child;
                pending.Push(child);
            }
        }
    }
}
