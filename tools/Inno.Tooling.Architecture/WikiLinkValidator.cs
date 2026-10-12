using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inno.Tooling.Architecture;

internal static class WikiLinkValidator
{
    private static readonly Regex S_LINK = new(@"!?\[[^\]\r\n]*\]\((?<target><[^>]+>|[^)\r\n]+)\)", RegexOptions.CultureInvariant);
    private static readonly Regex S_REFERENCE = new(@"^\s{0,3}\[[^\]]+\]:\s*(?<target><[^>]+>|\S+)", RegexOptions.CultureInvariant | RegexOptions.Multiline);
    private static readonly Regex S_HEADING = new(@"^ {0,3}#{1,6}\s+(?<text>.+?)(?:\s+#+)?$", RegexOptions.CultureInvariant | RegexOptions.Multiline);
    private static readonly Regex S_EXPLICIT_ANCHOR = new("\\b(?:id|name)=[\"'](?<name>[^\"']+)[\"']", RegexOptions.CultureInvariant);

    internal static void Validate(
        string root,
        ICollection<string> failures
    ) {
        var anchors = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (string page in RepositorySourceInventory.Files(Path.Combine(root, "docs"), "*.md"))
        {
            string source = WithoutFences(File.ReadAllText(page));
            foreach (Match match in S_LINK.Matches(source).Concat(S_REFERENCE.Matches(source)))
            {
                string target = match.Groups["target"].Value.Trim();
                target = target.StartsWith('<') ? target[1..target.IndexOf('>')] : target.Split(" \"", StringSplitOptions.None)[0];
                if (target.Contains("://", StringComparison.Ordinal) || target.StartsWith("mailto:", StringComparison.Ordinal)
                    || target.StartsWith("data:", StringComparison.Ordinal))
                    continue;
                string[] pieces = target.Split('#', 2);
                string path;
                try
                {
                    path = pieces[0].Length == 0 ? page
                        : Path.GetFullPath(Uri.UnescapeDataString(pieces[0]), Path.GetDirectoryName(page)!);
                }
                catch (ArgumentException)
                {
                    failures.Add($"Wiki '{Path.GetRelativePath(root, page)}' has malformed link '{target}'.");
                    continue;
                }
                if (!File.Exists(path) && !Directory.Exists(path))
                {
                    failures.Add($"Wiki '{Path.GetRelativePath(root, page)}' links to missing path '{target}'.");
                    continue;
                }
                if (pieces.Length < 2 || pieces[1].Length == 0 || !path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!anchors.TryGetValue(path, out HashSet<string>? names))
                    anchors.Add(path, names = ReadAnchors(File.ReadAllText(path)));
                if (!names.Contains(Uri.UnescapeDataString(pieces[1])))
                    failures.Add($"Wiki '{Path.GetRelativePath(root, page)}' links to missing anchor '{target}'.");
            }
        }
    }

    internal static string WithoutFences(string markdown)
    {
        string[] lines = markdown.Split('\n');
        char fence = '\0';
        int width = 0;
        for (int index = 0; index < lines.Length; index++)
        {
            string value = lines[index].TrimStart();
            int count = value.TakeWhile(character => character == '`' || character == '~').Count();
            if (fence == '\0' && count >= 3 && value.Take(count).All(character => character == value[0]))
            {
                fence = value[0];
                width = count;
                lines[index] = string.Empty;
            }
            else if (fence != '\0')
            {
                if (value.Length >= width && value.Take(width).All(character => character == fence)
                    && value.Trim(fence, ' ', '\r').Length == 0)
                    fence = '\0';
                lines[index] = string.Empty;
            }
        }
        return string.Join('\n', lines);
    }

    private static HashSet<string> ReadAnchors(string markdown)
    {
        string text = WithoutFences(markdown);
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (Match match in S_EXPLICIT_ANCHOR.Matches(text))
            names.Add(match.Groups["name"].Value);
        var occurrences = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (Match match in S_HEADING.Matches(text))
        {
            string heading = match.Groups["text"].Value.Trim().ToLowerInvariant();
            heading = Regex.Replace(heading, "<[^>]*>", string.Empty);
            heading = Regex.Replace(heading, @"[^\p{L}\p{M}\p{N}\p{Pc}\- ]", string.Empty).Replace(' ', '-');
            int suffix = occurrences.GetValueOrDefault(heading);
            occurrences[heading] = suffix + 1;
            names.Add(suffix == 0 ? heading : heading + "-" + suffix);
        }
        return names;
    }
}
