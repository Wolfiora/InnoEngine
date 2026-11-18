using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationProjectGuide(
    string path,
    string uid,
    string project
);
internal sealed record DocumentationProjectGuideRegistry(
    string schema,
    DocumentationProjectGuide[] entries
);

internal static class DocumentationProjectGuides
{
    internal const string C_REGISTRY = "reference/projects/content.json";

    internal static DocumentationProjectGuide[] Entries(string root)
    {
        string path = DocumentationApiFacts.Resolve(root, C_REGISTRY);
        if (!File.Exists(path)) return [];
        var registry = JsonSerializer.Deserialize<DocumentationProjectGuideRegistry>(File.ReadAllText(path))
            ?? throw new InvalidDataException("Missing project guide registry.");
        if (registry.schema != "inno-project-guides/1"
            || registry.entries.Select(entry => entry.path).Distinct(StringComparer.OrdinalIgnoreCase).Count() != registry.entries.Length
            || registry.entries.Select(entry => entry.project).Distinct(StringComparer.Ordinal).Count() != registry.entries.Length
            || registry.entries.Select(entry => entry.uid).Distinct(StringComparer.Ordinal).Count() != registry.entries.Length)
            throw new InvalidDataException("Invalid project guide registry.");
        foreach (var entry in registry.entries)
        {
            if (!entry.path.StartsWith("content/en/architecture/projects/", StringComparison.Ordinal)
                || !entry.path.EndsWith(".md", StringComparison.Ordinal)
                || entry.path.Split('/').Any(segment => segment is ".." or "." or ""))
                throw new InvalidDataException("Project guide is outside its registered content directory.");
            _ = DocumentationApiFacts.Resolve(root, entry.path);
        }
        return registry.entries;
    }

    internal static DocumentationApiPlan Plan(
        string root,
        DocumentationBinding binding,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts,
        string? reviewPage = null
    ) {
        var edits = new List<DocumentationApiEdit>();
        var warnings = new List<DocumentationApiWarning>();
        var projects = JsonSerializer.Deserialize<DocumentationProject[]>(File.ReadAllText(DocumentationApiFacts.Resolve(root, binding.projectManifest)))!
            .ToDictionary(project => project.name, StringComparer.Ordinal);
        var entries = Entries(root);
        if (reviewPage is not null && !entries.Any(entry => entry.path == reviewPage))
            throw new InvalidDataException("Review requires a registered project guide.");
        foreach (var entry in entries.Where(entry => reviewPage is null || entry.path == reviewPage))
        {
            string text = File.ReadAllText(DocumentationApiFacts.Resolve(root, entry.path));
            var fragment = Read(entry, text);
            projects.TryGetValue(entry.project, out var project);
            var contexts = facts.Values.SelectMany(context => context).Where(fact => fact.project == entry.project).ToArray();
            string contract = Identity(project, contexts);
            bool manual = fragment.source != "xml";
            if (reviewPage is not null && (project is null || !manual || string.IsNullOrWhiteSpace(fragment.body)
                || fragment.body.Trim() == Body(entry, contexts).Trim()))
                throw new InvalidDataException("Review requires an existing project and an independent named-author explanation.");
            bool stale = manual && reviewPage is null && (project is null || IsOutdated(fragment, project, contexts, contract));
            bool pending = manual && reviewPage is null && string.IsNullOrEmpty(fragment.reviewed);
            if (project is null)
                warnings.Add(new(entry.project, entry.path, "RemovedProject", "The source project is absent; the registered guide is retained for editorial resolution."));
            else if (stale)
                warnings.Add(new(entry.project, entry.path, "ReviewRequired", "Project facts or XML changed; review this explanation."));
            else if (pending)
                warnings.Add(new(entry.project, entry.path, "InitialReviewPending", "This project guide has no recorded source review."));
            if (!manual || string.IsNullOrWhiteSpace(fragment.body) || fragment.body.Trim() == Body(entry, contexts).Trim())
                warnings.Add(new(entry.project, entry.path, "ManualExplanationRequired", "This project guide uses generated source information."));
            string reviewed = !manual ? "" : stale || pending ? fragment.reviewed ?? "" : contract;
            string updated = Metadata(entry, fragment, reviewed, stale)
                + (manual || project is null ? fragment.body : "\n\n" + Body(entry, contexts));
            if (updated != text) edits.Add(new(entry.path, DocumentationMarkdown.Hash(text), updated));
        }
        return new(edits, warnings, 0);
    }

