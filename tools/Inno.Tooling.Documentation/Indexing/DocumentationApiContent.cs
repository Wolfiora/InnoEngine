using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiContentEntry(
    string symbol,
    string path,
    string type,
    string title,
    string contract
);

internal sealed record DocumentationApiContentRegistry(
    string schema,
    IReadOnlyList<DocumentationApiContentEntry> entries,
    IReadOnlyList<string>? navigation = null
);

internal sealed record DocumentationApiEdit(
    string path,
    string? before,
    string? text
);

internal sealed record DocumentationApiWarning(
    string symbol,
    string path,
    string code,
    string message
);

internal sealed record DocumentationApiPlan(
    IReadOnlyList<DocumentationApiEdit> edits,
    IReadOnlyList<DocumentationApiWarning> warnings,
    int symbols
);

internal static class DocumentationApiContent
{
    internal const string C_REGISTRY = "reference/api/content.json";

    internal static DocumentationApiPlan Plan(
        string root,
        DocumentationBinding binding
    ) {
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts = DocumentationApiFacts.Read(root, binding);
        if (facts.Count == 0)
        {
            throw new InvalidDataException("Complete API synchronization discovered no eligible symbols.");
        }
        string registryPath = DocumentationApiFacts.Resolve(root, C_REGISTRY);
        DocumentationApiContentRegistry? previous = File.Exists(registryPath)
            ? JsonSerializer.Deserialize<DocumentationApiContentRegistry>(File.ReadAllBytes(registryPath))
            : null;
        if (previous is not null && previous.schema != "inno-api-content/1")
        {
            throw new InvalidDataException("Unsupported API content registry.");
        }
        Dictionary<string, DocumentationApiContentEntry> prior = (previous?.entries
                                                                  ?? []).ToDictionary(entry => entry.symbol, StringComparer.Ordinal);
        var registeredPaths = new HashSet<string>(
            (previous?.navigation ?? []).Concat(prior.Values.Select(entry => entry.path)),
            StringComparer.Ordinal
        );
        if (registeredPaths.Count != (previous?.navigation?.Count ?? 0) + prior.Count)
        {
            throw new InvalidDataException("API content registry repeats an owned path.");
        }
        var edits = new Dictionary<string, DocumentationApiEdit>(StringComparer.Ordinal);
        var warnings = new List<DocumentationApiWarning>();
        var entries = new List<DocumentationApiContentEntry>();
        Dictionary<string, string> typePaths = facts.Values.Where(contexts => contexts[0].kind == "NamedType")
            .ToDictionary(contexts => contexts[0].key, contexts => TypePath(contexts[0]), StringComparer.Ordinal);
        var categoryPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string typePath in typePaths.Values)
        {
            string? directory = Path.GetDirectoryName(Path.GetDirectoryName(typePath));
            while (directory is not null && directory.Replace('\\', '/').StartsWith("content/en/api/", StringComparison.Ordinal))
            {
                categoryPaths.Add(directory.Replace('\\', '/') + "/index.md");
                directory = Path.GetDirectoryName(directory);
            }
        }
        foreach (KeyValuePair<string, string> collision in typePaths.Where(pair => categoryPaths.Contains(pair.Value)).ToArray())
        {
            typePaths[collision.Key] = collision.Value.Replace(
                "/index.md",
                "-" + DocumentationMarkdown.Hash(collision.Key)[..12] + "/index.md",
                StringComparison.Ordinal
            );
        }
        foreach (IGrouping<string, KeyValuePair<string, string>> duplicates in typePaths
            .GroupBy(pair => pair.Value, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .ToArray()
        ) {
            foreach (KeyValuePair<string, string> duplicate in duplicates)
            {
                typePaths[duplicate.Key] = duplicate.Value.Replace(
                    "/index.md",
                    "-" + DocumentationMarkdown.Hash(duplicate.Key)[..12] + "/index.md",
                    StringComparison.Ordinal
                );
            }
        }
        foreach (KeyValuePair<string, DocumentationApiFact[]> pair in facts.OrderBy(pair => pair.Key, StringComparer.Ordinal))
        {
            DocumentationApiFact fact = pair.Value[0];
            string type = fact.kind == "NamedType" ? fact.key : fact.family + ":" + fact.project + ":" + fact.containingType;
            if (!typePaths.TryGetValue(type, out string? typePath))
            {
                throw new InvalidDataException("API member has no eligible declaring type: " + fact.key);
            }
            string title = DocumentationApiPresentation.Name(fact, facts);
            string fileName = string.Join('.', title.Split('.').Select(part => Slug(Regex.Replace(part, "<[^<>]+>", string.Empty))));
            string path = fact.kind == "NamedType"
                          ? typePath
                          : typePath.Replace(
                              "index.md",
                              fileName + "-" + DocumentationMarkdown.Hash(fact.key)[..16] + ".md",
                              StringComparison.Ordinal
                          );
            string contract = Contract(pair.Value);
            var entry = new DocumentationApiContentEntry(fact.key, path, type, title, contract);
            entries.Add(entry);
            DocumentationApiFragment? existing = null;
            if (prior.TryGetValue(fact.key, out DocumentationApiContentEntry? old))
            {
                string oldPath = RequireApiPath(root, old.path);
                if (File.Exists(oldPath))
                {
                    existing = DocumentationApiFragment.Read(old.path, File.ReadAllText(oldPath))
                               ?? throw new InvalidDataException("Registered API file lacks symbol metadata: " + old.path);
                }
                if (existing is not null && existing.symbol != fact.key)
                {
                    throw new InvalidDataException("Registered API identity changed: " + old.path);
                }
                if (old.path != path && existing is not null) Edit(old.path, null);
            }
            if (existing is not null && existing.source != "xml")
            {
                // Synchronization owns metadata; the authored body remains byte-for-byte intact.
                string fingerprint = Contract(pair.Value, normalizeXml: true);
                string? staleProse = existing.StaleProse(fingerprint, contract);
                bool stale = existing.IsOutdated(staleProse);
                Edit(path, Fragment(entry, fact, existing.body, existing, fingerprint, staleProse));
                if (string.IsNullOrWhiteSpace(existing.body)
                    || Normalize(existing.body) == Normalize(DocumentationXml.Read(fact.documentation).body)
                ) {
                    warnings.Add(new(fact.key, path, "EmptyManual", "Manual ownership has no independent explanatory content."));
                }
                if (stale)
                {
                    warnings.Add(
                        new(fact.key, path, "ReviewRequired", "The authored explanation is marked outdated for the current source.")
                    );
                }
            }
            else
            {
                Edit(
                    path,
                    Fragment(
                        entry,
                        fact,
                        DocumentationXml.Read(fact.documentation).body,
                        existing,
                        Contract(pair.Value, normalizeXml: true)
                    )
                );
                warnings.Add(new(fact.key, path, "XmlOnly", "This API still uses XML-synchronized prose."));
            }
        }
        foreach (DocumentationApiContentEntry removed in prior.Values.Where(entry => !facts.ContainsKey(entry.symbol)))
        {
            string path = RequireApiPath(root, removed.path);
            if (!File.Exists(path)) continue;
            DocumentationApiFragment fragment = DocumentationApiFragment.Read(removed.path, File.ReadAllText(path))
                ?? throw new InvalidDataException("Registered API file lacks symbol metadata: " + removed.path);
            if (fragment.symbol != removed.symbol)
            {
                throw new InvalidDataException("Removed API identity differs from its registration.");
            }
            if (fragment.source == "xml")
            {
                Edit(removed.path, null);
            }
            else
            {
                entries.Add(removed);
                warnings.Add(
                    new(removed.symbol, removed.path, "RemovedApi", "Preserved manual prose for an API absent from the current input.")
                );
            }
        }
        IReadOnlyDictionary<string, string> navigation = DocumentationApiNavigation.Create(
            entries,
            facts,
            DocumentationApiPresentation.ProjectDescriptions(root)
        );
        foreach (KeyValuePair<string, string> page in navigation)
        {
            Edit(page.Key, page.Value);
        }
        foreach (string removed in (previous?.navigation ?? []).Except(navigation.Keys, StringComparer.Ordinal))
        {
            Edit(removed, null);
        }
        Dictionary<string, string> moves = entries
            .Where(entry => prior.TryGetValue(entry.symbol, out DocumentationApiContentEntry? old) && old.path != entry.path)
            .ToDictionary(entry => prior[entry.symbol].path, entry => entry.path, StringComparer.Ordinal);
        var anchors = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (DocumentationApiContentEntry entry in entries.Where(
            entry => entry.type != entry.symbol && facts.ContainsKey(entry.symbol)
        )) {
            if (prior.TryGetValue(entry.type, out DocumentationApiContentEntry? oldType))
            {
                anchors[oldType.path + "#api-" + DocumentationMarkdown.Hash(facts[entry.symbol][0].id)[..16]] = entry.path;
            }
        }
        if (moves.Count != 0)
        {
            Dictionary<string, string> reverse = moves.ToDictionary(pair => pair.Value, pair => pair.Key, StringComparer.Ordinal);
            string[] paths = Directory.EnumerateFiles(Path.Combine(root, "content/en"), "*.md", SearchOption.AllDirectories)
                .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/'))
                .Concat(edits.Keys.Where(path => path.EndsWith(".md", StringComparison.Ordinal)))
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            foreach (string path in paths)
            {
                string? text = edits.TryGetValue(path, out DocumentationApiEdit? edit)
                               ? edit.text
                               : File.ReadAllText(DocumentationApiFacts.Resolve(root, path));
                if (text is null) continue;
                string rewritten = DocumentationApiLinks.Rewrite(root, reverse.GetValueOrDefault(path) ?? path, path, text, moves, anchors);
                if (text == rewritten) continue;
                string absolute = DocumentationApiFacts.Resolve(root, path);
                string? before = File.Exists(absolute) ? DocumentationMarkdown.Hash(File.ReadAllText(absolute)) : null;
                edits[path] = new(path, before, rewritten);
            }
        }
        Edit(
            C_REGISTRY,
            JsonSerializer.Serialize(
                new DocumentationApiContentRegistry(
                    "inno-api-content/1",
                    entries.OrderBy(entry => entry.symbol, StringComparer.Ordinal).ToArray(),
                    navigation.Keys.Order(StringComparer.Ordinal).ToArray()
                ),
                new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }
            ) + "\n"
        );
        DocumentationApiPlan guides = DocumentationProjectGuides.Plan(root, binding, facts);
        foreach (DocumentationApiEdit edit in guides.edits)
        {
            edits[edit.path] = edit;
        }
        warnings.AddRange(guides.warnings);
        return new(edits.Values.OrderBy(edit => edit.path, StringComparer.Ordinal).ToArray(), warnings, facts.Count);

