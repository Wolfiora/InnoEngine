using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Checks handwritten C# against the style rulebook and applies its automatic fixes.
/// </summary>
public static class StyleCommand
{
    /// <summary>
    /// Runs <c>check</c> or <c>fix</c> for a repository root.
    /// </summary>
    /// <param name="arguments">
    /// The operation, the repository root, and the optional <c>--rule</c>, <c>--paths-from</c> and <c>--report</c> options.
    /// </param>
    /// <returns>
    /// Zero when no finding and no rejected fix remains; otherwise one.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The arguments do not name an operation and a repository root, or an option is unknown.
    /// </exception>
    public static int Execute(string[] arguments)
    {
        if (arguments.Length < 2 || arguments[0] is not ("check" or "fix"))
        {
            throw new ArgumentException("Usage: style <check|fix> <root> [--rule ID]... [--paths-from FILE] [--report FILE]");
        }

        string root = Path.GetFullPath(arguments[1]);
        var ruleIds = new List<string>();
        string? pathsFrom = null;
        string? report = null;
        for (int index = 2; index < arguments.Length; index += 2)
        {
            if (index + 1 >= arguments.Length)
            {
                throw new ArgumentException($"The option '{arguments[index]}' requires a value.");
            }

            string value = arguments[index + 1];
            switch (arguments[index])
            {
                case "--rule":
                    ruleIds.Add(value);
                    break;
                case "--paths-from":
                    pathsFrom = value;
                    break;
                case "--report":
                    report = value;
                    break;
                default:
                    throw new ArgumentException($"Unknown option '{arguments[index]}'.");
            }
        }

        string[] paths = pathsFrom is null
                         ? StyleEngine.EnumerateSources(root).ToArray()
                         : File.ReadAllLines(pathsFrom)
                             .Where(static line => line.Length > 0)
                             .Select(line => Path.Combine(root, line))
                             .ToArray();
        StyleReport result = StyleEngine.Run(root, paths, ruleIds, fix: arguments[0] == "fix");
        if (report is not null)
        {
            IEnumerable<string> lines = result.findings
                .Select(static finding => $"{finding.rule}\t{finding.path}\t{finding.line}\t{finding.message}")
                .Concat(
                    result.rejections.Select(static rejection => $"REJECTED:{rejection.rule}\t{rejection.path}\t0\t{rejection.reason}")
                );
            File.WriteAllLines(report, lines);
        }

        Console.WriteLine(
            $"Files: {result.files}. Changed: {result.changedFiles}. Findings: {result.findings.Count}. Rejected fixes: {result.rejections.Count}."
        );
        foreach (IGrouping<string, StyleFinding> group in result.findings.GroupBy(static finding => finding.rule))
        {
            Console.WriteLine($"{group.Key}\t{group.Count()}\t{group.Select(static finding => finding.path).Distinct().Count()} files");
        }

        return result.findings.Count == 0 && result.rejections.Count == 0 ? 0 : 1;
    }
}
