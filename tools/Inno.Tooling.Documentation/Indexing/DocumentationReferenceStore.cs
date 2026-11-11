using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationReferenceStore
{
    internal sealed record Prepared(
        IReadOnlyList<DocumentationReferenceInfo> references,
        IReadOnlyDictionary<string, byte[]> inputs
    );

    internal static async Task<Prepared> PrepareAsync(
        string root,
        string output,
        DocumentationBinding binding,
        IReadOnlyList<DocumentationProject> projects,
        byte[] projectBytes,
        BuildCachePublisher publisher,
        CancellationToken cancellation
    ) {
        var inputs = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        var references = new List<DocumentationReferenceInfo>();
        var owners = projects.ToDictionary(static project => project.name, StringComparer.Ordinal);
        await AddAsync(new(binding.projectManifest, binding.projectManifestIdentity, "project-inventory", null, null,
            binding.revision, binding.sourceTree, projects.Select(static project => project.name).Order(StringComparer.Ordinal).ToArray()),
            projectBytes).ConfigureAwait(false);
        foreach (DocumentationApiCapture capture in binding.apiManifests ?? [])
        {
            cancellation.ThrowIfCancellationRequested();
            DocumentationStore.RequireHash(capture.sha256, "API reference");
            DocumentationStore.RequireHash(capture.sourceRevision, "API source revision", allowGitTree: true);
            DocumentationStore.RequireHash(capture.sourceTree, "API source tree", allowGitTree: true);
            string path = Resolve(root, capture.path);
            byte[] bytes = File.ReadAllBytes(path);
            if (DocumentationMarkdown.Hash(bytes) != capture.sha256)
                throw new InvalidDataException("API reference identity differs from its binding: " + capture.path);
            using JsonDocument payload = JsonDocument.Parse(bytes);
            JsonElement facts = payload.RootElement;
            string? target = facts.TryGetProperty("target", out JsonElement selectedTarget) ? selectedTarget.GetString() : null;
            if (string.IsNullOrWhiteSpace(capture.model) || string.IsNullOrWhiteSpace(capture.configuration)
                || facts.GetProperty("model").GetString() != capture.model
                || facts.GetProperty("configuration").GetString() != capture.configuration || target != capture.target)
                throw new InvalidDataException("API reference context differs from its binding: " + capture.path);
            var selected = new HashSet<string>(StringComparer.Ordinal);
            foreach (JsonElement project in facts.GetProperty("projects").EnumerateArray())
            {
                string name = project.GetProperty("name").GetString()!;
                if (!owners.TryGetValue(name, out DocumentationProject? owner) || !selected.Add(name)
                    || owner.path != project.GetProperty("project").GetString()
                    || !owner.surfaces.Contains(capture.model, StringComparer.Ordinal))
                    throw new InvalidDataException("API reference owner or surface is absent, repeated or mismatched: " + name);
            }
            if (selected.Count == 0)
                throw new InvalidDataException("API reference contains no project facts: " + capture.path);
            await AddAsync(new(capture.path, capture.sha256, capture.model, capture.configuration, capture.target,
                capture.sourceRevision, capture.sourceTree, selected.Order(StringComparer.Ordinal).ToArray()), bytes).ConfigureAwait(false);
        }
        return new(references.OrderBy(static reference => reference.path, StringComparer.Ordinal).ToArray(), inputs);

        async Task AddAsync(
            DocumentationReferenceInfo reference,
            byte[] bytes
        ) {
            string source = Resolve(root, reference.path);
            if (!inputs.TryAdd(source, bytes))
                throw new InvalidDataException("Repeated documentation reference path: " + reference.path);
            string directory = Path.Combine(output, "references", reference.identity);
            string file = PathBoundary.RequireUnlinkedPath(output, Path.Combine(directory, "reference.json"));
            string lease = PathBoundary.RequireUnlinkedPath(output, Path.Combine(output, "leases", "reference-" + reference.identity + ".lock"));
            using FileLease writing = await FileLease.AcquireAsync(lease, Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false);
            if (!File.Exists(file) || DocumentationMarkdown.Hash(File.ReadAllBytes(file)) != reference.identity)
            {
                cancellation.ThrowIfCancellationRequested();
                AtomicFile.WriteAllBytes(file, bytes);
            }
            publisher.EnsureImmutableRegistration(directory, lease, [], null, cancellation);
            references.Add(reference);
        }
    }

    internal static void Verify(Prepared prepared)
    {
        foreach (var input in prepared.inputs)
            if (!File.ReadAllBytes(input.Key).AsSpan().SequenceEqual(input.Value))
                throw new IOException("Documentation reference changed during preparation: " + input.Key);
    }

    private static string Resolve(
        string root,
        string relative
    ) {
        string path = PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, relative));
        if (!relative.StartsWith("reference/", StringComparison.Ordinal) || !relative.EndsWith(".json", StringComparison.Ordinal)
            || relative != Path.GetRelativePath(root, path).Replace('\\', '/'))
            throw new InvalidDataException("Expected a canonical JSON path within the documentation reference owner: " + relative);
        return path;
    }
}
