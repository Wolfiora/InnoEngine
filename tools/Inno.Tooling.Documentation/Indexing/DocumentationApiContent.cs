using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiContentEntry(string symbol, string path, string type, string title, string contract);
internal sealed record DocumentationApiContentRegistry(string schema, IReadOnlyList<DocumentationApiContentEntry> entries, IReadOnlyList<string>? navigation = null);
internal sealed record DocumentationApiEdit(string path, string? before, string? text);
internal sealed record DocumentationApiWarning(string symbol, string path, string code, string message);
internal sealed record DocumentationApiPlan(IReadOnlyList<DocumentationApiEdit> edits, IReadOnlyList<DocumentationApiWarning> warnings, int symbols);

internal static class DocumentationApiContent
{
    internal const string C_REGISTRY = "reference/api/content.json";

    internal static DocumentationApiPlan Plan(string root, DocumentationBinding binding)
    {
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts = DocumentationApiFacts.Read(root, binding);
        if (facts.Count == 0) throw new InvalidDataException("Complete API synchronization discovered no eligible symbols.");
        string registryPath = DocumentationApiFacts.Resolve(root, C_REGISTRY);
        DocumentationApiContentRegistry? previous = File.Exists(registryPath)
            ? JsonSerializer.Deserialize<DocumentationApiContentRegistry>(File.ReadAllBytes(registryPath)) : null;
        if (previous is not null && previous.schema != "inno-api-content/1") throw new InvalidDataException("Unsupported API content registry.");
        var prior = (previous?.entries ?? []).ToDictionary(entry => entry.symbol, StringComparer.Ordinal);
        var registeredPaths = new HashSet<string>((previous?.navigation ?? []).Concat(prior.Values.Select(entry => entry.path)), StringComparer.Ordinal);
        if (registeredPaths.Count != (previous?.navigation?.Count ?? 0) + prior.Count)
            throw new InvalidDataException("API content registry repeats an owned path.");
        var edits = new Dictionary<string, DocumentationApiEdit>(StringComparer.Ordinal);
        var warnings = new List<DocumentationApiWarning>();
        var entries = new List<DocumentationApiContentEntry>();
        var typePaths = facts.Values.Where(contexts => contexts[0].kind == "NamedType")
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
        foreach (var collision in typePaths.Where(pair => categoryPaths.Contains(pair.Value)).ToArray())
            typePaths[collision.Key] = collision.Value.Replace("/index.md", "-" + DocumentationMarkdown.Hash(collision.Key)[..12] + "/index.md", StringComparison.Ordinal);
        foreach (var duplicates in typePaths.GroupBy(pair => pair.Value, StringComparer.OrdinalIgnoreCase).Where(group => group.Count() > 1).ToArray())
            foreach (var duplicate in duplicates)
                typePaths[duplicate.Key] = duplicate.Value.Replace("/index.md", "-" + DocumentationMarkdown.Hash(duplicate.Key)[..12] + "/index.md", StringComparison.Ordinal);
        foreach (var pair in facts.OrderBy(pair => pair.Key, StringComparer.Ordinal))
        {
            DocumentationApiFact fact = pair.Value[0];
            string type = fact.kind == "NamedType" ? fact.key : fact.family + ":" + fact.project + ":" + fact.containingType;
            if (!typePaths.TryGetValue(type, out string? typePath)) throw new InvalidDataException("API member has no eligible declaring type: " + fact.key);
            string path = fact.kind == "NamedType" ? typePath
                : typePath.Replace("index.md", "_members/" + Slug(Title(fact)) + "-" + DocumentationMarkdown.Hash(fact.key)[..16] + ".md", StringComparison.Ordinal);
            string contract = Contract(pair.Value);
            var entry = new DocumentationApiContentEntry(fact.key, path, type, Title(fact), contract);
            entries.Add(entry);
            DocumentationApiFragment? existing = null;
            if (prior.TryGetValue(fact.key, out DocumentationApiContentEntry? old))
            {
                string oldPath = RequireApiPath(root, old.path);
                if (File.Exists(oldPath)) existing = DocumentationApiFragment.Read(old.path, File.ReadAllText(oldPath))
                    ?? throw new InvalidDataException("Registered API file lacks symbol metadata: " + old.path);
                if (existing is not null && existing.symbol != fact.key) throw new InvalidDataException("Registered API identity changed: " + old.path);
                if (old.path != path && existing is not null) Edit(old.path, null);
            }
            if (existing?.source == "manual")
            {
                // An unchanged file is never rewritten, even when declarations or XML changed.
                if (existing.path != path) Edit(path, existing.text);
                if (string.IsNullOrWhiteSpace(existing.body) || Normalize(existing.body) == Normalize(DocumentationXml.Render(fact.documentation)))
                    warnings.Add(new(fact.key, path, "EmptyManual", "Manual ownership has no independent explanatory content."));
                if (existing.reviewed != contract)
                    warnings.Add(new(fact.key, path, "ReviewRequired", "The current source contract has not been acknowledged by this manual fragment."));
            }
            else
            {
                Edit(path, Fragment(entry, fact, DocumentationXml.Render(fact.documentation)));
                warnings.Add(new(fact.key, path, "XmlOnly", "This API still uses XML-synchronized prose."));
            }
        }
        foreach (DocumentationApiContentEntry removed in prior.Values.Where(entry => !facts.ContainsKey(entry.symbol)))
        {
            string path = RequireApiPath(root, removed.path);
            if (!File.Exists(path)) continue;
            DocumentationApiFragment fragment = DocumentationApiFragment.Read(removed.path, File.ReadAllText(path))
                ?? throw new InvalidDataException("Registered API file lacks symbol metadata: " + removed.path);
            if (fragment.symbol != removed.symbol) throw new InvalidDataException("Removed API identity differs from its registration.");
            if (fragment.source == "xml") Edit(removed.path, null);
            else
            {
                entries.Add(removed);
                warnings.Add(new(removed.symbol, removed.path, "RemovedApi", "Preserved manual prose for an API absent from the current input."));
            }
        }
        IReadOnlyDictionary<string, string> navigation = DocumentationApiNavigation.Create(entries, facts);
        foreach (var page in navigation) Edit(page.Key, page.Value);
        foreach (string removed in (previous?.navigation ?? []).Except(navigation.Keys, StringComparer.Ordinal)) Edit(removed, null);
        Edit(C_REGISTRY, JsonSerializer.Serialize(new DocumentationApiContentRegistry("inno-api-content/1", entries.OrderBy(entry => entry.symbol, StringComparer.Ordinal).ToArray(), navigation.Keys.Order(StringComparer.Ordinal).ToArray()),
            new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }) + "\n");
        return new(edits.Values.OrderBy(edit => edit.path, StringComparer.Ordinal).ToArray(), warnings, facts.Count);