    internal static void Compose(
        string root,
        DocumentationBinding binding,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts,
        IReadOnlyDictionary<string, DocumentationApiFragment> fragments,
        IReadOnlyDictionary<string, DocumentationApiContentEntry> apiEntries,
        IDictionary<string, string> pages,
        IDictionary<string, string> inputs
    ) {
        var entries = Entries(root);
        if (entries.Length == 0) return;
        inputs[C_REGISTRY] = DocumentationMarkdown.Hash(File.ReadAllText(DocumentationApiFacts.Resolve(root, C_REGISTRY)));
        var projects = JsonSerializer.Deserialize<DocumentationProject[]>(File.ReadAllText(DocumentationApiFacts.Resolve(root, binding.projectManifest)))!
            .ToDictionary(project => project.name, StringComparer.Ordinal);
        foreach (var entry in entries)
        {
            var fragment = Read(entry, pages[entry.path]);
            projects.TryGetValue(entry.project, out var project);
            var contexts = facts.Values.SelectMany(context => context).Where(fact => fact.project == entry.project).ToArray();
            bool stale = project is null || IsOutdated(fragment, project, contexts, Identity(project, contexts));
            string status = stale ? "OUTDATED" : fragment.source == "xml" ? "SYNCED"
                : string.IsNullOrEmpty(fragment.reviewed) ? "REVIEW-PENDING" : "VERIFIED";
            string[] summaries = fragment.Summaries("");
            fragment.metadata.Children.Remove(new YamlScalarNode("description"));
            string header = "---\n" + Serialize(fragment.metadata) + "\n---";
            var text = new StringBuilder(header).Append("\n\n> [!").Append(status)
                .Append("]\n> Project guide ownership and source review status.\n\n# ").Append(entry.project).Append("\n\n");
            if (summaries.Length != 0)
                text.Append("> ").Append(string.Join("\n> ", summaries.Select(summary => "- " + summary.Replace("\n", "\n>   ", StringComparison.Ordinal)))).Append("\n\n");
            text.Append(Regex.Replace(fragment.body, @"\A\s*# [^\r\n]+\r?\n", ""));
            if (project is not null)
            {
                text.Append("\n\n## Source project facts\n\n**Project:** `").Append(project.name)
                    .Append("`  \n**Source:** `").Append(project.path).Append("`\n\n");
                var types = contexts.Where(fact => fact.kind == "NamedType").GroupBy(fact => fact.key, StringComparer.Ordinal)
                    .OrderBy(group => group.Key, StringComparer.Ordinal).ToArray();
                if (types.Length != 0)
                {
                    text.Append("| Type | API | Description |\n| --- | --- | --- |\n");
                    foreach (var type in types)
                    {
                        var fact = type.First();
                        var api = apiEntries[fact.key];
                        text.Append("| [").Append(DocumentationApiPresentation.Cell(api.title)).Append("](")
                            .Append(Path.GetRelativePath(Path.GetDirectoryName(entry.path)!, api.path).Replace('\\', '/'))
                            .Append(") | ").Append(fact.scope ?? fact.family).Append(" | ")
                            .Append(fragments[fact.key].TableDescription(fact.documentation)).Append(" |\n");
                    }
                }
            }
            if (project is not null)
                text.Append("\n<!-- api-source -->\n[Source declaration](").Append(binding.repository).Append("/blob/")
                    .Append(binding.revision).Append('/').Append(project.path).Append(")\n");
            pages[entry.path] = text.ToString();
        }
    }

