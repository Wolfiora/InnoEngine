using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationApiPresentation
{
    private static readonly MarkdownPipeline S_MARKDOWN = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();

    internal static string? Parent(DocumentationApiFact fact)
        => fact.containingType is null ? null : fact.family + ":" + fact.project + ":" + fact.containingType;

    internal static string Name(
        DocumentationApiFact fact,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts
    ) {
        var names = new List<string>();
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (true)
        {
            if (!visited.Add(fact.key)) throw new InvalidDataException("Cyclic API containment: " + fact.key);
            names.Add(DocumentationApiContent.Title(fact));
            string? parent = Parent(fact);
            if (parent is null) break;
            if (!facts.TryGetValue(parent, out DocumentationApiFact[]? owner))
                throw new InvalidDataException("Missing API containing type: " + parent);
            fact = owner[0];
        }
        names.Reverse();
        return string.Join('.', names);
    }

    internal static string Cell(string text) => WebUtility.HtmlEncode(text)
        .Replace("|", "\\|", StringComparison.Ordinal).Replace("[", "\\[", StringComparison.Ordinal)
        .Replace("]", "\\]", StringComparison.Ordinal).Replace("\r", " ", StringComparison.Ordinal)
        .Replace("\n", " ", StringComparison.Ordinal);

    internal static string Summary(string markdown)
    {
        MarkdownDocument document = Markdown.Parse(markdown, S_MARKDOWN);
        ParagraphBlock? paragraph = document.OfType<ParagraphBlock>().FirstOrDefault();
        if (paragraph is null) return "No summary is available; open the declaration and full contract.";
        string text = Regex.Replace(Markdown.ToPlainText(markdown[paragraph.Span.Start..(paragraph.Span.End + 1)]), @"\s+", " ").Trim();
        return text.Length <= 240 ? text : text[..240].TrimEnd() + "…";
    }

    internal static IReadOnlyDictionary<string, string> ProjectDescriptions(string root)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        string content = Path.Combine(root, "content/en");
        if (!Directory.Exists(content)) return result;
        foreach (string path in Directory.EnumerateFiles(content, "*.md", SearchOption.AllDirectories))
        {
            string relative = Path.GetRelativePath(root, path).Replace('\\', '/');
            if (relative.StartsWith("content/en/api/", StringComparison.Ordinal)) continue;
            string text = File.ReadAllText(DocumentationApiFacts.Resolve(root, relative));
            if (Markdown.Parse(text, S_MARKDOWN).OfType<YamlFrontMatterBlock>().FirstOrDefault() is null) continue;
            YamlMappingNode metadata = DocumentationApiFragment.ReadMetadata(relative, text);
            if (!metadata.Children.TryGetValue(new YamlScalarNode("kind"), out YamlNode? kind)
                || kind is not YamlScalarNode { Value: "project" }) continue;
            if (!metadata.Children.TryGetValue(new YamlScalarNode("owners"), out YamlNode? owners)
                || owners is not YamlSequenceNode sequence) continue;
            string summary = metadata.Children.TryGetValue(new YamlScalarNode("description"), out YamlNode? description)
                && description is YamlScalarNode { Value: string value } ? value : Summary(text);
            foreach (YamlScalarNode owner in sequence.Children)
                if (!result.TryAdd(owner.Value!, summary)) throw new InvalidDataException("Duplicate project guide: " + owner.Value);
        }
        return result;
    }
}
