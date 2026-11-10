using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Adapts the shared documentation contracts to the existing Build command line.
/// </summary>
public static class DocumentationCommand
{
    /// <summary>
    /// Explicitly indexes a selected checkout or queries an already prepared immutable snapshot.
    /// </summary>
    /// <param name="arguments">
    /// index, search, read, symbol, resource, status or catalog followed by engine-root, cache-root, docs-root,
    /// snapshot, source-tree, query, page, project, surface, section, id, offset, limit and format as applicable.
    /// Index requires docs-root. Read-only commands reject docs-root rather than implicitly refreshing mutable content.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation or reading while preserving the previous complete generation.
    /// </param>
    /// <returns>
    /// Zero for valid results including empty searches; two for missing, ambiguous or mismatched targets.
    /// Partial results have explicit continuations and do not assert contract completeness.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Options are absent, duplicated or invalid for the operation.
    /// </exception>
    /// <exception cref="IOException">
    /// The selected source or snapshot cannot be safely read or published.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels the operation.
    /// </exception>
    public static async Task<int> ExecuteAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Length == 0 || arguments[0] is not ("index" or "search" or "read" or "symbol" or "resource" or "status" or "catalog"))
            throw new ArgumentException("Usage: docs <index|search|read|symbol|resource|status|catalog> --engine-root ROOT [--cache-root PATH] [--format text|json]. Index requires --docs-root CHECKOUT; reads require a prepared snapshot.");
        string operation = arguments[0];
        string[] specific = operation switch
        {
            "index" => ["--docs-root"],
            "search" => ["--query", "--project", "--surface", "--limit", "--offset"],
            "read" => ["--page", "--project", "--section", "--offset"],
            "symbol" => ["--id", "--project", "--surface", "--limit", "--offset"],
            "resource" => ["--uri", "--offset"],
            _ => []
        };
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 1; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if ((key is not ("--engine-root" or "--cache-root" or "--format" or "--snapshot" or "--source-tree") && Array.IndexOf(specific, key) < 0)
                || index + 1 >= arguments.Length || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown, duplicate or incomplete documentation option: " + key);
        }
        string format = options.GetValueOrDefault("--format", "text");
        if (format is not ("text" or "json"))
            throw new ArgumentException("Documentation format must be text or json.");
        string root = Path.GetFullPath(options.GetValueOrDefault("--engine-root", Directory.GetCurrentDirectory()));
        string cache = Path.GetFullPath(options.GetValueOrDefault("--cache-root", Path.Combine(root, "artifacts", "documentation-query")));
        DocumentationResult result;
        if (operation == "index")
        {
            if (options.ContainsKey("--snapshot") || options.ContainsKey("--source-tree"))
                throw new ArgumentException("Index establishes its binding from engine.lock.json; snapshot selectors apply only to reads.");
            result = await DocumentationStore.IndexAsync(root, Required("--docs-root"), cache, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, options.GetValueOrDefault("--snapshot"),
                options.GetValueOrDefault("--source-tree"), cancellationToken).ConfigureAwait(false);
            if (operation == "catalog")
            {
                DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync(cancellationToken).ConfigureAwait(false);
                if (format == "json")
                    Console.WriteLine(JsonSerializer.Serialize(catalog, DocumentationStore.json));
                else
                {
                    Console.WriteLine($"Snapshot: {catalog.snapshot}; content: {catalog.contentIdentity}; source API: {catalog.sourceApiStatus}");
                    foreach (DocumentationPageInfo page in catalog.pages)
                        Console.WriteLine($"{page.id}: {page.path} ({page.kind}; {page.sections.Count} sections)");
                    foreach (var project in catalog.projects)
                        if (project.Value is null)
                            Console.WriteLine("Missing project entry: " + project.Key);
                }
                return 0;
            }
            string? value = operation switch
            {
                "search" => Required("--query"),
                "symbol" => Required("--id"),
                "resource" => Required("--uri"),
                _ => options.GetValueOrDefault("--page")
            };
            result = await snapshot.QueryAsync(new(operation, value, options.GetValueOrDefault("--project"),
                options.GetValueOrDefault("--surface"), options.GetValueOrDefault("--section"),
                int.Parse(options.GetValueOrDefault("--offset", "0"), CultureInfo.InvariantCulture),
                int.Parse(options.GetValueOrDefault("--limit", "5"), CultureInfo.InvariantCulture)), cancellationToken).ConfigureAwait(false);
        }
        if (format == "json")
            Console.WriteLine(JsonSerializer.Serialize(result, DocumentationStore.json));
        else
        {
            Console.WriteLine($"{result.status}; snapshot: {result.snapshot}; source API: {result.sourceApiStatus}");
            Console.WriteLine(result.message);
            foreach (DocumentationHit hit in result.hits)
            {
                Console.WriteLine($"\n{hit.resource} ({hit.page}:{hit.line}; {hit.completeness}; {hit.reason})");
                Console.WriteLine(hit.text);
            }
            foreach (DocumentationContinuation next in result.continuations)
                Console.WriteLine($"Continue: docs read --page {next.pageId}" + (next.section is null ? "" : " --section " + next.section)
                    + $" --offset {next.offset} — {next.reason}");
            if (result.nextRequest is not null)
                Console.WriteLine("Next request: " + JsonSerializer.Serialize(result.nextRequest, DocumentationStore.json));
        }
        return result.status is "Missing" or "Ambiguous" or "Mismatched" ? 2 : 0;

        string Required(string option) => options.TryGetValue(option, out string? value) && !string.IsNullOrWhiteSpace(value)
            ? value : throw new ArgumentException("Missing documentation option: " + option);
    }
}
