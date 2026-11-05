using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture.Documentation;

internal sealed record DocumentationIndex(
    string identity,
    string toolIdentity,
    IReadOnlyList<DocumentationPage> pages,
    int rebuiltPages,
    DocumentationCatalog catalog
) {
    internal static async Task<DocumentationIndex> LoadAsync(
        string root,
        CancellationToken cancellation
    ) {
        string docs = Path.Combine(root, "docs");
        string output = PathBoundary.RequireUnlinkedPath(root, Path.Combine(root, "artifacts", "documentation"));
        string pageDirectory = Path.Combine(output, "pages");
        string leasePath = Path.Combine(output, "pages.lock");
        var publisher = new BuildCachePublisher(output, BuildCacheArtifactRole.ReusableWorkspace);
        BuildCacheOwnerCatalog.Register(root, output, "documentation-index", publisher.role, cancellation);
        using FileLease activity = await publisher.AcquireUseAsync(cancellation).ConfigureAwait(false);
        using CoordinatedFileLease lease = await CoordinatedFileLease.AcquireAsync(Path.Combine(output, "owner.lock"),
            leasePath, Timeout.InfiniteTimeSpan, cancellationToken: cancellation).ConfigureAwait(false);
        publisher.BeginWorkingDirectory(pageDirectory, leasePath, null, cancellation);
        string manifest = Path.Combine(output, "index.json");
        PathBoundary.RequireUnlinkedPath(output, manifest);
        string[] previous = [];
        if (File.Exists(manifest))
        {
            try
            {
                using JsonDocument published = JsonDocument.Parse(File.ReadAllText(manifest));
                previous = published.RootElement.GetProperty("entries").EnumerateObject()
                    .Select(static pair => pair.Value.GetString() ?? string.Empty)
                    .Where(static key => Regex.IsMatch(key, "^[a-f0-9]{64}$")).Distinct(StringComparer.Ordinal).ToArray();
            }
            catch (JsonException) { }
            catch (KeyNotFoundException) { }
            catch (InvalidOperationException) { }
        }
        string tool = DocumentationMarkdown.Hash(typeof(DocumentationIndex).Assembly.ManifestModule.ModuleVersionId + ":"
            + typeof(Markdig.Markdown).Assembly.ManifestModule.ModuleVersionId);
        var pages = new List<DocumentationPage>();
        var entries = new SortedDictionary<string, string>(StringComparer.Ordinal);
        int rebuilt = 0;
        foreach (string file in PathBoundary.EnumerateFiles(docs).Where(static path => path.EndsWith(".md", StringComparison.Ordinal))
            .Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            string relative = Path.GetRelativePath(root, file).Replace('\\', '/');
            string text = File.ReadAllText(file);
            string key = DocumentationMarkdown.Hash(tool + "\n" + relative + "\n" + text);
            string cache = Path.Combine(pageDirectory, key + ".json");
            PathBoundary.RequireUnlinkedPath(output, cache);
            DocumentationPage? page = null;
            if (File.Exists(cache))
            {
                try
                {
                    CachedPage? cached = JsonSerializer.Deserialize<CachedPage>(File.ReadAllText(cache));
                    page = cached?.page;
                    if (page?.path != relative || page.text != text || page.identity != DocumentationMarkdown.Hash(text)
                        || page.sections is null || page.symbols is null || page.includes is null || page.links is null
                        || cached!.identity != DocumentationMarkdown.Hash(JsonSerializer.Serialize(page, DocumentationOutput.options)))
                        page = null;
                }
                catch (JsonException) { }
            }
            if (page is null)
            {
                page = DocumentationMarkdown.Parse(relative, text);
                cancellation.ThrowIfCancellationRequested();
                var cached = new CachedPage(DocumentationMarkdown.Hash(JsonSerializer.Serialize(page, DocumentationOutput.options)), page);
                AtomicFile.WriteAllBytes(cache, JsonSerializer.SerializeToUtf8Bytes(cached, DocumentationOutput.options));
                rebuilt++;
            }
            pages.Add(page);
            entries.Add(relative, key);
        }
        // A failed include graph must never replace the last complete index.
        var catalog = new DocumentationCatalog(root, pages);
        string[] current = PathBoundary.EnumerateFiles(docs).Where(static path => path.EndsWith(".md", StringComparison.Ordinal))
            .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/')).Order(StringComparer.Ordinal).ToArray();
        if (!current.SequenceEqual(entries.Keys, StringComparer.Ordinal)
            || pages.Any(page => File.ReadAllText(Path.Combine(root, page.path)) != page.text))
            throw new IOException("Documentation changed during indexing; no complete index was published. Retry against a stable snapshot.");
        string projectIdentity = string.Join("\n", catalog.projects.OrderBy(static pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => pair.Key + ":" + Path.GetRelativePath(root, pair.Value.project).Replace('\\', '/')));
        string identity = DocumentationMarkdown.Hash(tool + "\n" + projectIdentity + "\n"
            + string.Join("\n", entries.Select(static pair => pair.Key + ":" + pair.Value)));
        cancellation.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(manifest, JsonSerializer.SerializeToUtf8Bytes(
            new { identity, toolIdentity = tool, entries }, DocumentationOutput.options));
        // Only prior published payloads grant deletion authority; unknown files and failed-run diagnostics stay intact.
        foreach (string key in previous.Except(entries.Values, StringComparer.Ordinal))
        {
            string retired = PathBoundary.RequireUnlinkedPath(output, Path.Combine(pageDirectory, key + ".json"));
            File.Delete(retired);
        }
        publisher.Record(pageDirectory, leasePath, [], "documentation", null, cancellation);
        return new(identity, tool, pages, rebuilt, catalog);
    }

    private sealed record CachedPage(
        string identity,
        DocumentationPage page
    );
}
