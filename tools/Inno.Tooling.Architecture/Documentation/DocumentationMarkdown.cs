using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.Tables;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Inno.Tooling.Architecture.Documentation;

internal static class DocumentationMarkdown
{
    private static readonly MarkdownPipeline S_PIPELINE = new MarkdownPipelineBuilder()
        .UsePipeTables().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();

    internal static MarkdownDocument ParseDocument(string text) => Markdown.Parse(text, S_PIPELINE);

    internal static DocumentationPage Parse(
        string path,
        string text,
        bool includeSymbols = true
    ) {
        MarkdownDocument document = ParseDocument(text);
        HeadingBlock[] headings = document.Descendants<HeadingBlock>().ToArray();
        var sections = new List<DocumentationSection>();
        if (headings.Length == 0 || headings[0].Span.Start > 0)
            sections.Add(new("preamble", "Preamble", 0, 0, headings.FirstOrDefault()?.Span.Start ?? text.Length, 1));
        for (int index = 0; index < headings.Length; index++)
        {
            HeadingBlock heading = headings[index];
            HeadingBlock? next = headings.Skip(index + 1).FirstOrDefault(candidate => candidate.Level <= heading.Level);
            sections.Add(new(heading.GetAttributes().Id ?? "section-" + index,
                heading.Inline?.ToString() ?? string.Empty, heading.Level, heading.Span.Start,
                next?.Span.Start ?? text.Length, heading.Line + 1));
        }
        string[] html = document.Descendants<HtmlBlock>().Select(block => text[block.Span.Start..(block.Span.End + 1)]).ToArray();
        var symbols = new List<DocumentationSymbol>();
        var rows = new Dictionary<string, TableRow>(StringComparer.Ordinal);
        foreach (TableRow row in includeSymbols ? document.Descendants<TableRow>() : [])
            foreach (Match anchor in Regex.Matches(text[row.Span.Start..(row.Span.End + 1)], "id=\"(?<id>api-[a-f0-9]+)\""))
                if (!rows.TryAdd(anchor.Groups["id"].Value, row))
                    throw new InvalidDataException($"Duplicate API anchor in {path}: {anchor.Groups["id"].Value}");
        foreach (string block in includeSymbols ? html : [])
        foreach (Match match in Regex.Matches(block, @"<!-- api: (?<id>\S+) (?<hash>[a-f0-9]{64}) -->"))
        {
            string id = match.Groups["id"].Value;
            string anchor = "api-" + Hash(id)[..16];
            rows.TryGetValue(anchor, out TableRow? row);
            int offset = row?.Span.Start ?? text.IndexOf(match.Value, StringComparison.Ordinal);
            DocumentationSection section = sections.Last(candidate => candidate.start <= offset && candidate.end > offset);
            symbols.Add(new(id, match.Groups["hash"].Value, anchor, section.anchor,
                row is null ? string.Empty : text[row.Span.Start..(row.Span.End + 1)]));
        }
        string[] includes = html.SelectMany(block => Regex.Matches(block, @"<!-- api-include: (?<path>[^\r\n]+?) -->")
            .Select(static match => match.Groups["path"].Value)).ToArray();
        string[] links = document.Descendants<LinkInline>().Where(static link => link.Url is not null)
            .Select(static link => link.Url!).Distinct(StringComparer.Ordinal).ToArray();
        return new(path, Hash(text), headings.FirstOrDefault()?.Inline?.ToString() ?? Path.GetFileNameWithoutExtension(path),
            text, html.Any(static block => block.Contains("<!-- api:", StringComparison.Ordinal)), sections, symbols, includes, links);
    }

    internal static HashSet<string> Anchors(string text)
    {
        MarkdownDocument document = ParseDocument(text);
        var result = document.Descendants<HeadingBlock>().Select(static heading => heading.GetAttributes().Id!)
            .ToHashSet(StringComparer.Ordinal);
        foreach (string html in document.Descendants<HtmlBlock>().Select(block => text[block.Span.Start..(block.Span.End + 1)])
            .Concat(document.Descendants<HtmlInline>().Select(static inline => inline.Tag)))
            foreach (Match match in Regex.Matches(html, "\\b(?:id|name)=[\"'](?<name>[^\"']+)[\"']"))
                result.Add(match.Groups["name"].Value);
        return result;
    }

    internal static string Hash(string value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    internal static string Guidance(string markdown)
    {
        MarkdownDocument document = ParseDocument(markdown);
        return string.Join("\n", document.Where(static block => block is not Table and not HtmlBlock)
            .Select(block => markdown[block.Span.Start..(block.Span.End + 1)]));
    }
}
