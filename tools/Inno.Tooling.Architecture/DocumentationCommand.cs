using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Architecture.Documentation;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Queries repository documentation without evaluating SDK projects, compiling products or invoking network services.
/// </summary>
public static class DocumentationCommand
{
    /// <summary>
    /// Maps existing canonical pages and their included or same-name directory fragments to actual repository projects.
    /// Missing project pages remain the responsibility of the required documentation structure check.
    /// </summary>
    /// <param name="root">
    /// The absolute repository root containing docs and the existing source project inventory.
    /// </param>
    /// <returns>
    /// Repository-relative Markdown paths mapped to repository-relative project paths, without SDK evaluation.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Project pages are ambiguous, or an inclusion is absent, repeats or cycles.
    /// </exception>
    /// <exception cref="IOException">
    /// A documentation path escapes its owner, follows a link or cannot be read.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The root is invalid or repository projects have duplicate names.
    /// </exception>
    public static IReadOnlyDictionary<string, string> GetProjectDocuments(string root)
    {
        root = Path.GetFullPath(root);
        DocumentationCatalog catalog = DocumentationCatalog.Read(root, requireCompleteCoverage: false);
        return catalog.owners.ToDictionary(static pair => pair.Key,
            pair => Path.GetRelativePath(root, catalog.projects[pair.Value].project).Replace('\\', '/'), StringComparer.Ordinal);
    }

    /// <summary>
    /// Indexes Markdown or retrieves search excerpts, complete sections and exact documented symbols.
    /// </summary>
    /// <param name="arguments">
    /// The index, search, read or symbol operation followed by engine-root, query, project, page, section, id,
    /// limit, offset and format options. Format is text or json; pages are relative to the repository root.
    /// Offset resumes an explicitly partial section without discarding intervening Markdown.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels index preparation before publication while retaining the last complete index.
    /// </param>
    /// <returns>
    /// Zero after emitting a valid result, including explicit empty search results or ambiguity candidates.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An operation, option, project, section or symbol is absent, ambiguous as an input, or invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// A page is missing, a path escapes its owner, an inclusion is invalid or documentation changes during indexing.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels indexing or ownership acquisition.
    /// </exception>
    public static async Task<int> ExecuteAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Length == 0 || arguments[0] is not ("index" or "search" or "read" or "symbol"))
            throw new ArgumentException("Usage: docs <index|search|read|symbol> --engine-root ROOT [--format text|json]");
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        string[] specific = arguments[0] switch
        {
            "search" => ["--query", "--project", "--limit"],
            "read" => ["--page", "--project", "--section", "--offset"],
            "symbol" => ["--project", "--id"],
            _ => []
        };
        for (int index = 1; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (key is not ("--engine-root" or "--format") && Array.IndexOf(specific, key) < 0
                || index + 1 >= arguments.Length || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown, duplicate or incomplete documentation option: " + key);
        }
        string root = Path.GetFullPath(options.GetValueOrDefault("--engine-root", Directory.GetCurrentDirectory()));
        string format = options.GetValueOrDefault("--format", "text");
        if (format is not ("text" or "json"))
            throw new ArgumentException("Documentation format must be text or json.");
        cancellationToken.ThrowIfCancellationRequested();
        DocumentationIndex snapshot = await DocumentationIndex.LoadAsync(root, cancellationToken).ConfigureAwait(false);
        var query = new DocumentationQuery(root, snapshot);
        DocumentationResult result = arguments[0] switch
        {
            "index" => new("Indexed", snapshot.identity, "Unverified",
                $"Indexed {snapshot.pages.Count} pages; reparsed {snapshot.rebuiltPages}. No source API validation was performed.", [], []),
            "search" => query.Search(Required("--query"), options.GetValueOrDefault("--project"),
                int.Parse(options.GetValueOrDefault("--limit", "5"), System.Globalization.CultureInfo.InvariantCulture)),
            "read" => query.Read(options.GetValueOrDefault("--page"), options.GetValueOrDefault("--project"), options.GetValueOrDefault("--section"),
                int.Parse(options.GetValueOrDefault("--offset", "0"), System.Globalization.CultureInfo.InvariantCulture)),
            "symbol" => query.Symbol(Required("--project"), Required("--id")),
            _ => throw new ArgumentException("Unknown documentation operation.")
        };
        DocumentationOutput.Write(result, format == "json");
        return 0;

        string Required(string option) => options.TryGetValue(option, out string? value) && !string.IsNullOrWhiteSpace(value)
            ? value : throw new ArgumentException("Missing documentation option: " + option);
    }
}
