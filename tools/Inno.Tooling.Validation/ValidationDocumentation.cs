using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation;

internal static class ValidationDocumentation
{
    internal sealed record SourceFacts(
        string identity,
        bool empty
    );

    internal sealed record Owner(
        string path,
        bool test,
        bool nativeReference
    );

    internal sealed record Selection(
        ValidationDocumentationBinding binding,
        DocumentationCatalogReport catalog,
        IReadOnlyDictionary<string, Owner> owners,
        IReadOnlyDictionary<string, SourceFacts> sourceIdentities,
        IReadOnlySet<string> changedPages,
        IReadOnlySet<string> changedReferenceOwners
    );

    internal static async Task<Selection?> CaptureAsync(
        string? cache,
        string? identity,
        IReadOnlyDictionary<string, ValidationProject> projects,
        CancellationToken cancellation,
        string? baseline = null
    ) {
        if ((cache is null) != (identity is null))
            throw new ArgumentException("Documentation validation requires an explicit cache-root and snapshot pair.");
        if (cache is null)
        {
            if (baseline is not null)
                throw new ArgumentException("A documentation baseline requires the current cache and snapshot.");
            return null;
        }
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, identity,
            cancellationToken: cancellation).ConfigureAwait(false);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync(cancellation).ConfigureAwait(false);
        DocumentationReferenceInfo inventory = catalog.references.Single(reference => reference.model == "project-inventory");
        using JsonDocument source = JsonDocument.Parse(await snapshot.ReadReferenceAsync(inventory.path, cancellation).ConfigureAwait(false));
        var owners = new Dictionary<string, Owner>(StringComparer.Ordinal);
        foreach (JsonElement owner in source.RootElement.EnumerateArray())
        {
            string name = owner.GetProperty("name").GetString()!;
            string path = owner.GetProperty("path").GetString()!;
            if (!projects.TryGetValue(path, out ValidationProject? actual) || Path.GetFileNameWithoutExtension(path) != name)
                throw new InvalidDataException("Documentation inventory differs from the actual source owner: " + path);
            bool native = owner.TryGetProperty("apiDocumentation", out JsonElement scope) && scope.GetString() == "backend-native";
            if (native && (!path.StartsWith("backends/", StringComparison.Ordinal) || !path.Contains("/native/", StringComparison.Ordinal)))
                throw new InvalidDataException("Native reference exclusion crosses its declared backend owner: " + path);
            if (!owners.TryAdd(name, new(path, actual.test, native)))
                throw new InvalidDataException("Duplicate documentation project identity: " + name);
        }
        Dictionary<string, SourceFacts> sourceIdentities = await SourceIdentitiesAsync(snapshot, catalog).ConfigureAwait(false);
        var changedPages = new HashSet<string>(StringComparer.Ordinal);
        var changedOwners = new HashSet<string>(StringComparer.Ordinal);
        if (baseline is not null && baseline != snapshot.identity)
        {
            using DocumentationSnapshot previous = await DocumentationStore.OpenAsync(cache, baseline, cancellationToken: cancellation).ConfigureAwait(false);
            DocumentationCatalogReport prior = await previous.ExportCatalogAsync(cancellation).ConfigureAwait(false);
            var pages = prior.pages.ToDictionary(static page => page.id, StringComparer.Ordinal);
            var changedPaths = new HashSet<string>(StringComparer.Ordinal);
            foreach (DocumentationPageInfo page in catalog.pages)
                if (!pages.Remove(page.id, out DocumentationPageInfo? old) || old.contentIdentity != page.contentIdentity || old.path != page.path)
                {
                    changedPages.Add(page.id);
                    changedPaths.Add(page.path);
                    if (old is not null) changedPaths.Add(old.path);
                }
            foreach (DocumentationPageInfo removed in pages.Values)
            {
                changedPages.Add(removed.id);
                changedPaths.Add(removed.path);
            }
            bool expanded;
            do
            {
                expanded = false;
                foreach (DocumentationPageInfo page in catalog.pages)
                    if (page.includes.Any(changedPaths.Contains) || page.contracts.Any(contract => changedPages.Contains(contract.Split('#', 2)[0])))
                    {
                        expanded |= changedPages.Add(page.id);
                        changedPaths.Add(page.path);
                    }
            } while (expanded);
            Dictionary<string, SourceFacts> earlier = await SourceIdentitiesAsync(previous, prior).ConfigureAwait(false);
            foreach (var input in sourceIdentities)
                if (earlier.GetValueOrDefault(input.Key) != input.Value)
                    changedOwners.Add(input.Key.Split('\0')[1]);
            foreach (string removed in earlier.Keys.Except(sourceIdentities.Keys, StringComparer.Ordinal))
            {
                string name = removed.Split('\0')[1];
                if (owners.ContainsKey(name))
                    changedOwners.Add(name);
            }
        }
        return new(new(Path.GetFullPath(cache), snapshot.identity, snapshot.sourceTree, baseline), catalog, owners, sourceIdentities, changedPages, changedOwners);

