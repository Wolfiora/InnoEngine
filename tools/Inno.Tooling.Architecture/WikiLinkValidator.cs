using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Inno.Tooling.Architecture.Documentation;
using Markdig;
using Markdig.Syntax;

namespace Inno.Tooling.Architecture;

internal static class WikiLinkValidator
{
    internal static void Validate(
        string root,
        ICollection<string> failures,
        string contentRoot = "docs"
    ) {
        var anchors = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        string location = Path.Combine(root, contentRoot);
        foreach (string page in File.Exists(location) ? [location] : RepositorySourceInventory.Files(location, "*.md"))
        {
            DocumentationPage parsed = DocumentationMarkdown.Parse(page, File.ReadAllText(page), includeSymbols: false);
            foreach (string target in parsed.links)
            {
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
                    anchors.Add(path, names = DocumentationMarkdown.Anchors(File.ReadAllText(path)));
                if (!names.Contains(Uri.UnescapeDataString(pieces[1])))
                    failures.Add($"Wiki '{Path.GetRelativePath(root, page)}' links to missing anchor '{target}'.");
            }
        }
    }

    internal static string WithoutFences(string markdown)
    {
        char[] text = markdown.ToCharArray();
        foreach (CodeBlock block in DocumentationMarkdown.ParseDocument(markdown).Descendants<CodeBlock>())
            for (int index = block.Span.Start; index <= block.Span.End; index++)
                if (text[index] is not ('\r' or '\n'))
                    text[index] = ' ';
        return new string(text);
    }
}
