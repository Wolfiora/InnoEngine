using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Inno.Tooling.Architecture;

internal static class WikiContentBoundaryValidator
{
    private static readonly Regex S_EXTERNAL_CONSUMERS = new(
        @"\bInno(?:Engine)?[./\\](?:Canvas|Rendering2D|Samples)\b|\bRendering2D\b|\bFlappyBird\b|\bCamera2D\b|\bSpriteShaderPreview\b",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    private static readonly Regex S_REMOVED_ENTRIES = new(
        @"InnoEngine (?:Editor|Rebuild)|InnoEngine\.Rebuild|entry[/\\](?:Editor|Rebuild)[/\\]|RunRebuildAsync|-- (?:engine|rebuild)\b|\b(?:Inno\.Build\.Cli|InnoEngine\.Build)(?:\.dll)?\s+(?:engine|rebuild)\b|\bdotnet\s+(?:[^\r\n]*[/\\])?Inno\.Build\.Cli\.dll\b",
        RegexOptions.CultureInvariant);

    internal static void Validate(
        string root,
        ICollection<string> failures
    ) {
        foreach (string page in RepositorySourceInventory.Files(Path.Combine(root, "docs"), "*.md"))
        {
            string source = File.ReadAllText(page);
            foreach (Regex rule in new[] { S_EXTERNAL_CONSUMERS, S_REMOVED_ENTRIES })
                foreach (Match match in rule.Matches(source))
                    failures.Add($"Wiki '{Path.GetRelativePath(root, page)}' contains a removed entry or external consumer reference: '{match.Value}'.");
        }
    }
}
