using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationApiNavigation
{
    internal static IReadOnlyDictionary<string, string> Create(
        IEnumerable<DocumentationApiContentEntry> entries,
        IReadOnlyDictionary<string, DocumentationApiFact[]> facts
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
            if (fact.family == "scripting") categories["scripting/" + fact.scope!.ToLowerInvariant()] = fact.scope;
            else
            {
                string[] names = (fact.project.StartsWith("Inno.", StringComparison.Ordinal) ? fact.project[5..] : fact.project).Split('.');
                string current = "engine";
                foreach (string name in names)
                {
                    current += "/" + DocumentationApiContent.Slug(name);
                    categories[current] = name;
                }
            }
        }
        foreach (var category in categories)
        {
            string path = "content/en/api/" + category.Key + "/index.md";
            var text = new StringBuilder("---\nuid: api.directory.").Append(category.Key.Replace('/', '.'))
                .Append("\ntitle: ").Append(JsonSerializer.Serialize(category.Value)).Append("\nkind: landing\nowners: []\n---\n\n# ")
                .Append(category.Value).Append("\n\n");
            if (category.Key == "engine") text.Append("Browse public C# engine and tooling contracts by their owning project. Backend, external-library and test APIs are outside this reference.\n\n");
            else if (category.Key == "scripting") text.Append("Browse the actual projected script API by its declared scope. Runtime contracts are also available to Editor scripts; Authoring annotations are compilation-time contracts.\n\n");
            else text.Append("Find types in this ").Append(category.Key.StartsWith("scripting/", StringComparison.Ordinal) ? "declared script scope" : "project category").Append(". Each type links to its exact declarations and member contracts.\n\n");
            foreach (var child in categories.Where(child => Parent(child.Key) == category.Key))
                text.Append("## ").Append(child.Value).Append("\n\n[Browse ").Append(child.Value).Append("](")
                    .Append(Relative(path, "content/en/api/" + child.Key + "/index.md")).Append(").\n\n");
            DocumentationApiContentEntry[] direct = types.Where(type => Parent(Parent(type.path)) == "content/en/api/" + category.Key)
                .OrderBy(type => type.title, StringComparer.OrdinalIgnoreCase).ThenBy(type => type.symbol, StringComparer.Ordinal).ToArray();
            if (category.Key.StartsWith("scripting/", StringComparison.Ordinal))
            {
                foreach (var letter in direct.GroupBy(type => char.IsAsciiLetter(type.title[0]) ? char.ToUpperInvariant(type.title[0]).ToString() : "Other"))
                {
                    text.Append("## ").Append(letter.Key).Append("\n\n");
                    foreach (DocumentationApiContentEntry type in letter) AppendType(text, path, type, direct, facts);
                    text.Append('\n');
                }
            }
            else foreach (DocumentationApiContentEntry type in direct)
            {
                text.Append("## ").Append(type.title.Replace("`", "", StringComparison.Ordinal)).Append("\n\n");
                AppendType(text, path, type, direct, facts);
                text.Append('\n');
            }
            pages.Add(path, text.ToString());
        }
        return pages;
    }

    private static void AppendType(StringBuilder text, string page, DocumentationApiContentEntry type,
        DocumentationApiContentEntry[] siblings, IReadOnlyDictionary<string, DocumentationApiFact[]> facts)
    {
        string title = siblings.Count(sibling => sibling.title == type.title) > 1 ? facts[type.symbol][0].id[2..] : type.title;
        text.Append("- [").Append(title.Replace("`", "", StringComparison.Ordinal)).Append("](")
            .Append(Relative(page, type.path)).Append(")\n");
    }

    private static string Parent(string path) => path.LastIndexOf('/') is int index && index >= 0 ? path[..index] : "";
    private static string Relative(string from, string to) => Path.GetRelativePath(Path.GetDirectoryName(from)!, to).Replace('\\', '/');
}