        void Edit(string path, string? text)
        {
            string absolute = path == C_REGISTRY ? registryPath : RequireApiPath(root, path);
            string? before = File.Exists(absolute) ? File.ReadAllText(absolute) : null;
            if (before == text) return;
            if (before is not null && path != C_REGISTRY && !registeredPaths.Contains(path))
                throw new InvalidDataException("Synchronization cannot overwrite an unregistered API file: " + path);
            edits[path] = new(path, before is null ? null : DocumentationMarkdown.Hash(before), text);
        }
    }

    internal static string Fragment(DocumentationApiContentEntry entry, DocumentationApiFact fact, string body)
        => "---\nuid: api." + DocumentationMarkdown.Hash(entry.symbol) + "\nsymbol: " + JsonSerializer.Serialize(entry.symbol)
            + "\nsource: xml\nreviewed: " + entry.contract + "\ntitle: " + JsonSerializer.Serialize(entry.title)
            + "\nkind: api\nowners: [" + fact.project + "]\napiSurface: " + fact.surface
            + (fact.scope is null ? "" : "\napiScope: " + fact.scope) + "\n---\n\n" + body.Trim() + "\n";

    internal static string Contract(IEnumerable<DocumentationApiFact> contexts)
        => DocumentationMarkdown.Hash(string.Join('\n', contexts.Select(fact => fact.signatureHash + "\0" + fact.documentation
            + "\0" + fact.scope + "\0" + fact.configuration + "\0" + fact.target + "\0" + fact.surface).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal)));

    internal static string Title(DocumentationApiFact fact)
    {
        string name = fact.id[2..].Split('(')[0];
        return Regex.Replace(name[(name.LastIndexOf('.') + 1)..], "`+[0-9]+", "")
            .Replace("#ctor", "Constructor", StringComparison.Ordinal).Replace("#cctor", "Static constructor", StringComparison.Ordinal);
    }

    internal static string Slug(string text)
        => Regex.Replace(text.ToLowerInvariant(), "[^a-z0-9]+", "-").Trim('-');

    internal static string TypePath(DocumentationApiFact fact)
    {
        string category = fact.family == "scripting" ? "scripting/" + fact.scope!.ToLowerInvariant()
            : "engine/" + string.Join('/', (fact.project.StartsWith("Inno.", StringComparison.Ordinal) ? fact.project[5..] : fact.project).Split('.').Select(Slug));
        return "content/en/api/" + category + "/" + Slug(Title(fact)) + "/index.md";
    }

    internal static string RequireApiPath(string root, string path)
    {
        string resolved = DocumentationApiFacts.Resolve(root, path);
        if (!path.StartsWith("content/en/api/engine/", StringComparison.Ordinal)
            && !path.StartsWith("content/en/api/scripting/", StringComparison.Ordinal)
            || !path.EndsWith(".md", StringComparison.Ordinal)
            || path != Path.GetRelativePath(root, resolved).Replace('\\', '/'))
            throw new InvalidDataException("Synchronization is restricted to registered engine/script API content: " + path);
        return resolved;
    }

    private static string Normalize(string text) => Regex.Replace(text, "\\s+", " ").Trim();
}
