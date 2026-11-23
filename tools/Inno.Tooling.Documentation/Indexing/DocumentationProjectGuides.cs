using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationProjectGuides
{
    internal const string C_REGISTRY = "reference/projects/content.json";

    private const string C_GUIDES = "content/en/architecture/projects/";
    private const string C_BACKEND_GUIDES = "content/en/backends/projects/";
    private const string C_TEST_GUIDES = "content/en/contributing/testing/projects/";

    internal static DocumentationProjectGuide[] Entries(string root)
    {
        string path = DocumentationApiFacts.Resolve(root, C_REGISTRY);
        if (!File.Exists(path)) return [];
        DocumentationProjectGuideRegistry registry = JsonSerializer.Deserialize<DocumentationProjectGuideRegistry>(File.ReadAllText(path))
                                                     ?? throw new InvalidDataException("Missing project guide registry.");
        if (registry.schema != "inno-project-guides/1"
            || registry.entries.Select(entry => entry.path).Distinct(StringComparer.OrdinalIgnoreCase).Count() != registry.entries.Length
            || registry.entries.Select(entry => entry.project).Distinct(StringComparer.Ordinal).Count() != registry.entries.Length
            || registry.entries.Select(entry => entry.uid).Distinct(StringComparer.Ordinal).Count() != registry.entries.Length
        ) {
            throw new InvalidDataException("Invalid project guide registry.");
        }
        foreach (DocumentationProjectGuide entry in registry.entries)
        {
            if (!IsGuidePath(entry.path)
                || !entry.path.EndsWith(".md", StringComparison.Ordinal)
                || entry.path.Split('/').Any(segment => segment is ".." or "." or "")
            ) {
                throw new InvalidDataException("Project guide is outside its registered content directory.");
            }
            _ = DocumentationApiFacts.Resolve(root, entry.path);
        }
        return registry.entries;
    }

    internal static DocumentationApiPlan Plan(
        string root,
        DocumentationBinding binding,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts
    ) {
        List<DocumentationApiEdit> edits = [];
        List<DocumentationApiWarning> warnings = [];
        Dictionary<string, DocumentationProject> projects = JsonSerializer
            .Deserialize<DocumentationProject[]>(
                File.ReadAllText(DocumentationApiFacts.Resolve(root, binding.projectManifest))
            )!
            .ToDictionary(project => project.name, StringComparer.Ordinal);
        DocumentationProjectGuide[] registered = Entries(root);
        string registryPath = DocumentationApiFacts.Resolve(root, C_REGISTRY);
        // A checkout without the registry does not manage project guides.
        DocumentationProjectGuide[] created = File.Exists(registryPath) ? NewGuides(root, projects.Values, registered) : [];
        List<DocumentationProjectGuide> entries = [];
        foreach (DocumentationProjectGuide entry in registered.Concat(created))
        {
            bool isNew = !registered.Contains(entry);
            string absolute = DocumentationApiFacts.Resolve(root, entry.path);
            if (isNew && File.Exists(absolute))
            {
                throw new InvalidDataException("Synchronization cannot overwrite an unregistered project guide: " + entry.path);
            }
            string? text = isNew ? null : File.ReadAllText(absolute);
            DocumentationApiFragment fragment = Read(
                entry,
                text ?? "---\nuid: " + entry.uid + "\nsymbol: project:" + entry.project + "\nsource: xml\n---\n"
            );
            projects.TryGetValue(entry.project, out DocumentationProject? project);
            DocumentationApiFact[] contexts = facts.Values
                .SelectMany(context => context)
                .Where(fact => fact.project == entry.project)
                .ToArray();
            string contract = Identity(project, contexts);
            bool manual = fragment.source != "xml";
            if (project is null && !manual)
            {
                // The same removal rule as API pages: synchronized prose leaves with its source,
                // authored prose stays for editorial resolution.
                edits.Add(new DocumentationApiEdit(entry.path, DocumentationMarkdown.Hash(text!), null));
                continue;
            }
            entries.Add(entry);
            string? staleProse = StaleProse(fragment, project, contexts, contract);
            bool stale = fragment.IsOutdated(staleProse);
            if (project is null)
            {
                warnings.Add(
                    new DocumentationApiWarning(
                        entry.project,
                        entry.path,
                        "RemovedProject",
                        "The source project is absent; the registered guide is retained for editorial resolution."
                    )
                );
            }
            else if (stale)
            {
                warnings.Add(
                    new DocumentationApiWarning(entry.project, entry.path, "ReviewRequired", "The authored project guide is outdated.")
                );
            }
            if (!manual || string.IsNullOrWhiteSpace(fragment.body) || fragment.body.Trim() == Body(entry, contexts).Trim())
            {
                warnings.Add(
                    new DocumentationApiWarning(
                        entry.project,
                        entry.path,
                        "ManualExplanationRequired",
                        "This project guide uses generated source information."
                    )
                );
            }
            string updated = Metadata(entry, fragment, contract, staleProse)
                             + (manual || project is null ? fragment.body : "\n\n" + Body(entry, contexts));
            if (updated != text)
            {
                edits.Add(new DocumentationApiEdit(entry.path, text is null ? null : DocumentationMarkdown.Hash(text), updated));
            }
        }
        if (created.Length != 0 || entries.Count != registered.Length)
        {
            var registry = new DocumentationProjectGuideRegistry(
                "inno-project-guides/1",
                entries.OrderBy(entry => entry.path, StringComparer.Ordinal).ToArray()
            );
            var options = new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" };
            edits.Add(
                new DocumentationApiEdit(
                    C_REGISTRY,
                    DocumentationMarkdown.Hash(File.ReadAllText(registryPath)),
                    JsonSerializer.Serialize(registry, options) + "\n"
                )
            );
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
        DocumentationProjectGuide[] entries = Entries(root);
        if (entries.Length == 0) return;
        inputs[C_REGISTRY] = DocumentationMarkdown.Hash(File.ReadAllText(DocumentationApiFacts.Resolve(root, C_REGISTRY)));
        Dictionary<string, DocumentationProject> projects = JsonSerializer
            .Deserialize<DocumentationProject[]>(
                File.ReadAllText(DocumentationApiFacts.Resolve(root, binding.projectManifest))
            )!
            .ToDictionary(project => project.name, StringComparer.Ordinal);
        foreach (DocumentationProjectGuide entry in entries)
        {
            DocumentationApiFragment fragment = Read(entry, pages[entry.path]);
            projects.TryGetValue(entry.project, out DocumentationProject? project);
            DocumentationApiFact[] contexts = facts.Values
                .SelectMany(context => context)
                .Where(fact => fact.project == entry.project)
                .ToArray();
            bool stale = project is null || fragment.IsOutdated(StaleProse(fragment, project, contexts, Identity(project, contexts)));
            string status = fragment.source == "xml" ? "SYNCED" : stale ? "OUTDATED" : "AUTHORED";
            string[] summaries = fragment.Summaries(string.Empty);
            fragment.metadata.Children.Remove(new YamlScalarNode("description"));
            string header = "---\n" + Serialize(fragment.metadata) + "\n---";
            StringBuilder text = new StringBuilder(header).Append("\n\n> [!")
                .Append(status)
                .Append("]\n> Project guide source status.\n\n# ")
                .Append(entry.project)
                .Append("\n\n");
            if (summaries.Length != 0)
            {
                text
                    .Append("> ")
                    .Append(
                        string.Join("\n> ", summaries.Select(summary => "- " + summary.Replace("\n", "\n>   ", StringComparison.Ordinal)))
                    )
                    .Append("\n\n");
            }
            text.Append(Regex.Replace(fragment.body, @"\A\s*# [^\r\n]+\r?\n", string.Empty));
            if (project is not null)
            {
                text.Append("\n\n## Source project facts\n\n**Project:** `")
                    .Append(project.name)
                    .Append("`  \n**Source:** `")
                    .Append(project.path)
                    .Append("`\n\n");
                IGrouping<string, DocumentationApiFact>[] types = contexts.Where(fact => fact.kind == "NamedType")
                    .GroupBy(fact => fact.key, StringComparer.Ordinal)
                    .OrderBy(group => group.Key, StringComparer.Ordinal)
                    .ToArray();
                if (types.Length != 0)
                {
                    text.Append("| Type | API | Description |\n| --- | --- | --- |\n");
                    foreach (IGrouping<string, DocumentationApiFact> type in types)
                    {
                        DocumentationApiFact fact = type.First();
                        DocumentationApiContentEntry api = apiEntries[fact.key];
                        text.Append("| [")
                            .Append(DocumentationApiPresentation.Cell(api.title))
                            .Append("](")
                            .Append(Path.GetRelativePath(Path.GetDirectoryName(entry.path)!, api.path).Replace('\\', '/'))
                            .Append(") | ")
                            .Append(fact.scope ?? fact.family)
                            .Append(" | ")
                            .Append(fragments[fact.key].TableDescription(fact.documentation))
                            .Append(" |\n");
                    }
                }
            }
            if (project is not null)
            {
                text.Append("\n<!-- api-source -->\n[Source declaration](")
                    .Append(binding.repository)
                    .Append("/blob/")
                    .Append(binding.revision)
                    .Append('/')
                    .Append(project.path)
                    .Append(")\n");
            }
            pages[entry.path] = text.ToString();
        }
    }

    private static bool IsGuidePath(string path)
        => path.StartsWith(C_GUIDES, StringComparison.Ordinal)
           || path.StartsWith(C_BACKEND_GUIDES, StringComparison.Ordinal)
           || path.StartsWith(C_TEST_GUIDES, StringComparison.Ordinal);

    // Plans a synchronized guide for each reader-facing project that no project page owns yet.
    // Authored project pages own their projects without a registry entry; synchronization never claims them.
    private static DocumentationProjectGuide[] NewGuides(
        string root,
        IEnumerable<DocumentationProject> projects,
        DocumentationProjectGuide[] registered
    ) {
        var owned = new HashSet<string>(registered.Select(entry => entry.project), StringComparer.Ordinal);
        owned.UnionWith(DocumentationApiPresentation.ProjectDescriptions(root).Keys);
        List<DocumentationProjectGuide> guides = [];
        foreach (DocumentationProject project in projects.OrderBy(project => project.name, StringComparer.Ordinal))
        {
            if (project.path.StartsWith("platforms/", StringComparison.Ordinal) || owned.Contains(project.name))
            {
                continue;
            }
            string slug = project.name.ToLowerInvariant().Replace('.', '-');
            string directory = project.path.StartsWith("tests/", StringComparison.Ordinal)
                               ? C_TEST_GUIDES
                               : project.path.StartsWith("backends/", StringComparison.Ordinal) ? C_BACKEND_GUIDES : C_GUIDES;
            guides.Add(new DocumentationProjectGuide(directory + slug + ".md", "project." + slug, project.name));
        }
        return guides.ToArray();
    }

    private static DocumentationApiFragment Read(
        DocumentationProjectGuide entry,
        string text
    ) {
        DocumentationApiFragment fragment = DocumentationApiFragment.Read(entry.path, text)
            ?? throw new InvalidDataException("Registered project guide is missing ownership metadata: " + entry.path);
        if (fragment.symbol != "project:" + entry.project || fragment.uid != entry.uid)
        {
            throw new InvalidDataException("Project guide identity differs from registry: " + entry.path);
        }
        return fragment;
    }

    private static string Identity(
        DocumentationProject? project,
        DocumentationApiFact[] contexts
    ) {
        var identity = new
        {
            metadata = project is null
            ? null
            : project with { surfaces = project.surfaces.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray() },
            symbols = contexts.GroupBy(fact => fact.key, StringComparer.Ordinal)
                .OrderBy(group => group.Key, StringComparer.Ordinal)
                .Select(group => new { symbol = group.Key, contract = DocumentationApiContent.Contract(group, normalizeXml: true) })
        };
        return DocumentationMarkdown.Hash(JsonSerializer.Serialize(identity));
    }

    private static string? StaleProse(
        DocumentationApiFragment fragment,
        DocumentationProject? project,
        DocumentationApiFact[] contexts,
        string contract
    ) {
        if (string.IsNullOrEmpty(fragment.sourceFingerprint) || fragment.sourceFingerprint == contract)
        {
            return fragment.StaleProse(contract);
        }
        string legacy = DocumentationMarkdown.Hash(JsonSerializer.Serialize(project) + "\n" + DocumentationApiContent.Contract(contexts));
        return fragment.StaleProse(contract, legacy);
    }

    private static string Metadata(
        DocumentationProjectGuide entry,
        DocumentationApiFragment fragment,
        string fingerprint,
        string? staleProse
    ) {
        var generated = new YamlMappingNode
        {
            { "uid", entry.uid },
            { "symbol", "project:" + entry.project },
            { "title", entry.project },
            { "kind", "project" },
            { "owners", new YamlSequenceNode(new YamlScalarNode(entry.project)) },
            { "sourceFingerprint", fingerprint },
            { "contracts", fragment.metadata.Children.TryGetValue(new YamlScalarNode("contracts"), out YamlNode? contracts)
                           ? contracts
                           : new YamlSequenceNode() }
        };
        if (staleProse is not null) generated.Add("outdatedFor", staleProse);
        var authored = new YamlMappingNode
        {
            { "source", fragment.source },
            { "description", fragment.metadata.Children.TryGetValue(new YamlScalarNode("description"), out YamlNode? description)
                             ? description
                             : new YamlScalarNode("null") { Style = ScalarStyle.Plain } },
            { "outdated", fragment.source != "xml" && fragment.explicitlyOutdated ? "true" : "false" }
        };
        foreach (KeyValuePair<YamlNode, YamlNode> field in fragment.metadata.Children)
        {
            if (((YamlScalarNode)field.Key).Value is not ("reviewed" or "outdatedFor")
                && !generated.Children.ContainsKey(field.Key) && !authored.Children.ContainsKey(field.Key)
            ) {
                authored.Add(field.Key, field.Value);
            }
        }
        return "---\n# Generated metadata. Maintained by project synchronization tooling.\n" + Serialize(generated)
               + "\n\n# Authoring metadata. Edit these fields and the explanation below.\n" + Serialize(authored) + "\n---";
    }

    private static string Body(
        DocumentationProjectGuide entry,
        DocumentationApiFact[] contexts
    ) {
        StringBuilder text = new StringBuilder("# ").Append(entry.project).Append("\n\n## Source contracts\n\n");
        IOrderedEnumerable<IGrouping<string, DocumentationApiFact>> types = contexts.Where(fact => fact.kind == "NamedType")
            .GroupBy(fact => fact.key, StringComparer.Ordinal)
            .OrderBy(group => group.Key, StringComparer.Ordinal);
        foreach (IGrouping<string, DocumentationApiFact> type in types)
        {
            DocumentationApiFact fact = type.First();
            text.Append("### ")
                .Append(WebUtility.HtmlEncode(fact.id[2..]))
                .Append(" (")
                .Append(fact.scope ?? fact.family)
                .Append(")\n\n");
            foreach (string summary in DocumentationXml.Read(fact.documentation).summaries)
            {
                text.Append(summary).Append("\n\n");
            }
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

internal sealed record DocumentationProjectGuide(
    string path,
    string uid,
    string project
);

internal sealed record DocumentationProjectGuideRegistry(
    string schema,
    DocumentationProjectGuide[] entries
);
