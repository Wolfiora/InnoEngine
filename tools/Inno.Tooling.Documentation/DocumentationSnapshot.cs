using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Tooling.Documentation.Indexing;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Owns a protected immutable snapshot and answers transport-independent documentation requests.
/// Concurrent queries are supported; the owner must finish them before disposing the session.
/// Exact reads and symbol lookups do not initialize keyword-search postings.
/// </summary>
public sealed class DocumentationSnapshot : IDisposable
{
    private const int C_MAX_CONTENT = 16000;
    private readonly string m_cache;
    private readonly DocumentationManifest m_manifest;
    private readonly FileLease m_reading;
    private readonly string? m_expectedSourceTree;
    private readonly Dictionary<string, string> m_paths;
    private readonly Lazy<Dictionary<string, int[]>> m_terms;
    private readonly Dictionary<string, DocumentationSymbolReference[]> m_symbols;
    private readonly Dictionary<string, DocumentationReferenceInfo> m_references;
    private readonly ConcurrentDictionary<string, DocumentationPage> m_pages = new(StringComparer.Ordinal);
    private bool m_disposed;

    /// <summary>
    /// Gets the exact generation shared by every result in this session.
    /// </summary>
    public string identity => m_manifest.identity;

    /// <summary>
    /// Gets the source-tree identity recorded by the documentation binding, without asserting source API verification.
    /// </summary>
    public string sourceTree => m_manifest.binding.sourceTree;

