using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Tooling.Documentation.Indexing;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Publishes immutable documentation generations and opens protected read-only snapshots.
/// No operation invokes an SDK, product build or network client.
/// </summary>
public static class DocumentationStore
{
    internal static JsonSerializerOptions json { get; } = new() { WriteIndented = false };
    // Snapshot format compatibility is a parser/index protocol, not a compilation artifact identity.
    // MVID includes build provenance and can differ between adapters compiling identical parser sources.
    internal static string schema { get; } = CreateSchemaIdentity();

    /// <summary>
    /// Explicitly scans a Docs checkout and publishes a consistent generation using reusable parsed pages.
    /// </summary>
    /// <param name="workspaceRoot">
    /// Absolute workspace owning the cache registration; may be the engine or Docs checkout.
    /// </param>
    /// <param name="documentationRoot">
    /// Explicit Docs checkout containing content/en and reference/engine.lock.json. No sibling path is inferred.
    /// </param>
    /// <param name="cacheRoot">
    /// Exclusive documentation cache boundary beneath workspaceRoot, outside the authored content tree.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels scanning, waiting and preparation before atomic selection. The previous generation remains readable.
    /// </param>
    /// <returns>
    /// The published snapshot identity and the number of parsed and reused pages; source API remains unverified.
    /// </returns>
    /// <exception cref="IOException">
    /// Inputs change during preparation, paths escape ownership, or a reference, contract or inclusion is invalid.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation is canceled. Completed immutable payloads remain registered for safe maintenance.
    /// </exception>
    public static async Task<DocumentationResult> IndexAsync(
        string workspaceRoot,
        string documentationRoot,
        string cacheRoot,
        CancellationToken cancellationToken = default
    ) {
        var total = Stopwatch.StartNew();
        var phase = Stopwatch.StartNew();
        var timings = new Dictionary<string, double>(StringComparer.Ordinal);
        double parsingMilliseconds = 0;
        double registrationMilliseconds = 0;
        string root = Path.GetFullPath(documentationRoot);
        string content = PathBoundary.RequireUnlinkedPath(root, Path.Combine(root, "content", "en"));
        string output = PathBoundary.RequireUnlinkedPath(Path.GetFullPath(workspaceRoot), Path.GetFullPath(cacheRoot));
        if (output == root || output.StartsWith(content + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new ArgumentException("Documentation cache must be outside authored content.", nameof(cacheRoot));
        var publisher = new BuildCachePublisher(output, BuildCacheArtifactRole.ImmutableArtifact);
        BuildCacheOwnerCatalog.Register(workspaceRoot, output, "documentation-snapshots", publisher.role, cancellationToken);
        using FileLease activity = await publisher.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        using FileLease writing = await FileLease.AcquireAsync(Path.Combine(output, "writer.lock"), Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        string bindingPath = PathBoundary.RequireUnlinkedPath(root, Path.Combine(root, "reference", "engine.lock.json"));
        byte[] bindingBytes = File.ReadAllBytes(bindingPath);
        DocumentationBinding binding = Deserialize<DocumentationBinding>(bindingBytes);
        RequireHash(binding.sourceTree, "engine source tree", allowGitTree: true);
        RequireHash(binding.revision, "engine revision", allowGitTree: true);
        string projectPath = PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, binding.projectManifest));
        byte[] projectBytes = File.ReadAllBytes(projectPath);
        if (DocumentationMarkdown.Hash(projectBytes) != binding.projectManifestIdentity)
            throw new InvalidDataException("Project manifest does not match the engine binding.");
        DocumentationProject[] projects = Deserialize<DocumentationProject[]>(projectBytes);
        DocumentationManifest? previous = TryReadCurrent(output);
        var reusable = previous?.schema == schema
            ? previous.pages.Values.ToDictionary(static page => page.key, StringComparer.Ordinal)
            : new Dictionary<string, DocumentationPageReference>(StringComparer.Ordinal);
        var pages = new List<DocumentationPage>();
        var entries = new SortedDictionary<string, DocumentationPageReference>(StringComparer.Ordinal);
        int parsed = 0;
        RecordPhase("SetupAndLockWait");
        foreach (string path in EnumerateContent(content))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relative = Path.GetRelativePath(root, path).Replace('\\', '/');
            string text = File.ReadAllText(path);
            string key = DocumentationMarkdown.Hash(schema + "\n" + relative + "\n" + text);
            string directory = Path.Combine(output, "pages", key);
            string payload = PathBoundary.RequireUnlinkedPath(output, Path.Combine(directory, "page.json"));
            string lease = Path.Combine(output, "leases", key + ".lock");
            using FileLease pageWriting = await FileLease.AcquireAsync(lease, Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
            DocumentationPage? page = null;
            byte[]? bytes = null;
            if (reusable.TryGetValue(key, out DocumentationPageReference? known) && File.Exists(payload))
            {
                bytes = File.ReadAllBytes(payload);
                if (DocumentationMarkdown.Hash(bytes) == known.payloadIdentity)
                    page = Deserialize<DocumentationPage>(bytes);
                if (page?.text != text || page.path != relative || page.identity != DocumentationMarkdown.Hash(text))
                    page = null;
            }
            if (page is null)
            {
                long started = Stopwatch.GetTimestamp();
                page = DocumentationMarkdown.Parse(relative, text);
                parsingMilliseconds += Stopwatch.GetElapsedTime(started).TotalMilliseconds;
                bytes = JsonSerializer.SerializeToUtf8Bytes(page, json);
                cancellationToken.ThrowIfCancellationRequested();
                AtomicFile.WriteAllBytes(payload, bytes);
                parsed++;
            }
            long registrationStarted = Stopwatch.GetTimestamp();
            publisher.Record(directory, lease, [], null, null);
            registrationMilliseconds += Stopwatch.GetElapsedTime(registrationStarted).TotalMilliseconds;
            pages.Add(page);
            entries.Add(relative, new(page.metadata.uid, key, DocumentationMarkdown.Hash(bytes!), page.metadata));
        }
        if (pages.Count == 0)
            throw new InvalidDataException("The selected Docs checkout has no content. Prepare a documentation source before querying.");
        RecordPhase("PagePreparation");
        timings.Add("MarkdownParsing", parsingMilliseconds);
        timings.Add("OwnerRegistration", registrationMilliseconds);
        // The source manifest supplies project facts; authored page metadata supplies each canonical entry.
        projects = projects.Select(project => project with
        {
            entry = pages.Where(page => page.metadata.kind == "project" && page.metadata.owners.Contains(project.name, StringComparer.Ordinal))
                .Select(static page => page.metadata.uid).SingleOrDefault()
        }).ToArray();
        DocumentationCatalog.Validate(root, pages, projects);
        RecordPhase("CatalogValidation");
        var search = pages.SelectMany(page => page.sections.Select(section =>
        {
            string body = DocumentationMarkdown.OwnText(page, section);
            string terms = section.title + " " + body + " " + string.Join(' ', page.metadata.aliases)
                + " " + string.Join(' ', page.metadata.owners);
            return new DocumentationSearchEntry(page.metadata.uid, section.anchor, section.title,
                body[..Math.Min(body.Length, 320)], DocumentationMarkdown.Tokens(terms));
        })).ToArray();
        var symbols = pages.SelectMany(page => page.symbols.Select(symbol => new DocumentationSymbolReference(
            page.metadata.owners.Single(), page.metadata.apiSurface!, symbol.id, page.metadata.uid, symbol.anchor))).ToArray();
        var manifest = new DocumentationManifest("", schema, binding, entries, projects, search, symbols);
        string identity = DocumentationMarkdown.Hash(JsonSerializer.SerializeToUtf8Bytes(manifest, json));
        manifest = manifest with { identity = identity };
        string generation = Path.Combine(output, "generations", identity);
        string generationLease = Path.Combine(output, "leases", identity + ".lock");
        RecordPhase("IndexProjection");
        // Recheck actual contents, including additions and same-length timestamp-preserving edits.
        string[] current = EnumerateContent(content).Select(path => Path.GetRelativePath(root, path).Replace('\\', '/')).ToArray();
        if (!current.SequenceEqual(entries.Keys, StringComparer.Ordinal)
            || pages.Any(page => File.ReadAllText(Path.Combine(root, page.path)) != page.text)
            || !File.ReadAllBytes(bindingPath).SequenceEqual(bindingBytes)
            || !File.ReadAllBytes(projectPath).SequenceEqual(projectBytes))
            throw new IOException("Documentation inputs changed during preparation. The previous snapshot remains selected; refresh a stable source.");
        cancellationToken.ThrowIfCancellationRequested();
        RecordPhase("FreshnessValidation");
        if (previous?.identity == identity)
            return Finish($"Confirmed {pages.Count} pages; parsed {parsed}, reused {pages.Count - parsed}. Existing generation retained without rewriting its manifest.");
        using FileLease publishing = await FileLease.AcquireAsync(generationLease, Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        AtomicFile.WriteAllBytes(Path.Combine(generation, "manifest.json"), JsonSerializer.SerializeToUtf8Bytes(manifest, json));
        publisher.Record(generation, generationLease, entries.Values.Select(page => Path.Combine(output, "pages", page.key)).ToArray(),
            "documentation", null, cancellationToken);
        // Cancellation is observed before this commit point; selection cannot expose a partial generation.
        AtomicFile.WriteAllBytes(Path.Combine(output, "current.json"), JsonSerializer.SerializeToUtf8Bytes(identity));
        return Finish($"Published {pages.Count} pages; parsed {parsed}, reused {pages.Count - parsed}. No source API verification was performed.");

        void RecordPhase(string name)
        {
            timings.Add(name, phase.Elapsed.TotalMilliseconds);
            phase.Restart();
        }

        DocumentationResult Finish(string message)
        {
            RecordPhase("Publication");
            timings.Add("Total", total.Elapsed.TotalMilliseconds);
            return new("Indexed", identity, "Unverified", message, [], [])
            {
                timingsMilliseconds = new ReadOnlyDictionary<string, double>(timings)
            };
        }
    }

    /// <summary>
    /// Opens one immutable generation without scanning authored content or acquiring an update lock.
    /// </summary>
    /// <param name="cacheRoot">
    /// Absolute prepared cache root. This method does not prepare or download missing content.
    /// </param>
    /// <param name="snapshot">
    /// Exact generation identity, or null to resolve the currently published pointer once.
    /// </param>
    /// <param name="expectedSourceTree">
    /// Expected engine input tree, or null when the caller only needs the explicitly bound documentation snapshot.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels reading lease acquisition and manifest loading.
    /// </param>
    /// <returns>
    /// A caller-owned query session. Dispose it after the final query to release its generation reading lease.
    /// </returns>
    /// <exception cref="IOException">
    /// The snapshot is missing, corrupt, outside its boundary or produced by an incompatible parser schema.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Reading acquisition is canceled.
    /// </exception>
    public static async Task<DocumentationSnapshot> OpenAsync(
        string cacheRoot,
        string? snapshot = null,
        string? expectedSourceTree = null,
        CancellationToken cancellationToken = default
    ) {
        string output = Path.GetFullPath(cacheRoot);
        PathBoundary.RequireUnlinkedPath(output, output);
        string activityPath = PathBoundary.RequireUnlinkedPath(output, Path.Combine(output, "owner.lock"));
        if (!File.Exists(activityPath))
            throw new FileNotFoundException("No prepared documentation snapshot. Run docs index with an explicit documentation source.", activityPath);
        using FileLease activity = await FileLease.AcquireSharedAsync(activityPath, Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        string identity = snapshot ?? Deserialize<string>(File.ReadAllBytes(PathBoundary.RequireUnlinkedPath(output, Path.Combine(output, "current.json"))));
        RequireHash(identity, "snapshot");
        FileLease reading = await FileLease.AcquireSharedAsync(PathBoundary.RequireUnlinkedPath(output,
            Path.Combine(output, "leases", identity + ".lock")), Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        try
        {
            DocumentationManifest manifest = ReadManifest(output, identity);
            cancellationToken.ThrowIfCancellationRequested();
            return new(output, manifest, reading, expectedSourceTree);
        }
        catch
        {
            reading.Dispose();
            throw;
        }
    }

    internal static T Deserialize<T>(byte[] bytes)
        => JsonSerializer.Deserialize<T>(bytes, json) ?? throw new InvalidDataException("Documentation payload is null.");

    internal static void RequireHash(
        string value,
        string label,
        bool allowGitTree = false
    ) {
        if (value is null || (value.Length != 64 && (!allowGitTree || value.Length != 40))
            || value.Any(static character => !char.IsAsciiHexDigitLower(character)))
            throw new InvalidDataException("Invalid " + label + " identity.");
    }

    private static string[] EnumerateContent(string content)
        => PathBoundary.EnumerateFiles(content).Where(static path => path.EndsWith(".md", StringComparison.Ordinal))
            .OrderBy(path => Path.GetRelativePath(content, path).Replace('\\', '/'), StringComparer.Ordinal).ToArray();

    private static string CreateSchemaIdentity()
    {
        var assembly = typeof(DocumentationStore).Assembly;
        string[] sources = assembly.GetManifestResourceNames().Where(static name => name.StartsWith("DocumentationSource/", StringComparison.Ordinal))
            .Order(StringComparer.Ordinal).ToArray();
        if (sources.Length == 0)
            throw new InvalidDataException("Documentation tooling lacks its authored query protocol inputs.");
        var inputs = new List<string>();
        foreach (string source in sources)
        {
            using Stream stream = assembly.GetManifestResourceStream(source)
                ?? throw new InvalidDataException("Missing query protocol input: " + source);
            using var reader = new StreamReader(stream);
            inputs.Add(source + "\0" + DocumentationMarkdown.Hash(reader.ReadToEnd().Replace("\r\n", "\n", StringComparison.Ordinal)));
        }
        return DocumentationMarkdown.C_SCHEMA + ":" + DocumentationMarkdown.Hash(string.Join('\n', inputs));
    }

    private static DocumentationManifest ReadManifest(
        string output,
        string identity
    ) {
        string path = PathBoundary.RequireUnlinkedPath(output, Path.Combine(output, "generations", identity, "manifest.json"));
        DocumentationManifest manifest = Deserialize<DocumentationManifest>(File.ReadAllBytes(path));
        if (manifest.identity != identity || manifest.schema != schema
            || DocumentationMarkdown.Hash(JsonSerializer.SerializeToUtf8Bytes(manifest with { identity = "" }, json)) != identity)
            throw new InvalidDataException("Documentation snapshot integrity or schema mismatch; explicitly rebuild its index.");
        return manifest;
    }

    private static DocumentationManifest? TryReadCurrent(string output)
    {
        try
        {
            string pointer = PathBoundary.RequireUnlinkedPath(output, Path.Combine(output, "current.json"));
            if (!File.Exists(pointer))
                return null;
            string identity = Deserialize<string>(File.ReadAllBytes(pointer));
            RequireHash(identity, "snapshot");
            return ReadManifest(output, identity);
        }
        catch (JsonException) { return null; }
        catch (InvalidDataException) { return null; }
        catch (FileNotFoundException) { return null; }
    }
}
