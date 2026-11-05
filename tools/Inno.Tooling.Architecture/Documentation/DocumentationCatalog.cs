using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture.Documentation;

internal sealed class DocumentationCatalog
{
    internal IReadOnlyDictionary<string, DocumentationPage> pages { get; }
    internal IReadOnlyDictionary<string, WikiProjectPage> projects { get; }
    internal Dictionary<string, string> owners { get; } = new(StringComparer.Ordinal);

    internal DocumentationCatalog(
        string root,
        IReadOnlyList<DocumentationPage> pages,
        IReadOnlyList<WikiProjectPage>? projectPages = null,
        bool requireCompleteCoverage = true
    ) {
        this.pages = pages.ToDictionary(static page => page.path, StringComparer.Ordinal);
        var failures = new List<string>();
        projects = (projectPages ?? WikiProjectCoverageValidator.Validate(root, failures, requireCompleteCoverage))
            .ToDictionary(static project => Path.GetFileNameWithoutExtension(project.project), StringComparer.Ordinal);
        if (failures.Count != 0)
            throw new InvalidDataException(string.Join(Environment.NewLine, failures));
        foreach (var pair in projects)
        {
            string entry = Path.GetRelativePath(root, pair.Value.page).Replace('\\', '/');
            var visited = new HashSet<string>(StringComparer.Ordinal);
            Visit(entry, pair.Key, visited);
            string directory = entry[..^3] + "/";
            foreach (string fragment in this.pages.Keys.Where(path => path.StartsWith(directory, StringComparison.Ordinal)))
            {
                if (!visited.Contains(fragment))
                    Visit(fragment, pair.Key, visited);
            }
        }
        foreach (DocumentationPage page in pages)
            if ((page.includes.Count != 0 || page.hasApi) && !owners.ContainsKey(page.path))
                throw new InvalidDataException("API documentation has no project owner: " + page.path);

        void Visit(
            string path,
            string owner,
            HashSet<string> visited
        ) {
            if (!visited.Add(path))
                throw new InvalidDataException("Duplicate or cyclic API inclusion: " + path);
            if (owners.TryGetValue(path, out string? existing) && existing != owner)
                throw new InvalidDataException("A documentation fragment has multiple owners: " + path);
            owners[path] = owner;
            if (!this.pages.TryGetValue(path, out DocumentationPage? page))
                throw new InvalidDataException("Missing documentation fragment: " + path);
            foreach (string include in page.includes)
            {
                string target = Path.GetFullPath(include, Path.GetDirectoryName(Path.Combine(root, path))!);
                PathBoundary.RequireUnlinkedPath(Path.Combine(root, "docs"), target);
                Visit(Path.GetRelativePath(root, target).Replace('\\', '/'), owner, visited);
            }
        }
    }

    internal static DocumentationCatalog Read(
        string root,
        IReadOnlyList<WikiProjectPage>? projects = null,
        bool requireCompleteCoverage = true
    ) {
        DocumentationPage[] pages = (Directory.Exists(Path.Combine(root, "docs"))
                ? PathBoundary.EnumerateFiles(Path.Combine(root, "docs")) : [])
            .Where(static file => file.EndsWith(".md", StringComparison.Ordinal))
            .Select(file => DocumentationMarkdown.Parse(Path.GetRelativePath(root, file).Replace('\\', '/'),
                File.ReadAllText(file), includeSymbols: false)).ToArray();
        return new(root, pages, projects, requireCompleteCoverage);
    }
}