    /// <summary>
    /// Exports canonical metadata for coverage, navigation and rendered-anchor verification.
    /// Reads protected cached pages only; it does not rescan a checkout or refresh the snapshot.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels reads without changing the selected generation or source content.
    /// </param>
    /// <returns>
    /// Complete page and project metadata. Missing project entries remain null; API evidence remains explicit.
    /// </returns>
    /// <exception cref="IOException">
    /// A cached page is missing, corrupt or outside its owned boundary.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The session was opened for a different engine source tree.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Export was canceled before the complete report was returned.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The session has released its reading protection.
    /// </exception>
    public async Task<DocumentationCatalogReport> ExportCatalogAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        cancellationToken.ThrowIfCancellationRequested();
        if (m_expectedSourceTree is not null && m_expectedSourceTree != sourceTree)
            throw new InvalidOperationException("Cannot export a catalog bound to a different source tree.");
        var pages = new List<DocumentationPageInfo>();
        foreach (string path in m_manifest.pages.Keys.Order(StringComparer.Ordinal))
        {
            DocumentationPage page = await LoadAsync(m_manifest.pages[path].pageId, cancellationToken).ConfigureAwait(false);
            pages.Add(new(page.metadata.uid, path, page.identity, page.title, page.metadata.kind,
                page.metadata.owners, page.metadata.apiSurface,
                page.sections.ToDictionary(static section => section.anchor, static section => section.line, StringComparer.Ordinal),
                page.symbols.ToDictionary(static symbol => symbol.id,
                    static symbol => new DocumentationSymbolInfo(symbol.anchor, symbol.signatureIdentity, symbol.code), StringComparer.Ordinal),
                page.metadata.contracts, page.includes, page.codeBlocks, page.metadata.examples));
        }
        string contentIdentity = DocumentationMarkdown.Hash(string.Join('\n', pages.Select(static page => page.path + "\0" + page.contentIdentity)));
        return new(identity, contentIdentity, m_manifest.binding.repository, m_manifest.binding.revision,
            sourceTree, m_manifest.binding.toolingRevision, "Unverified",
            m_manifest.projects.ToDictionary(static project => project.name, static project => project.entry, StringComparer.Ordinal), pages,
            m_manifest.references!);
    }

    /// <summary>
    /// Reads one complete, hash-verified project or API reference from this protected generation.
    /// The original checkout is not consulted, and no SDK evaluation or network operation occurs.
    /// </summary>
    /// <param name="path">
    /// Exact reference path reported by <see cref="ExportCatalogAsync"/>.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the selected payload read without changing publication.
    /// </param>
    /// <returns>
    /// Caller-owned bytes of the recorded payload, without semantic certification.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The path is not a reference in this snapshot.
    /// </exception>
    /// <exception cref="IOException">
    /// The payload is absent or outside its owned boundary.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The payload's bytes do not match the recorded content identity.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The snapshot does not match the caller's expected engine source tree.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The snapshot has released its reading protection.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels the read.
    /// </exception>
    public async Task<byte[]> ReadReferenceAsync(
        string path,
        CancellationToken cancellationToken = default
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        cancellationToken.ThrowIfCancellationRequested();
        if (m_expectedSourceTree is not null && m_expectedSourceTree != sourceTree)
            throw new InvalidOperationException("Cannot read references bound to a different source tree.");
        if (!m_references.TryGetValue(path, out DocumentationReferenceInfo? reference))
            throw new ArgumentException("Reference is absent from the selected snapshot: " + path, nameof(path));
        DocumentationStore.RequireHash(reference.identity, "reference payload");
        string file = PathBoundary.RequireUnlinkedPath(m_cache, Path.Combine(m_cache, "references", reference.identity, "reference.json"));
        byte[] bytes = await File.ReadAllBytesAsync(file, cancellationToken).ConfigureAwait(false);
        if (DocumentationMarkdown.Hash(bytes) != reference.identity)
            throw new InvalidDataException("Documentation reference integrity mismatch: " + path);
        return bytes;
    }

    internal DocumentationSnapshot(
        string cache,
        DocumentationManifest manifest,
        FileLease reading,
        string? expectedSourceTree
    ) {
        m_cache = cache;
        m_manifest = manifest;
        m_reading = reading;
        m_expectedSourceTree = expectedSourceTree;
        m_paths = manifest.pages.ToDictionary(static pair => pair.Value.pageId, static pair => pair.Key, StringComparer.Ordinal);
        m_symbols = manifest.symbols.GroupBy(static symbol => symbol.id, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.ToArray(), StringComparer.Ordinal);
        m_references = manifest.references!.ToDictionary(static reference => reference.path, StringComparer.Ordinal);
        m_terms = new(() => CreateSearchPostings(manifest), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    /// <summary>
    /// Searches, reads, resolves symbols or resources, or reports binding status without mutating the index.
    /// </summary>
    /// <remarks>
    /// Equally ranked multiword heading matches prefer task guides. Exact symbol and identifier searches
    /// retain their reference priority; excerpts still require explicit contract reads.
    /// </remarks>
    /// <param name="request">
    /// A structured operation against this snapshot. Search excerpts are never returned as complete contracts.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels query work or page lease acquisition; the session remains reusable after cancellation.
    /// </param>
    /// <returns>
    /// Explicit content, empty, ambiguity, missing, continuation or mismatch results with snapshot provenance.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The operation, offset, limit or required selector is invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// A selected immutable payload is missing, corrupt or escapes the cache boundary.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled before its result was returned.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The session has released its reading protection.
    /// </exception>
    public async Task<DocumentationResult> QueryAsync(
        DocumentationRequest request,
        CancellationToken cancellationToken = default
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        if (request.offset < 0 || request.limit is < 1 or > 100)
            throw new ArgumentException("Documentation offset must be nonnegative and limit must be between 1 and 100.");
        if (m_expectedSourceTree is not null && m_expectedSourceTree != sourceTree)
            return Result("Mismatched", "The selected documentation is bound to a different engine input tree. Select or prepare a matching snapshot.");
        if (request.project is not null && !m_manifest.projects.Any(project => project.name == request.project))
            return Result("Missing", "Unknown project identity: " + request.project);
        if (request.operation == "status")
            return Result("Ready", $"Bound to {m_manifest.binding.repository}@{m_manifest.binding.revision}; source tree {sourceTree}. A prepared snapshot does not certify a mutable checkout or source API.");
        if (request.operation == "resource")
            return await ResourceAsync(request, cancellationToken).ConfigureAwait(false);
        if (request.operation == "search")
            return await SearchAsync(request, cancellationToken).ConfigureAwait(false);
        if (request.operation == "symbol")
            return await SymbolAsync(request, cancellationToken).ConfigureAwait(false);
        if (request.operation != "read")
            throw new ArgumentException("Unknown documentation query operation: " + request.operation);
        string? pageId = request.value;
        if (pageId is null && request.project is not null)
            pageId = m_manifest.projects.Single(project => project.name == request.project).entry;
        if (pageId is null)
            return Result("Missing", "Specify a page identity or a project with an authoritative entry.");
        if (!m_paths.ContainsKey(pageId))
        {
            if (!m_manifest.pages.TryGetValue(pageId.Replace('\\', '/'), out DocumentationPageReference? reference))
                return Result("Missing", "Unknown page in this snapshot: " + pageId);
            pageId = reference.pageId;
        }
        DocumentationPage page = await LoadAsync(pageId, cancellationToken).ConfigureAwait(false);
        DocumentationSection? section = request.section is null ? null : page.sections.SingleOrDefault(candidate => candidate.anchor == request.section);
        if (request.section is not null && section is null)
        {
            DocumentationSymbol? symbol = page.symbols.SingleOrDefault(candidate => candidate.anchor == request.section);
            if (symbol is not null)
                return await SymbolAsync(new("symbol", symbol.id, page.metadata.owners.Single(), page.metadata.apiSurface), cancellationToken).ConfigureAwait(false);
            return Result("Missing", "Unknown section: " + request.section);
        }
        string text = section is null ? page.text : page.text[section.start..section.end];
        if (request.offset > text.Length)
            throw new ArgumentOutOfRangeException(nameof(request), "Continuation offset exceeds the selected content.");
        int end = Math.Min(text.Length, request.offset + C_MAX_CONTENT);
        if (end < text.Length)
        {
            int newline = text.LastIndexOf('\n', end - 1, end - request.offset);
            if (newline >= request.offset)
                end = newline + 1;
            if (char.IsHighSurrogate(text[end - 1]))
                end--;
        }
        var continuations = Contracts(page);
        if (end < text.Length)
            continuations.Insert(0, new(pageId, section?.anchor, end, "Continue the original content before treating this selection as complete."));
        return new(end < text.Length || continuations.Count != 0 ? "Partial" : "Found", identity, "Unverified",
            "Original Markdown from the selected snapshot. Related contracts and continuations remain explicit.",
            [Hit(page, section?.anchor ?? "", section?.line ?? 1, null, "Exact page or section", end == text.Length ? "CompleteUnit" : "Partial", text[request.offset..end])], continuations,
            end < text.Length ? request with { offset = end } : null);
    }

    /// <summary>
    /// Releases this generation's shared reading protection after all its queries finish.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        m_disposed = true;
        m_reading.Dispose();
    }

    private async Task<DocumentationResult> SearchAsync(
        DocumentationRequest request,
        CancellationToken cancellation
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.value);
        if (m_symbols.TryGetValue(request.value, out DocumentationSymbolReference[]? exact))
        {
            DocumentationSymbolReference[] matches = exact.Where(symbol => (request.project is null || symbol.project == request.project)
                && (request.surface is null || symbol.surface == request.surface)).ToArray();
            var symbolHits = new List<DocumentationHit>();
            foreach (DocumentationSymbolReference symbol in matches.Skip(request.offset).Take(request.limit))
            {
                DocumentationPage page = await LoadAsync(symbol.pageId, cancellation).ConfigureAwait(false);
                symbolHits.Add(Hit(page, symbol.anchor, 1, symbol.id, "Exact qualified symbol", "Excerpt", symbol.id));
            }
            return Result(symbolHits.Count == 0 ? "Empty" : "Found", "Exact symbol navigation; use symbol or resource to read its contract.", symbolHits)
                with { nextRequest = request.offset + symbolHits.Count < matches.Length ? request with { offset = request.offset + symbolHits.Count } : null };
        }
        string[] tokens = DocumentationMarkdown.Tokens(request.value);
        if (tokens.Length == 0)
            return Result("Empty", "No searchable terms.");
        IEnumerable<int>? selected = null;
        Dictionary<string, int[]> terms = m_terms.Value;
        foreach (string token in tokens)
        {
            cancellation.ThrowIfCancellationRequested();
            if (!terms.TryGetValue(token, out int[]? postings))
                return Result("Empty", "No matching documentation. No content was inferred.");
            selected = selected is null ? postings : selected.Intersect(postings);
        }
        var ranked = selected!.Select(index => m_manifest.search[index])
            .Where(entry => Scope(m_manifest.pages[m_paths[entry.pageId]].metadata, request))
            .Select(entry => (entry, match: Score(entry, request.value, tokens)))
            .OrderByDescending(static pair => pair.match.score).ThenByDescending(static pair => pair.match.taskGuide)
            .ThenBy(static pair => pair.entry.pageId, StringComparer.Ordinal)
            .ThenBy(static pair => pair.entry.section, StringComparer.Ordinal).ToArray();
        var hits = new List<DocumentationHit>();
        foreach (var pair in ranked.Skip(request.offset).Take(request.limit))
        {
            DocumentationPage page = await LoadAsync(pair.entry.pageId, cancellation).ConfigureAwait(false);
            DocumentationSection section = page.sections.Single(candidate => candidate.anchor == pair.entry.section);
            string body = DocumentationMarkdown.OwnText(page, section);
            string excerpt = body[..Math.Min(body.Length, 320)];
            hits.Add(Hit(page, section.anchor, section.line, null, pair.match.reason, "Excerpt", excerpt));
        }
        return Result(hits.Count == 0 ? "Empty" : "Found",
            $"{ranked.Length} matching sections; offset {request.offset}, returned {hits.Count}. Search excerpts are navigation, not complete contracts.", hits)
            with { nextRequest = request.offset + hits.Count < ranked.Length ? request with { offset = request.offset + hits.Count } : null };
    }

    private static Dictionary<string, int[]> CreateSearchPostings(DocumentationManifest manifest)
        => manifest.search.SelectMany((
            entry,
            index
        ) => entry.tokens.Select(token => (token, index)))
            .GroupBy(static pair => pair.token, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key,
                static group => group.Select(static pair => pair.index).Distinct().ToArray(), StringComparer.Ordinal);

    private (int score, bool taskGuide, string reason) Score(
        DocumentationSearchEntry entry,
        string query,
        string[] tokens
    ) {
        DocumentationMetadata metadata = m_manifest.pages[m_paths[entry.pageId]].metadata;
        var heading = new HashSet<string>(DocumentationMarkdown.Tokens(entry.title), StringComparer.Ordinal);
        int matched = tokens.Count(heading.Contains);
        bool exactHeading = entry.title.Equals(query, StringComparison.OrdinalIgnoreCase);
        bool exactProject = metadata.owners.Contains(query, StringComparer.OrdinalIgnoreCase);
        bool exactAlias = metadata.aliases.Contains(query, StringComparer.OrdinalIgnoreCase);
        bool taskGuide = tokens.Length > 1 && query.Trim().Any(char.IsWhiteSpace) && matched == tokens.Length
            && metadata.kind is "how-to" or "tutorial";
        int score = (exactHeading ? 100 : 0)
            + (entry.title.Contains(query, StringComparison.OrdinalIgnoreCase) ? 60 : 0)
            + (exactAlias ? 80 : 0) + (exactProject ? 90 : 0)
            + 50 * matched / tokens.Length;
        string reason = exactHeading ? "Exact heading"
            : exactProject ? "Exact project identity"
            : exactAlias ? "Exact query alias"
            : matched == tokens.Length ? "All query terms in heading"
            : matched > 0 ? "Heading and indexed body terms"
            : "Indexed body terms";
        return (score, taskGuide, reason);
    }

    private async Task<DocumentationResult> SymbolAsync(
        DocumentationRequest request,
        CancellationToken cancellation
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.value);
        DocumentationSymbolReference[] matches = m_symbols.GetValueOrDefault(request.value, [])
            .Where(symbol => (request.project is null || symbol.project == request.project)
                && (request.surface is null || symbol.surface == request.surface)).ToArray();
        if (matches.Length != 1)
        {
            DocumentationSymbolReference[] candidates = matches.Length > 1 ? matches : m_manifest.symbols
                .Where(symbol => symbol.id.Contains(request.value, StringComparison.OrdinalIgnoreCase)
                    && (request.project is null || symbol.project == request.project)
                    && (request.surface is null || symbol.surface == request.surface)).ToArray();
            var hits = new List<DocumentationHit>();
            foreach (DocumentationSymbolReference candidate in candidates.Skip(request.offset).Take(request.limit))
            {
                DocumentationPage page = await LoadAsync(candidate.pageId, cancellation).ConfigureAwait(false);
                hits.Add(Hit(page, candidate.anchor, 1, candidate.id, "Qualified symbol candidate", "Excerpt", candidate.id));
            }
            return Result(candidates.Length == 0 ? "Missing" : "Ambiguous",
                $"{candidates.Length} candidates; offset {request.offset}, returned {hits.Count}. Qualify the exact XML ID, project and surface. No overload was chosen.", hits)
                with { nextRequest = request.offset + hits.Count < candidates.Length ? request with { offset = request.offset + hits.Count } : null };
        }
        DocumentationPage owner = await LoadAsync(matches[0].pageId, cancellation).ConfigureAwait(false);
        DocumentationSymbol selected = owner.symbols.Single(symbol => symbol.id == request.value);
        DocumentationSection section = owner.sections.Single(candidate => candidate.anchor == selected.section);
        var continuations = Contracts(owner);
        string text = selected.declaration;
        if (text.Length > C_MAX_CONTENT)
        {
            continuations.Insert(0, new(owner.metadata.uid, section.anchor, 0, "Read the complete declaration and its constraints."));
            return new("Navigation", identity, "Unverified", "The declaration requires an explicit section read.", [], continuations);
        }
        return new(continuations.Count == 0 ? "Found" : "Partial", identity, "Unverified",
            "Exact documented declaration; associated contracts remain explicit. Symbol presence is not source API verification.",
            [Hit(owner, selected.anchor, section.line, selected.id, "Exact qualified symbol", "CompleteUnit", text)], continuations);
    }

    private async Task<DocumentationResult> ResourceAsync(
        DocumentationRequest request,
        CancellationToken cancellation
    ) {
        if (!Uri.TryCreate(request.value, UriKind.Absolute, out Uri? resource) || resource.Scheme != "inno-doc"
            || resource.Host != "innoengine" || resource.Query.Length != 0 || resource.UserInfo.Length != 0)
            throw new ArgumentException("Invalid documentation resource URI.");
        string[] parts = resource.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4 || parts[0] != "snapshots" || parts[2] != "pages")
            throw new ArgumentException("Invalid documentation resource path.");
        if (parts[1] != identity)
            return Result("Mismatched", "The resource belongs to a different snapshot. Open that exact generation explicitly.");
        return await QueryAsync(new("read", Uri.UnescapeDataString(parts[3]), section: resource.Fragment.Length == 0
            ? null : Uri.UnescapeDataString(resource.Fragment[1..]), offset: request.offset), cancellation).ConfigureAwait(false);
    }

    private async Task<DocumentationPage> LoadAsync(
        string pageId,
        CancellationToken cancellation
    ) {
        cancellation.ThrowIfCancellationRequested();
        if (m_pages.TryGetValue(pageId, out DocumentationPage? loaded))
            return loaded;
        string path = m_paths[pageId];
        DocumentationPageReference reference = m_manifest.pages[path];
        DocumentationStore.RequireHash(reference.key, "page payload");
        string lease = PathBoundary.RequireUnlinkedPath(m_cache, Path.Combine(m_cache, "leases", reference.key + ".lock"));
        using FileLease reading = await FileLease.AcquireSharedAsync(lease, Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false);
        string payload = PathBoundary.RequireUnlinkedPath(m_cache, Path.Combine(m_cache, "pages", reference.key, "page.json"));
        byte[] bytes = await File.ReadAllBytesAsync(payload, cancellation).ConfigureAwait(false);
        if (DocumentationMarkdown.Hash(bytes) != reference.payloadIdentity)
            throw new InvalidDataException("Corrupt documentation page payload. Explicitly refresh its index: " + path);
        DocumentationPage page = DocumentationStore.Deserialize<DocumentationPage>(bytes);
        if (page.path != path || page.metadata.uid != pageId)
            throw new InvalidDataException("Documentation page identity mismatch: " + path);
        return m_pages.GetOrAdd(pageId, page);
    }

    private List<DocumentationContinuation> Contracts(DocumentationPage page)
        => page.metadata.contracts.Select(contract =>
        {
            string[] parts = contract.Split('#', 2);
            return new DocumentationContinuation(parts[0], parts.Length == 2 ? parts[1] : null, 0, "Necessary associated contract");
        }).ToList();

    private DocumentationHit Hit(
        DocumentationPage page,
        string section,
        int line,
        string? symbol,
        string reason,
        string completeness,
        string text
    ) => new(page.metadata.uid, page.path, section, line, page.metadata.owners, page.metadata.apiSurface, symbol, reason, completeness, text,
        "inno-doc://innoengine/snapshots/" + identity + "/pages/" + Uri.EscapeDataString(page.metadata.uid)
        + (section.Length == 0 ? "" : "#" + Uri.EscapeDataString(section)));

    private DocumentationResult Result(
        string status,
        string message,
        IReadOnlyList<DocumentationHit>? hits = null
    ) => new(status, identity, "Unverified", message, hits ?? [], []);

    private static bool Scope(
        DocumentationMetadata metadata,
        DocumentationRequest request
    ) => (request.project is null || metadata.owners.Contains(request.project, StringComparer.Ordinal))
        && (request.surface is null || metadata.apiSurface == request.surface);
}
