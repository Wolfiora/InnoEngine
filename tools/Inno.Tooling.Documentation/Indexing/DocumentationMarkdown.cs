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
using Markdig.Extensions.Yaml;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationMarkdown
{
    // Changes to parsing, tokenization or cached contracts must change this schema identity.
    internal const string C_SCHEMA = "inno-doc/2;markdig/1.5.1;yaml/16.3.0;tokens/1";
    private static readonly MarkdownPipeline S_PIPELINE = new MarkdownPipelineBuilder()
        .UseYamlFrontMatter().UsePipeTables().UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
        .UseGenericAttributes().Build();

    internal static DocumentationPage Parse(
        string path,
        string text
    ) {
        MarkdownDocument document = Markdown.Parse(text, S_PIPELINE);
        YamlFrontMatterBlock? front = document.OfType<YamlFrontMatterBlock>().SingleOrDefault();
        if (front is null)
            throw new InvalidDataException("Documentation requires identity metadata: " + path);
        var yaml = new YamlStream();
        yaml.Load(new StringReader(front.Lines.ToString()));
        if (yaml.Documents.Count != 1 || yaml.Documents[0].RootNode is not YamlMappingNode metadata)
            throw new InvalidDataException("Documentation metadata must be one mapping: " + path);
        string uid = Scalar("uid") ?? throw new InvalidDataException("Missing page uid: " + path);
        if (!Regex.IsMatch(uid, "^[a-z0-9][a-z0-9._/-]*$") || uid.Split('/').Any(static segment => segment is "" or "." or ".."))
            throw new InvalidDataException("Invalid stable page identity: " + uid);
        string? exampleMode = Scalar("exampleMode");
        string[] exampleProjects = Sequence("exampleProjects");
        if (exampleMode is not (null or "host" or "runtime-script" or "editor-script")
            || (exampleMode is null) != (exampleProjects.Length == 0)
            || exampleProjects.Distinct(StringComparer.Ordinal).Count() != exampleProjects.Length)
            throw new InvalidDataException("Examples require an explicit reference mode and unique project identities: " + path);
        var facts = new DocumentationMetadata(uid, Scalar("kind") ?? "", Sequence("owners"),
            Sequence("aliases"), Sequence("contracts"), Scalar("apiSurface"),
            exampleMode is null ? null : new DocumentationExampleContext(exampleMode, exampleProjects));
        if (facts.kind is not ("landing" or "tutorial" or "how-to" or "explanation" or "project" or "api" or "troubleshooting"))
            throw new InvalidDataException("Unknown documentation page kind: " + path);
        if (facts.kind == "project" && facts.owners.Length != 1)
            throw new InvalidDataException("A project entry requires exactly one actual owner: " + path);
        HeadingBlock[] headings = document.Descendants<HeadingBlock>().ToArray();
        if (headings.Count(static heading => heading.Level == 1) != 1)
            throw new InvalidDataException("Documentation requires exactly one H1: " + path);
        var sections = new List<DocumentationSection>();
        for (int index = 0; index < headings.Length; index++)
        {
            HeadingBlock heading = headings[index];
            HeadingBlock? next = headings.Skip(index + 1).FirstOrDefault(candidate => candidate.Level <= heading.Level);
            sections.Add(new(heading.GetAttributes().Id ?? throw new InvalidDataException("Missing section identity: " + path),
                HeadingText(heading), heading.Level, heading.Span.Start,
                next?.Span.Start ?? text.Length, heading.Line + 1));
        }
        if (sections.Select(static section => section.anchor).Distinct(StringComparer.Ordinal).Count() != sections.Count)
            throw new InvalidDataException("Duplicate section identity: " + path);
        string[] html = document.Descendants<HtmlBlock>().Select(block => text[block.Span.Start..(block.Span.End + 1)]).ToArray();
        foreach (string fragment in html.Concat(document.Descendants<HtmlInline>().Select(static inline => inline.Tag)))
        {
            // Ordinary documentation has no executable HTML surface. Layout belongs to the website theme.
            string remaining = Regex.Replace(fragment, @"<!--(?:(?!-->)[\s\S])*-->", string.Empty);
            remaining = Regex.Replace(remaining, "<a id=\"api-[a-f0-9]+\">|</a>", string.Empty);
            if (!string.IsNullOrWhiteSpace(remaining))
                throw new InvalidDataException("Documentation permits only comments and explicit API anchors as HTML: " + path);
        }
        var rows = new Dictionary<string, TableRow>(StringComparer.Ordinal);
        foreach (TableRow row in document.Descendants<TableRow>())
        foreach (Match anchor in Regex.Matches(text[row.Span.Start..(row.Span.End + 1)], "id=\"(?<id>api-[a-f0-9]+)\""))
            if (!rows.TryAdd(anchor.Groups["id"].Value, row))
                throw new InvalidDataException("Duplicate API anchor: " + path);
        FencedCodeBlock[] fences = document.Descendants<FencedCodeBlock>().ToArray();
        var symbols = new List<DocumentationSymbol>();
        foreach (string block in html)
        foreach (Match marker in Regex.Matches(block, @"<!-- api: (?<id>\S+) (?<hash>[a-f0-9]{64}) -->"))
        {
            string id = marker.Groups["id"].Value;
            string anchor = "api-" + Hash(id)[..16];
            rows.TryGetValue(anchor, out TableRow? row);
            int start = row?.Span.Start ?? text.IndexOf(marker.Value, StringComparison.Ordinal);
            DocumentationSection section = sections.Last(candidate => candidate.start <= start && candidate.end > start);
            string declaration = row is null ? text[section.start..section.end] : text[row.Span.Start..(row.Span.End + 1)];
            string[] code = row is not null
                ? row.Descendants<CodeInline>().Select(static item => item.Content).ToArray()
                : fences
                    .Where(item => item.Span.Start >= section.start && item.Span.End < section.end && item.Info == "csharp")
                    .Select(static item => item.Lines.ToString().Replace("\r\n", "\n").TrimEnd('\n')).ToArray();
            symbols.Add(new(id, marker.Groups["hash"].Value, anchor, section.anchor, declaration, code));
        }
        string[] includes = html.SelectMany(block => Regex.Matches(block, @"<!-- api-include: (?<path>[^\r\n]+?) -->")
            .Select(static marker => marker.Groups["path"].Value)).ToArray();
        DocumentationCodeBlockInfo[] codeBlocks = fences.Select(block =>
        {
            string code = block.Lines.ToString().Replace("\r\n", "\n").TrimEnd('\n');
            string identity = Hash(code);
            string? section = sections.LastOrDefault(candidate => candidate.start <= block.Span.Start)?.anchor;
            return new DocumentationCodeBlockInfo(block.Info ?? "", code, identity, block.Line + 1, section,
                symbols.Where(symbol => symbol.signatureIdentity == identity).Select(static symbol => symbol.id).ToArray());
        }).ToArray();
        return new(path, Hash(text), HeadingText(headings.Single(static heading => heading.Level == 1)),
            text, facts, sections, symbols, includes, document.Descendants<LinkInline>()
                .Where(static link => link.Url is not null).Select(static link => link.Url!).Distinct(StringComparer.Ordinal).ToArray(), codeBlocks);

        string HeadingText(HeadingBlock heading)
            => Markdown.ToPlainText(text[heading.Span.Start..(heading.Span.End + 1)], S_PIPELINE).Trim();

        string? Scalar(string key)
        {
            if (!metadata.Children.TryGetValue(new YamlScalarNode(key), out YamlNode? node))
                return null;
            return node is YamlScalarNode scalar ? scalar.Value : throw new InvalidDataException("Expected scalar " + key + ": " + path);
        }

        string[] Sequence(string key)
        {
            if (!metadata.Children.TryGetValue(new YamlScalarNode(key), out YamlNode? node))
                return [];
            if (node is not YamlSequenceNode sequence || sequence.Children.Any(static child => child is not YamlScalarNode))
                throw new InvalidDataException("Expected string sequence " + key + ": " + path);
            return sequence.Children.Cast<YamlScalarNode>().Select(static scalar => scalar.Value ?? "").ToArray();
        }
    }

    internal static string Hash(string value) => Hash(Encoding.UTF8.GetBytes(value));
    internal static string Hash(byte[] value) => Convert.ToHexStringLower(SHA256.HashData(value));

    internal static string[] Tokens(string text)
    {
        string expanded = Regex.Replace(text, "([a-z0-9])([A-Z])", "$1 $2");
        return Regex.Matches(expanded.ToLowerInvariant(), @"[\p{L}\p{N}]+")
            .Select(static match => match.Value).Distinct(StringComparer.Ordinal).ToArray();
    }

    internal static string OwnText(
        DocumentationPage page,
        DocumentationSection section
    ) {
        int next = page.sections.FirstOrDefault(candidate => candidate.start > section.start)?.start ?? page.text.Length;
        return page.text[section.start..Math.Min(section.end, next)];
    }
}
