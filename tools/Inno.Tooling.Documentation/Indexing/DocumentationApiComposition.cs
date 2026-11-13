using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiOrigin(string page, string id, string path, string source, string? scope, string revision, string[] surfaces);
internal sealed record DocumentationComposedContent(
    IReadOnlyDictionary<string, string> pages,
    IReadOnlyDictionary<string, string> inputs,
    IReadOnlyList<DocumentationApiOrigin> origins
);

internal static class DocumentationApiComposition
{
    internal static DocumentationComposedContent Prepare(string root, DocumentationBinding binding, IReadOnlyDictionary<string, string> raw)
    {
        var pages = new SortedDictionary<string, string>(raw.ToDictionary(pair => pair.Key, pair => pair.Value), StringComparer.Ordinal);
        var inputs = new SortedDictionary<string, string>(raw.ToDictionary(pair => pair.Key, pair => DocumentationMarkdown.Hash(pair.Value)), StringComparer.Ordinal);
        var origins = new List<DocumentationApiOrigin>();
        string registryPath = DocumentationApiFacts.Resolve(root, DocumentationApiContent.C_REGISTRY);
        if (!File.Exists(registryPath)) return new(pages, inputs, origins);
        string registryText = File.ReadAllText(registryPath);
        inputs.Add(DocumentationApiContent.C_REGISTRY, DocumentationMarkdown.Hash(registryText));
        DocumentationApiContentRegistry registry = JsonSerializer.Deserialize<DocumentationApiContentRegistry>(registryText)
            ?? throw new InvalidDataException("Missing API content registry.");
        if (registry.schema != "inno-api-content/1") throw new InvalidDataException("Unsupported API content registry.");
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts = DocumentationApiFacts.Read(root, binding);
        var entries = registry.entries.ToDictionary(entry => entry.symbol, StringComparer.Ordinal);
        var fragments = new Dictionary<string, DocumentationApiFragment>(StringComparer.Ordinal);
        foreach (DocumentationApiContentEntry entry in registry.entries)
        {
            DocumentationApiContent.RequireApiPath(root, entry.path);
            if (!raw.TryGetValue(entry.path, out string? text)) throw new InvalidDataException("Registered API fragment is missing: " + entry.path);
            DocumentationApiFragment fragment = DocumentationApiFragment.Read(entry.path, text)
                ?? throw new InvalidDataException("Registered API fragment has no symbol: " + entry.path);
            if (fragment.symbol != entry.symbol) throw new InvalidDataException("API fragment identity does not match its registry: " + entry.path);
            fragments.Add(entry.symbol, fragment);
            pages.Remove(entry.path);
        }
        foreach (string key in facts.Keys)
            if (!entries.ContainsKey(key)) throw new InvalidDataException("API capture has no registered content: " + key);
        var byType = entries.Values.Where(entry => facts.ContainsKey(entry.symbol)).GroupBy(entry => entry.type, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.OrderBy(member => member.title, StringComparer.OrdinalIgnoreCase)
                .ThenBy(member => member.symbol, StringComparer.Ordinal).ToArray(), StringComparer.Ordinal);
        foreach (var type in facts.Values.Where(contexts => contexts[0].kind == "NamedType"))
        {
            DocumentationApiFact fact = type[0];
            DocumentationApiContentEntry entry = entries[fact.key];
            DocumentationApiFragment fragment = fragments[fact.key];
            string header = Header(fragment, type, byType[fact.key].Select(member => fragments[member.symbol]));
            var text = new StringBuilder(header).Append("\n\n# ").Append(entry.title.Replace("`", "", StringComparison.Ordinal)).Append("\n\n");
            text.Append("**Project:** `").Append(fact.project).Append("` · **API:** ").Append(fact.family)
                .Append(fact.scope is null ? "" : " · **Scope:** " + fact.scope).Append("\n\n");
            if (fact.baseType is not null) text.Append("**Inherits:** `").Append(fact.baseType).Append("`\n\n");
            if (fact.interfaces.Count != 0) text.Append("**Implements:** ").Append(string.Join(", ", fact.interfaces.Select(value => "`" + value + "`"))).Append("\n\n");
            AppendSymbol(text, type, fragment, entry.path, 2, "Declaration");
            DocumentationApiContentEntry[] members = byType[fact.key].Where(member => member.symbol != fact.key).ToArray();
            if (members.Length != 0)
            {
                text.Append("\n## Member summary\n\n| Member | Kind |\n| --- | --- |\n");
                foreach (DocumentationApiContentEntry member in members)
                    text.Append("| [").Append(member.title.Replace("|", "\\|", StringComparison.Ordinal)).Append("](#api-")
                        .Append(DocumentationMarkdown.Hash(facts[member.symbol][0].id)[..16]).Append(") | ")
                        .Append(facts[member.symbol][0].kind).Append(" |\n");
                foreach (var group in members.GroupBy(member => Category(facts[member.symbol][0])).OrderBy(group => group.Key, StringComparer.Ordinal))
                {
                    text.Append("\n## ").Append(group.Key).Append("\n\n");
                    foreach (DocumentationApiContentEntry member in group)
                        AppendSymbol(text, facts[member.symbol], fragments[member.symbol], entry.path, 3, member.title.Replace("`", "", StringComparison.Ordinal));
                }
            }
            pages.Add(entry.path, text.ToString());
        }
        return new(pages, inputs, origins);

        void AppendSymbol(StringBuilder text, DocumentationApiFact[] contexts, DocumentationApiFragment fragment, string ownerPath, int level, string title)
        {
            DocumentationApiFact fact = contexts[0];
            text.Append('\n').Append('#', level).Append(' ').Append(title).Append("\n\n<a id=\"api-")
                .Append(DocumentationMarkdown.Hash(fact.id)[..16]).Append("\"></a>\n<!-- api: ")
                .Append(fact.id).Append(' ').Append(fact.signatureHash).Append(" -->\n\n");
            foreach (var variants in contexts.GroupBy(value => value.signature, StringComparer.Ordinal))
            {
                text.Append("**Available in:** ").Append(string.Join(", ", variants.Select(value => value.surface + "/" + value.configuration
                    + (value.target is null ? "" : "/" + value.target)).Distinct(StringComparer.Ordinal))).Append("\n\n```csharp\n")
                    .Append(variants.Key).Append("\n```\n\n");
            }
            if (fragment.source == "xml") text.Append("**XML-synced**\n\n");
            text.Append(Rebase(root, fragment.body.Trim(), fragment.path, ownerPath, level)).Append("\n\n")
                .Append("[Source declaration](").Append(binding.repository).Append("/blob/").Append(fact.revision)
                .Append('/').Append(fact.source).Append("#L").Append(fact.line).Append(").\n\n");
            origins.Add(new(ownerPath, fact.id, fragment.path, fragment.source, fact.scope, fact.revision,
                contexts.Select(context => context.surface).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray()));
        }
    }

