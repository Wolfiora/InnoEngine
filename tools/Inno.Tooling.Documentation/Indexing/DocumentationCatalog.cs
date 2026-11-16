using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationCatalog
{
    internal static void Validate(
        string root,
        IReadOnlyList<DocumentationPage> pages,
        IReadOnlyList<DocumentationProject> projects
    ) {
        var paths = pages.ToDictionary(static page => page.path, StringComparer.Ordinal);
        var identities = pages.ToDictionary(static page => page.metadata.uid, StringComparer.Ordinal);
        var owners = projects.ToDictionary(static project => project.name, StringComparer.Ordinal);
        foreach (DocumentationProject project in projects)
        {
            if (project.entry is null)
                continue;
            if (!identities.TryGetValue(project.entry, out DocumentationPage? entry)
                || entry.metadata.kind != "project" || !entry.metadata.owners.Contains(project.name, StringComparer.Ordinal))
                throw new InvalidDataException("Project entry does not identify its actual owner: " + project.name);
        }
        foreach (DocumentationPage page in pages)
        {
            foreach (string link in page.links)
                ValidateLink(page, link);
            foreach (string owner in page.metadata.owners)
                if (!owners.ContainsKey(owner))
                    throw new InvalidDataException("Unknown project owner " + owner + ": " + page.path);
            if (page.metadata.examples is DocumentationExampleContext examples)
                foreach (string project in examples.projects)
                    if (!owners.ContainsKey(project))
                        throw new InvalidDataException("Unknown example project " + project + ": " + page.path);
            if (page.symbols.Count != 0 && (page.metadata.owners.Length != 1 || page.metadata.apiSurface is null))
                throw new InvalidDataException("API pages require one actual owner and an explicit surface: " + page.path);
            if (page.metadata.apiSurface is string surface && !page.metadata.owners.Any(owner => owners[owner].surfaces.Contains(surface, StringComparer.Ordinal)))
                throw new InvalidDataException("API surface is absent from the source manifest: " + page.path);
            Visit(page, new HashSet<string>(StringComparer.Ordinal));
            foreach (string contract in page.metadata.contracts)
            {
                string[] target = contract.Split('#', 2);
                if (!identities.TryGetValue(target[0], out DocumentationPage? required)
                    || (target.Length == 2 && !required.sections.Any(section => section.anchor == target[1])))
                    throw new InvalidDataException("Missing associated contract " + contract + ": " + page.path);
            }
        }
        var declarations = new HashSet<string>(StringComparer.Ordinal);
        foreach (DocumentationPage page in pages)
        foreach (DocumentationSymbol symbol in page.symbols)
            if (!declarations.Add(page.metadata.owners.Single() + ":" + page.metadata.apiSurface + ":" + symbol.id))
                throw new InvalidDataException("Duplicate authoritative API symbol: " + symbol.id);

        void ValidateLink(
            DocumentationPage page,
            string link
        ) {
            if (Uri.TryCreate(link, UriKind.Absolute, out Uri? external))
            {
                if (external.Scheme is not ("https" or "http" or "mailto" or "inno-doc"))
                    throw new InvalidDataException("Unsupported documentation link scheme: " + link);
                return;
            }
            string[] parts = link.Split('#', 2);
            string targetPath = Uri.UnescapeDataString(parts[0].Split('?', 2)[0]);
            string target = targetPath.Length == 0 ? Path.Combine(root, page.path)
                : Path.GetFullPath(targetPath, Path.GetDirectoryName(Path.Combine(root, page.path))!);
            PathBoundary.RequireUnlinkedPath(root, target);
            string relative = Path.GetRelativePath(root, target).Replace('\\', '/');
            if (!File.Exists(target))
                throw new InvalidDataException("Missing documentation link " + link + ": " + page.path);
            if (relative.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
            {
                if (!paths.TryGetValue(relative, out DocumentationPage? selected))
                    throw new InvalidDataException("Markdown links must resolve within the selected content catalog: " + link);
                if (parts.Length == 2 && parts[1].Length != 0)
                {
                    string anchor = Uri.UnescapeDataString(parts[1]);
                    if (!selected.sections.Any(section => section.anchor == anchor)
                        && !selected.symbols.Any(symbol => symbol.anchor == anchor))
                        throw new InvalidDataException("Missing documentation anchor " + link + ": " + page.path);
                }
            }
        }

        void Visit(
            DocumentationPage page,
            HashSet<string> visiting
        ) {
            if (!visiting.Add(page.path))
                throw new InvalidDataException("Cyclic or duplicate API inclusion: " + page.path);
            foreach (string include in page.includes)
            {
                string target = Path.GetFullPath(include, Path.GetDirectoryName(Path.Combine(root, page.path))!);
                PathBoundary.RequireUnlinkedPath(Path.Combine(root, "content", "en"), target);
                string relative = Path.GetRelativePath(root, target).Replace('\\', '/');
                if (!paths.TryGetValue(relative, out DocumentationPage? child))
                    throw new InvalidDataException("Missing included API page: " + relative);
                if (!page.metadata.owners.SequenceEqual(child.metadata.owners, StringComparer.Ordinal)
                    || page.metadata.apiSurface != child.metadata.apiSurface)
                    throw new InvalidDataException("API inclusion crosses project or surface ownership: " + relative);
                Visit(child, visiting);
            }
        }
    }
}
