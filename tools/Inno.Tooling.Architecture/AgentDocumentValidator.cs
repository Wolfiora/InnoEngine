using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inno.Tooling.Architecture;

internal static class AgentDocumentValidator
{
    internal static void Validate(
        string root,
        ICollection<string> failures
    ) {
        AgentLinkValidator.Validate(root, failures, ".agents");
        foreach (string entry in new[] { "AGENTS.md", "CONTRIBUTING.md", "README.md" })
            AgentLinkValidator.Validate(root, failures, entry);
        foreach (string skill in RepositorySourceInventory.Files(Path.Combine(root, ".agents", "skills"), "SKILL.md"))
        {
            string text = File.ReadAllText(skill).Replace("\r\n", "\n");
            string name = Path.GetFileName(Path.GetDirectoryName(skill)!);
            if (!text.StartsWith("---\n", StringComparison.Ordinal) || !text.Contains("\nname: " + name + "\n", StringComparison.Ordinal)
                || !Regex.IsMatch(text, @"(?m)^description: \S.+$") || text.IndexOf("\n---\n", 4, StringComparison.Ordinal) < 0)
                failures.Add("Invalid skill metadata: " + Path.GetRelativePath(root, skill));
        }
        foreach (string category in new[] { "rulebook", "records", "records/plans", "records/decisions", "records/acceptance", "records/history" })
            if (!File.Exists(Path.Combine(root, ".agents", category, "README.md")))
                failures.Add("Missing agent category navigation: " + category);
        if (Directory.Exists(Path.Combine(root, "records")) && Directory.EnumerateFiles(Path.Combine(root, "records"), "*", SearchOption.AllDirectories).Any())
            failures.Add("The retired root records owner still contains files.");
    }
}
