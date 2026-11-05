using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture.Documentation;

internal sealed class DocumentationQuery
{
    private readonly string m_root;
    private readonly DocumentationIndex m_index;
    private readonly DocumentationCatalog m_catalog;

    internal DocumentationQuery(
        string root,
        DocumentationIndex index
    ) {
        m_root = root;
        m_index = index;
        m_catalog = index.catalog;
    }

    internal DocumentationResult Search(
        string query,
        string? project,
        int limit
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        if (limit is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(limit), "The result limit must be between 1 and 100.");
        if (project is not null)
            RequireProject(project);
        string[] tokens = Regex.Split(Regex.Replace(query, "([a-z])([A-Z])", "$1 $2"), @"[\s._:()<>/]+")
            .Where(static value => value.Length != 0).ToArray();
        var candidates = new List<(int score, DocumentationHit hit)>();
        foreach (DocumentationPage page in m_index.pages)
        {
            string? owner = m_catalog.owners.GetValueOrDefault(page.path);
            if (project is not null && owner != project)
                continue;
            foreach (DocumentationSection section in page.sections)
            {
                string body = OwnText(page, section);
                if (!tokens.All(token => (section.title + " " + body).Contains(token, StringComparison.OrdinalIgnoreCase))
                    && !string.Equals(owner, query, StringComparison.OrdinalIgnoreCase))
                    continue;
                int score = (owner?.Equals(query, StringComparison.OrdinalIgnoreCase) == true ? 100 : 0)
                    + (page.symbols.Any(symbol => symbol.section == section.anchor && symbol.id.Equals(query, StringComparison.Ordinal)) ? 200 : 0)
                    + (section.title.Contains(query, StringComparison.OrdinalIgnoreCase) ? 60 : 0);
                if (body.Contains(query, StringComparison.OrdinalIgnoreCase))
                    score += 30 + (owner is not null && section.level == 1 ? 15 : 0);
                score += tokens.Sum(token => (section.title.Contains(token, StringComparison.OrdinalIgnoreCase) ? 10 : 0)
                    + (body.Contains(token, StringComparison.OrdinalIgnoreCase) ? 1 : 0));
                if (score == 0)
                    continue;
                string summary = body.Replace('\r', ' ').Replace('\n', ' ').Trim();
                if (summary.Length > 320)
                {
                    int match = summary.IndexOf(query, StringComparison.OrdinalIgnoreCase);
                    int start = Math.Max(0, match - 100);
                    summary = (start == 0 ? "" : "… ") + summary[start..Math.Min(summary.Length, start + 320)]
                        + "… [search excerpt; read the complete section for constraints]";
                }
                candidates.Add((score, new(page.path, section.anchor, section.line, owner, summary)));
            }
        }
        DocumentationHit[] hits = candidates.OrderByDescending(static item => item.score)
            .ThenBy(static item => item.hit.page, StringComparer.Ordinal).ThenBy(static item => item.hit.line)
            .Take(limit).Select(static item => item.hit).ToArray();
        return Result(hits.Length == 0 ? "Empty" : "Found", $"{candidates.Count} matching sections; showing {hits.Length}. Excerpts are navigation, not complete contracts.", hits);
    }

    internal DocumentationResult Read(
        string? page,
        string? project,
        string? section,
        int offset = 0
    ) {
        if ((page is null) == (project is null))
            throw new ArgumentException("Specify exactly one page or project.");
        string path = project is null ? ResolvePage(page!) : RequireProject(project);
        DocumentationPage document = m_catalog.pages[path];
        DocumentationSection? selected = section is null ? null : document.sections.SingleOrDefault(item => item.anchor == section);
        if (section is not null && selected is null && offset == 0)
        {
            DocumentationSymbol? symbol = document.symbols.SingleOrDefault(item => item.anchor == section);
            if (symbol is not null && m_catalog.owners.TryGetValue(path, out string? owner))
                return Symbol(owner, symbol.id);
        }
        if (section is not null && selected is null)
            throw new ArgumentException("Unknown section: " + section);
        string text = selected is null ? document.text : document.text[selected.start..selected.end];
        if (offset < 0 || offset > text.Length)
            throw new ArgumentOutOfRangeException(nameof(offset));
        if (text.Length > 16000 && offset == 0)
        {
            DocumentationSection[] children = selected is null ? document.sections.Skip(1).ToArray()
                : document.sections.Where(item => item.start > selected.start && item.start < selected.end).ToArray();
            string introduction = selected is null ? OwnText(document, document.sections[0]) : OwnText(document, selected);
            if (children.Length != 0 && introduction.Length <= 16000)
                return new("Navigation", m_index.identity, "Unverified", "The complete selection is large. Read the listed sections; no omitted contract is claimed to be read.",
                [new(path, selected?.anchor ?? "", selected?.line ?? 1, m_catalog.owners.GetValueOrDefault(path),
                    introduction)],
                children.Select(item => new DocumentationContinuation(path, item.anchor, item.title)).ToArray());
        }
        if (text.Length - offset > 16000)
        {
            int end = text.LastIndexOf('\n', offset + 16000, 16000);
            if (end <= offset)
                end = offset + 16000;
            if (char.IsHighSurrogate(text[end - 1]))
                end--;
            return new("Partial", m_index.identity, "Unverified", "Original Markdown page segment; read the continuation before treating this contract as complete.",
                [new(path, selected?.anchor ?? "", selected?.line ?? 1, m_catalog.owners.GetValueOrDefault(path), text[offset..end])],
                [new(path, selected?.anchor ?? "", selected?.title ?? document.title, end)]);
        }
        return Result("Found", "Original Markdown; documentation freshness does not certify the source API.",
            [new(path, selected?.anchor ?? "", selected?.line ?? 1, m_catalog.owners.GetValueOrDefault(path), text[offset..])]);
    }

