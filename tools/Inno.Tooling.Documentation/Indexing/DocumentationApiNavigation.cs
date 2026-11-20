using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationApiNavigation
{
    internal static IReadOnlyDictionary<string, string> Create(
        IEnumerable<DocumentationApiContentEntry> entries,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts,
        IReadOnlyDictionary<string, string> descriptions,
        IReadOnlyDictionary<string, DocumentationApiFragment>? fragments = null
    ) {
        var pages = new SortedDictionary<string, string>(StringComparer.Ordinal);
        DocumentationApiContentEntry[] types = entries.Where(entry => entry.type == entry.symbol && facts.ContainsKey(entry.symbol)).ToArray();
        var categories = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["engine"] = "Engine API", ["scripting"] = "Scripting API"
        };
        foreach (DocumentationApiContentEntry type in types)
        {
            DocumentationApiFact fact = facts[type.symbol][0];
            string current = fact.family == "scripting" ? "scripting/" + fact.scope!.ToLowerInvariant() : "engine";
            if (fact.family == "scripting")
            {
                categories[current] = fact.scope!;
                current += "/_projects";
            }
            string[] names = (fact.project.StartsWith("Inno.", StringComparison.Ordinal) ? fact.project[5..] : fact.project).Split('.');
            foreach (string name in names)
            {
                current += "/" + DocumentationApiContent.Slug(name);
                categories[current] = name;
            }
        }
        foreach (var category in categories)
        {
            string path = "content/en/api/" + category.Key + "/index.md";
            var text = new StringBuilder("---\nuid: api.directory.").Append(category.Key.Replace('/', '.'))
                .Append("\ntitle: ").Append(JsonSerializer.Serialize(category.Value)).Append("\nkind: landing\nowners: []\n---\n\n# ")
                .Append(category.Value).Append("\n\n");
            if (category.Key == "engine") text.Append("Browse public C# contracts from src/ and build/ by their owning project. All other source roots, tests and fixtures are outside Engine API.\n\n");
            else if (category.Key == "scripting") text.Append("Browse the actual projected script API by its declared scope. Runtime contracts are also available to Editor scripts; Authoring annotations are compilation-time contracts.\n\n");
            else text.Append("Find types in this ").Append(category.Key.StartsWith("scripting/", StringComparison.Ordinal) && category.Key.Count(character => character == '/') == 1 ? "declared script scope" : "project category").Append(". Each type links to its exact declarations and member contracts.\n\n");
            var children = categories.Where(child => Parent(child.Key) == category.Key).ToArray();
            if (children.Length != 0)
            {
                text.Append("## Categories\n\n| Category | Purpose |\n| --- | --- |\n");
                foreach (var child in children)
                {
                    string prefix = "content/en/api/" + child.Key + "/";
                    string[] projects = types.Where(type => (Category(facts[type.symbol][0]) + "/").StartsWith(child.Key + "/", StringComparison.Ordinal))
                        .Select(type => facts[type.symbol][0].project).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
                    string description = projects.Length == 1 && descriptions.TryGetValue(projects[0], out string? value)
                        ? value : "Browse " + child.Value + " contracts" + (projects.Length == 0 ? "." : " from " + string.Join(", ", projects.Take(4)) + (projects.Length > 4 ? " and related projects." : "."));
                    text.Append("| [").Append(DocumentationApiPresentation.Cell(child.Value)).Append("](")
                        .Append(Relative(path, prefix + "index.md")).Append(") | ").Append(DocumentationApiPresentation.Cell(description)).Append(" |\n");
                }
                text.Append('\n');
            }
            DocumentationApiContentEntry[] direct = types.Where(type => Category(facts[type.symbol][0]) == category.Key)
                .OrderBy(type => type.title, StringComparer.OrdinalIgnoreCase).ThenBy(type => type.symbol, StringComparer.Ordinal).ToArray();
            if (direct.Length != 0)
            {
                text.Append("## Types\n\n| Type | Purpose |\n| --- | --- |\n");
                foreach (DocumentationApiContentEntry type in direct) AppendType(text, path, type, direct, facts, fragments);
                text.Append('\n');
            }
            if (types.Any(type => type.path == path)) throw new InvalidDataException("API category conflicts with a registered type: " + path);
            pages.Add(path, text.ToString());
        }
        return pages;
    }

    private static void AppendType(
        StringBuilder text,
        string page,
        DocumentationApiContentEntry type,
        DocumentationApiContentEntry[] siblings,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts,
        IReadOnlyDictionary<string, DocumentationApiFragment>? fragments
    ) {
        string title = siblings.Count(sibling => sibling.title == type.title) > 1
            ? DocumentationApiContent.QualifiedTitle(facts[type.symbol][0]) : type.title;
        text.Append("| [").Append(DocumentationApiPresentation.Cell(title)).Append("](")
            .Append(Relative(page, type.path)).Append(") | ")
            .Append(fragments is not null && fragments.TryGetValue(type.symbol, out var fragment)
                ? fragment.TableDescription(facts[type.symbol][0].documentation)
                : DocumentationApiPresentation.Cell(DocumentationApiPresentation.Summary(DocumentationXml.Read(facts[type.symbol][0].documentation).summary)))
            .Append(" |\n");
    }

    private static string Category(DocumentationApiFact fact)
    {
        string root = fact.family == "scripting" ? "scripting/" + fact.scope!.ToLowerInvariant() + "/_projects" : "engine";
        string project = fact.project.StartsWith("Inno.", StringComparison.Ordinal) ? fact.project[5..] : fact.project;
        return root + "/" + string.Join('/', project.Split('.').Select(DocumentationApiContent.Slug));
    }

    private static string Parent(string path)
    {
        string parent = path.LastIndexOf('/') is int index && index >= 0 ? path[..index] : "";
        return parent.EndsWith("/_projects", StringComparison.Ordinal) ? parent[..^10] : parent;
    }
    private static string Relative(
        string from,
        string to
    ) => Path.GetRelativePath(Path.GetDirectoryName(from)!, to).Replace('\\', '/');
}