    private static DocumentationApiFragment Read(
        DocumentationProjectGuide entry,
        string text
    ) {
        var fragment = DocumentationApiFragment.Read(entry.path, text)
            ?? throw new InvalidDataException("Registered project guide is missing ownership metadata: " + entry.path);
        if (fragment.symbol != "project:" + entry.project || fragment.uid != entry.uid)
            throw new InvalidDataException("Project guide identity differs from registry: " + entry.path);
        return fragment;
    }

    private static string Identity(
        DocumentationProject? project,
        DocumentationApiFact[] contexts
    ) {
        var identity = new
        {
            metadata = project is null ? null : project with { surfaces = project.surfaces.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray() },
            symbols = contexts.GroupBy(fact => fact.key, StringComparer.Ordinal).OrderBy(group => group.Key, StringComparer.Ordinal)
                .Select(group => new { symbol = group.Key, contract = DocumentationApiContent.Contract(group, normalizeXml: true) })
        };
        return DocumentationMarkdown.Hash(JsonSerializer.Serialize(identity));
    }

    private static bool IsOutdated(
        DocumentationApiFragment fragment,
        DocumentationProject project,
        DocumentationApiFact[] contexts,
        string contract
    ) {
        if (fragment.source == "xml") return false;
        if (fragment.outdated) return true;
        if (string.IsNullOrEmpty(fragment.reviewed) || fragment.reviewed == contract) return false;
        string legacy = DocumentationMarkdown.Hash(JsonSerializer.Serialize(project) + "\n" + DocumentationApiContent.Contract(contexts));
        return fragment.reviewed != legacy;
    }

    private static string Metadata(
        DocumentationProjectGuide entry,
        DocumentationApiFragment fragment,
        string reviewed,
        bool outdated
    ) {
        var generated = new YamlMappingNode
        {
            { "uid", entry.uid }, { "symbol", "project:" + entry.project }, { "title", entry.project },
            { "kind", "project" }, { "owners", new YamlSequenceNode(new YamlScalarNode(entry.project)) },
            { "reviewed", reviewed },
            { "contracts", fragment.metadata.Children.TryGetValue(new YamlScalarNode("contracts"), out var contracts) ? contracts : new YamlSequenceNode() }
        };
        var authored = new YamlMappingNode
        {
            { "source", fragment.source },
            { "description", fragment.metadata.Children.TryGetValue(new YamlScalarNode("description"), out var description) ? description : new YamlScalarNode("null") { Style = YamlDotNet.Core.ScalarStyle.Plain } },
            { "outdated", outdated ? "true" : "false" }
        };
        foreach (var field in fragment.metadata.Children)
            if (!generated.Children.ContainsKey(field.Key) && !authored.Children.ContainsKey(field.Key)) authored.Add(field.Key, field.Value);
        return "---\n# Generated metadata. Maintained by project synchronization and review tooling.\n" + Serialize(generated)
            + "\n\n# Authoring metadata. Edit these fields and the explanation below.\n" + Serialize(authored) + "\n---";
    }

    private static string Body(
        DocumentationProjectGuide entry,
        DocumentationApiFact[] contexts
    ) {
        var text = new StringBuilder("# ").Append(entry.project).Append("\n\n## Source contracts\n\n");
        var types = contexts.Where(fact => fact.kind == "NamedType").GroupBy(fact => fact.key, StringComparer.Ordinal)
            .OrderBy(group => group.Key, StringComparer.Ordinal);
        foreach (var type in types)
        {
            var fact = type.First();
            text.Append("### ").Append(WebUtility.HtmlEncode(fact.id[2..])).Append(" (")
                .Append(fact.scope ?? fact.family).Append(")\n\n");
            foreach (string summary in DocumentationXml.Read(fact.documentation).summaries) text.Append(summary).Append("\n\n");
        }
        if (!types.Any()) text.Append("No public type XML is present for this project in the selected API snapshot.\n");
        return text.ToString();
    }

    private static string Serialize(YamlMappingNode metadata)
    {
        using var writer = new StringWriter();
        new YamlStream(new YamlDocument(metadata)).Save(writer, assignAnchors: false);
        string text = writer.ToString().ReplaceLineEndings("\n").TrimEnd();
        return text.EndsWith("\n...", StringComparison.Ordinal) ? text[..^4] : text;
    }
}