    internal DocumentationResult Symbol(
        string project,
        string id
    ) {
        string entry = RequireProject(project);
        var symbols = m_index.pages.Where(page => m_catalog.owners.GetValueOrDefault(page.path) == project)
            .SelectMany(page => page.symbols.Select(symbol => (page, symbol))).ToArray();
        var matches = symbols.Where(item => item.symbol.id == id).ToArray();
        if (matches.Length == 0)
        {
            var alternatives = symbols.Where(item => item.symbol.id.Contains(id, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (alternatives.Length == 0)
                throw new ArgumentException("Unknown symbol: " + id);
            return Result("Ambiguous", "Use an exact XML documentation ID; no overload was selected.",
                alternatives.Select(item => new DocumentationHit(item.page.path, item.symbol.section, 1, project, item.symbol.id)).ToArray());
        }
        if (matches.Length != 1)
            throw new InvalidDataException("A symbol has multiple authoritative declarations: " + id);
        (DocumentationPage page, DocumentationSymbol symbol) = matches[0];
        DocumentationSection section = page.sections.Single(item => item.anchor == symbol.section);
        string declaration = symbol.declaration.Length == 0 ? page.text[section.start..section.end] : symbol.declaration;
        var hits = new List<DocumentationHit>();
        var continuations = new List<DocumentationContinuation>();
        if (declaration.Length <= 16000)
            hits.Add(new(page.path, symbol.section, section.line, project, declaration));
        else
            continuations.Add(new(page.path, section.anchor, "Declaration and constraints: " + symbol.id));
        foreach (DocumentationPage document in m_index.pages.Where(candidate => candidate.path == entry
            || m_catalog.owners.GetValueOrDefault(candidate.path) == project))
        foreach (DocumentationSection contract in document.sections)
        {
            string body = OwnText(document, contract);
            if (document.symbols.Any(item => item.section == contract.anchor))
            {
                if (document.path != page.path || contract.anchor != symbol.section)
                    continue;
                body = DocumentationMarkdown.Guidance(body);
            }
            if (string.IsNullOrWhiteSpace(body))
                continue;
            if (body.Length <= 8000 && hits.Sum(static hit => hit.text.Length) + body.Length <= 16000)
                hits.Add(new(document.path, contract.anchor, contract.line, project, body));
            else
                continuations.Add(new(document.path, contract.anchor, contract.title));
        }
        return new(continuations.Count == 0 ? "Found" : "Partial", m_index.identity, "Unverified",
            "Declaration and project guidance. API markers are not current source verification evidence; continuations, when present, must also be read.", hits, continuations);
    }

    private string RequireProject(string project)
    {
        if (!m_catalog.projects.TryGetValue(project, out WikiProjectPage? owner))
            throw new ArgumentException("Unknown project: " + project);
        return Path.GetRelativePath(m_root, owner.page).Replace('\\', '/');
    }

    private string ResolvePage(string page)
    {
        string path = PathBoundary.Resolve(m_root, page);
        PathBoundary.RequireUnlinkedPath(Path.Combine(m_root, "docs"), path);
        string relative = Path.GetRelativePath(m_root, path).Replace('\\', '/');
        if (!m_catalog.pages.ContainsKey(relative))
            throw new FileNotFoundException("Unknown documentation page.", page);
        return relative;
    }

    private DocumentationResult Result(
        string status,
        string message,
        IReadOnlyList<DocumentationHit> hits
    ) => new(status, m_index.identity, "Unverified", message, hits, []);

    private static string OwnText(
        DocumentationPage page,
        DocumentationSection section
    ) {
        int end = page.sections.FirstOrDefault(candidate => candidate.start > section.start)?.start ?? page.text.Length;
        return page.text[section.start..Math.Min(section.end, end)];
    }
}