        void Edit(
            string path,
            string? text
        ) {
            string absolute = path == C_REGISTRY ? registryPath : RequireApiPath(root, path);
            string? before = File.Exists(absolute) ? File.ReadAllText(absolute) : null;
            if (before == text) return;
            if (before is not null && path != C_REGISTRY && !registeredPaths.Contains(path))
            {
                throw new InvalidDataException("Synchronization cannot overwrite an unregistered API file: " + path);
            }
            edits[path] = new(path, before is null ? null : DocumentationMarkdown.Hash(before), text);
        }
    }

    internal static string Contract(
        IEnumerable<DocumentationApiFact> contexts,
        bool normalizeXml = false
    ) => DocumentationMarkdown.Hash(
        string.Join(
            '\n',
            contexts
                .Select(fact => (
                    fact.signatureHash + "\0"
                    + (normalizeXml ? fact.documentation.ReplaceLineEndings("\n") : fact.documentation)
                    + "\0" + fact.scope + "\0" + fact.configuration + "\0" + fact.target + "\0" + fact.surface
                ))
                .Distinct(StringComparer.Ordinal)
                .Order(StringComparer.Ordinal)
        )
    );

    internal static string Title(DocumentationApiFact fact)
    {
        if (fact.kind != "NamedType") return PathName(fact);
        string qualified = QualifiedTitle(fact);
        return qualified[(qualified.LastIndexOf('.') + 1)..];
    }

    internal static string QualifiedTitle(DocumentationApiFact fact)
    {
        string identity = fact.id[2..];
        if (!identity.Contains('`')) return identity;
        // Match the exact qualified type in the captured declaration. Type parameter names
        // come from source facts, never from editable XML prose or an invented T1/T2 list.
        string pattern = string.Join(
            @"\.",
            identity.Split('.').Select(part => ("@?" + Regex.Replace(Regex.Escape(part), "`[0-9]+", "<[^<>]+>")))
        );
        Match declaration = Regex.Match(fact.signature, @"(?<![\w.])" + pattern + @"(?=[\s(:]|$)", RegexOptions.CultureInvariant);
        if (!declaration.Success)
        {
            throw new InvalidDataException("API type identity cannot be located in its declaration: " + fact.id);
        }
        return declaration.Value;
    }

    internal static string Slug(string text) => Regex.Replace(text.ToLowerInvariant(), "[^a-z0-9]+", "-").Trim('-');

    internal static string TypePath(DocumentationApiFact fact)
    {
        string category = fact.family == "scripting"
            ? "scripting/" + fact.scope!.ToLowerInvariant()
            : "engine/"
              + string.Join(
                  '/',
                  (fact.project.StartsWith("Inno.", StringComparison.Ordinal) ? fact.project[5..] : fact.project).Split('.').Select(Slug)
              );
        return "content/en/api/" + category + "/" + Slug(PathName(fact)) + "/index.md";
    }

    internal static string RequireApiPath(
        string root,
        string path
    ) {
        string resolved = DocumentationApiFacts.Resolve(root, path);
        if (!path.StartsWith("content/en/api/engine/", StringComparison.Ordinal)
            && !path.StartsWith("content/en/api/scripting/", StringComparison.Ordinal)
            || !path.EndsWith(".md", StringComparison.Ordinal)
            || path != Path.GetRelativePath(root, resolved).Replace('\\', '/')
        ) {
            throw new InvalidDataException("Synchronization is restricted to registered engine/script API content: " + path);
        }
        return resolved;
    }

    private static string Fragment(
        DocumentationApiContentEntry entry,
        DocumentationApiFact fact,
        string body,
        DocumentationApiFragment? previous = null,
        string? fingerprint = null,
        string? staleProse = null
    ) {
        bool manual = previous is not null && previous.source != "xml";
        var generated = new YamlMappingNode
        {
            { "uid", previous?.uid ?? "api." + DocumentationMarkdown.Hash(entry.symbol) },
            { "symbol", entry.symbol },
            { "title", entry.title },
            { "kind", "api" },
            { "owners", new YamlSequenceNode(new YamlScalarNode(fact.project)) },
            { "apiSurface", fact.surface },
            { "sourceFingerprint", fingerprint ?? entry.contract }
        };
        if (staleProse is not null) generated.Add("outdatedFor", staleProse);
        if (fact.scope is not null) generated.Add("apiScope", fact.scope);
        generated.Add(
            "contracts",
            previous is not null && previous.metadata.Children.TryGetValue(new YamlScalarNode("contracts"), out YamlNode? contracts)
            ? contracts
            : new YamlSequenceNode()
        );
        var authored = new YamlMappingNode
        {
            { "source", manual ? previous!.source : "xml" },
            { "description", previous is not null
                             && previous.metadata.Children.TryGetValue(new YamlScalarNode("description"), out YamlNode? description)
                             ? description
                             : new YamlScalarNode("null") { Style = ScalarStyle.Plain } },
            { "outdated", manual && previous!.ExplicitlyOutdated ? "true" : "false" }
        };
        if (previous is not null)
        {
            foreach (KeyValuePair<YamlNode, YamlNode> field in previous.metadata.Children)
            {
                string key = ((YamlScalarNode)field.Key).Value!;
                if (generated.Children.ContainsKey(field.Key) || authored.Children.ContainsKey(field.Key)
                    || key is "apiScope" or "apiSurfaces" or "reviewed" or "outdatedFor"
                ) {
                    continue;
                }
                authored.Add(field.Key, field.Value);
            }
        }
        return "---\n# Generated metadata. Maintained by API synchronization tooling.\n"
               + Serialize(generated) + "\n\n# Authoring metadata. Edit these fields and the explanation below.\n"
               + Serialize(authored) + "\n---" + (manual ? body : body.Length == 0 ? "\n" : "\n\n" + body.Trim() + "\n");
    }

    private static string Serialize(YamlMappingNode metadata)
    {
        using var writer = new StringWriter();
        new YamlStream(new YamlDocument(metadata)).Save(writer, assignAnchors: false);
        string text = writer.ToString().ReplaceLineEndings("\n").TrimEnd();
        return text.EndsWith("\n...", StringComparison.Ordinal) ? text[..^4] : text;
    }

    private static string PathName(DocumentationApiFact fact)
    {
        string name = fact.id[2..].Split('(')[0];
        return Regex.Replace(name[(name.LastIndexOf('.') + 1)..], "`+[0-9]+", string.Empty)
            .Replace("#ctor", "Constructor", StringComparison.Ordinal)
            .Replace("#cctor", "Static constructor", StringComparison.Ordinal);
    }

    private static string Normalize(string text) => Regex.Replace(text, "\\s+", " ").Trim();
}
