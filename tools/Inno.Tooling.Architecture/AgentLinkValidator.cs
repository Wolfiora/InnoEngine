using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Inno.Tooling.Architecture;

internal static class AgentLinkValidator
{
    private static readonly MarkdownPipeline S_PIPELINE = new MarkdownPipelineBuilder()
        .UsePipeTables().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();

    internal static void Validate(
        string root,
        ICollection<string> failures,
        string contentRoot
    ) {
        var anchors = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        string location = Path.Combine(root, contentRoot);
        foreach (string page in File.Exists(location) ? [location] : RepositorySourceInventory.Files(location, "*.md"))
        {
            MarkdownDocument parsed = Markdown.Parse(File.ReadAllText(page), S_PIPELINE);
            foreach (string target in parsed.Descendants<LinkInline>().Where(static link => link.Url is not null)
                .Select(static link => link.Url!).Distinct(StringComparer.Ordinal))
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
                    failures.Add($"Agent document '{Path.GetRelativePath(root, page)}' has malformed link '{target}'.");
                    continue;
                }
                if (!File.Exists(path) && !Directory.Exists(path))
                {
                    failures.Add($"Agent document '{Path.GetRelativePath(root, page)}' links to missing path '{target}'.");
                    continue;
                }
                if (pieces.Length < 2 || pieces[1].Length == 0 || !path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!anchors.TryGetValue(path, out HashSet<string>? names))
                    anchors.Add(path, names = Anchors(File.ReadAllText(path)));
                if (!names.Contains(Uri.UnescapeDataString(pieces[1])))
                    failures.Add($"Agent document '{Path.GetRelativePath(root, page)}' links to missing anchor '{target}'.");
            }
        }
    }

    private static HashSet<string> Anchors(string text)
    {
        MarkdownDocument document = Markdown.Parse(text, S_PIPELINE);
        var result = document.Descendants<HeadingBlock>().Select(static heading => heading.GetAttributes().Id!)
            .ToHashSet(StringComparer.Ordinal);
        foreach (string html in document.Descendants<HtmlBlock>().Select(block => text[block.Span.Start..(block.Span.End + 1)])
            .Concat(document.Descendants<HtmlInline>().Select(static inline => inline.Tag)))
            foreach (Match match in Regex.Matches(html, "\\b(?:id|name)=[\"'](?<name>[^\"']+)[\"']"))
                result.Add(match.Groups["name"].Value);
        return result;
    }
}