        async Task<Dictionary<string, SourceFacts>> SourceIdentitiesAsync(
            DocumentationSnapshot reader,
            DocumentationCatalogReport report
        ) {
            var identities = new Dictionary<string, SourceFacts>(StringComparer.Ordinal);
            foreach (DocumentationReferenceInfo reference in report.references.Where(static reference => reference.model == "source-clr"))
            {
                using JsonDocument facts = JsonDocument.Parse(await reader.ReadReferenceAsync(reference.path, cancellation).ConfigureAwait(false));
                foreach (JsonElement project in facts.RootElement.GetProperty("projects").EnumerateArray())
                {
                    string key = reference.path + "\0" + project.GetProperty("name").GetString();
                    if (!identities.TryAdd(key, new(ValidationInputs.Identity([new("project", project.GetRawText())]),
                        project.GetProperty("symbols").GetArrayLength() == 0)))
                        throw new InvalidDataException("Repeated source facts in the documentation snapshot: " + key);
                }
            }
            return identities;
        }
    }

    internal static DocumentationPageInfo[] SelectExamples(
        Selection selection,
        IReadOnlySet<string> affected,
        bool full
    ) => selection.catalog.pages.Where(page => (full || selection.changedPages.Contains(page.id) || page.owners.Concat(page.examples?.projects ?? [])
            .Any(owner => selection.owners.TryGetValue(owner, out Owner? project) && affected.Contains(project.path)))
        && page.codeBlocks.Any(static block => block.language is "csharp" or "cs" && block.declarationSymbols.Count == 0)).ToArray();

    internal static async Task<DocumentationCatalogReport> ReadCatalogAsync(
        ValidationPlan plan,
        DocumentationSnapshot snapshot,
        CancellationToken cancellation
    ) {
        if (plan.documentation is not { } binding || snapshot.identity != binding.snapshot || snapshot.sourceTree != binding.sourceTree)
            throw new InvalidDataException("Documentation inputs differ from the frozen validation binding.");
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync(cancellation).ConfigureAwait(false);
        // A retained receipt still requires intact inputs, including the inventory that establishes project ownership.
        foreach (string reference in plan.checks.SelectMany(static check => check.documentation?.references ?? [])
            .Concat(catalog.references.Where(static reference => reference.model == "project-inventory").Select(static reference => reference.path))
            .Distinct(StringComparer.Ordinal))
            _ = await snapshot.ReadReferenceAsync(reference, cancellation).ConfigureAwait(false);
        return catalog;
    }

    internal static string PageIdentity(IEnumerable<DocumentationPageInfo> pages)
        => ValidationInputs.Identity(pages.Select(static page => new KeyValuePair<string, string>(page.id, page.contentIdentity)));
}
