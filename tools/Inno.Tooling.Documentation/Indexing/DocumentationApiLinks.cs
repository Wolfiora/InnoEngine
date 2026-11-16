using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationApiLinks
{
    internal static string Rewrite(
        string root,
        string originalPath,
        string destinationPath,
        string text,
        IReadOnlyDictionary<string, string> moves,
        IReadOnlyDictionary<string, string> anchors
    ) {
        MarkdownDocument document = Markdown.Parse(text, new MarkdownPipelineBuilder().UseYamlFrontMatter().UsePreciseSourceLocation().EnableTrackTrivia().Build());
        var links = document.Descendants<LinkInline>().Where(link => link.Url is not null && link.Reference is null)
            .Select(link => (url: link.Url!, span: link.UrlSpan))
            .Concat(document.Descendants<LinkReferenceDefinition>().Where(link => link.Url is not null)
                .Select(link => (url: link.Url!, span: link.UrlSpan)));
        var changes = new List<(int start, int length, string value)>();
        foreach (var link in links)
        {
            if (Uri.TryCreate(link.url, UriKind.Absolute, out _) || link.url.StartsWith('/')) continue;
            string[] parts = link.url.Split('#', 2);
            string target = parts[0].Length == 0 ? originalPath : Path.GetRelativePath(root,
                Path.GetFullPath(Uri.UnescapeDataString(parts[0]), Path.GetDirectoryName(Path.Combine(root, originalPath))!)).Replace('\\', '/');
            string suffix = parts.Length == 2 ? "#" + parts[1] : "";
            string mapped = anchors.GetValueOrDefault(target + suffix) ?? moves.GetValueOrDefault(target) ?? target;
            if (mapped == target && originalPath == destinationPath) continue;
            // Moving prose may rebase local references; only paths are changed, never its explanation.
            string url = mapped == destinationPath && suffix.Length != 0 ? suffix
                : Path.GetRelativePath(Path.GetDirectoryName(destinationPath)!, mapped).Replace('\\', '/') + suffix;
            if (link.span.Start < 0 || link.span.End < link.span.Start) throw new InvalidDataException("API link has no source span: " + originalPath);
            changes.Add((link.span.Start, link.span.Length, url));
        }
        foreach (var change in changes.OrderByDescending(change => change.start))
            text = text.Remove(change.start, change.length).Insert(change.start, change.value);
        return text;
    }
}