    private static string Category(DocumentationApiFact fact) => fact.id.Contains(".#ctor", StringComparison.Ordinal) ? "Constructors"
        : fact.kind switch { "Method" => "Methods", "Property" => "Properties", "Event" => "Events", "Field" => "Fields", _ => "Members" };

    private static string Header(DocumentationApiFragment fragment, DocumentationApiFact[] contexts, IEnumerable<DocumentationApiFragment> members)
    {
        // Only the in-memory projection changes. The authored frontmatter and prose remain byte-preserved.
        YamlMappingNode metadata = fragment.metadata;
        metadata.Children[new YamlScalarNode("owners")] = new YamlSequenceNode(new YamlScalarNode(contexts[0].project));
        metadata.Children[new YamlScalarNode("apiSurface")] = new YamlScalarNode(contexts[0].surface);
        if (contexts[0].scope is string scope) metadata.Children[new YamlScalarNode("apiScope")] = new YamlScalarNode(scope);
        else metadata.Children.Remove(new YamlScalarNode("apiScope"));
        metadata.Children[new YamlScalarNode("apiSurfaces")] = new YamlSequenceNode(contexts.Select(context => context.surface)
            .Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).Select(value => new YamlScalarNode(value)));
        var contracts = new SortedSet<string>(StringComparer.Ordinal);
        foreach (DocumentationApiFragment member in members)
            if (member.metadata.Children.TryGetValue(new YamlScalarNode("contracts"), out YamlNode? value))
            {
                if (value is not YamlSequenceNode sequence || sequence.Children.Any(child => child is not YamlScalarNode))
                    throw new InvalidDataException("API contracts must be a sequence of stable references: " + member.path);
                foreach (YamlScalarNode contract in sequence.Children) contracts.Add(contract.Value ?? "");
            }
        metadata.Children[new YamlScalarNode("contracts")] = new YamlSequenceNode(contracts.Select(value => new YamlScalarNode(value)));
        using var writer = new StringWriter();
        new YamlStream(new YamlDocument(metadata)).Save(writer, assignAnchors: false);
        string yaml = writer.ToString().Replace("\r\n", "\n", StringComparison.Ordinal).TrimEnd();
        if (yaml.EndsWith("\n...", StringComparison.Ordinal)) yaml = yaml[..^4];
        return "---\n" + yaml + "\n---\n";
    }

    private static string Rebase(string root, string body, string fragmentPath, string pagePath, int level)
    {
        MarkdownDocument document = Markdown.Parse(body);
        var changes = new List<(int start, int length, string value)>();
        foreach (HeadingBlock heading in document.Descendants<HeadingBlock>())
        {
            string prefix = new('#', Math.Min(6, heading.Level + level));
            if (heading.IsSetext)
            {
                string source = body.Substring(heading.Span.Start, heading.Span.Length).TrimEnd();
                int underline = source.LastIndexOf('\n');
                if (underline < 0) throw new InvalidDataException("Invalid setext source span: " + fragmentPath);
                changes.Add((heading.Span.Start, 0, prefix + " "));
                changes.Add((heading.Span.Start + underline, heading.Span.Length - underline, ""));
            }
            else changes.Add((heading.Span.Start, heading.Level, prefix));
        }
        var links = document.Descendants<LinkInline>().Where(link => link.Url is not null && link.Reference is null)
            .Select(link => (url: link.Url!, span: link.UrlSpan))
            .Concat(document.Descendants<LinkReferenceDefinition>().Where(link => link.Url is not null).Select(link => (url: link.Url!, span: link.UrlSpan)));
        foreach (var link in links)
        {
            if (link.url.StartsWith('#') || Uri.TryCreate(link.url, UriKind.Absolute, out _)) continue;
            if (link.span.Start < 0 || link.span.End < link.span.Start) throw new InvalidDataException("API link lacks a source span.");
            string[] parts = link.url.Split('#', 2);
            string target = Path.GetFullPath(parts[0], Path.GetDirectoryName(Path.Combine(root, fragmentPath))!);
            string relative = Path.GetRelativePath(Path.GetDirectoryName(Path.Combine(root, pagePath))!, target).Replace('\\', '/');
            changes.Add((link.span.Start, link.span.Length, relative + (parts.Length == 2 ? "#" + parts[1] : "")));
        }
        foreach (var change in changes.OrderByDescending(change => change.start))
            body = body.Remove(change.start, change.length).Insert(change.start, change.value);
        return body;
    }
}
