using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Exports documentation project facts from the same source inventory used by architecture coverage.
/// </summary>
public static class DocumentationReferences
{
    /// <summary>
    /// Exports selected CLR source contracts using the existing evaluated API extractor, independently of Wiki pages.
    /// An explicit script surface reads prepared logical reference assemblies and compiler-produced origin metadata.
    /// This operation does not certify behavioral prose, examples or product execution.
    /// </summary>
    /// <param name="arguments">
    /// Required engine-root, dotnet, output and one or more project options; optional configuration and target.
    /// Optional artifacts-path and sdk-snapshot select already prepared outputs and one resolved graph with
    /// matching properties and owners. The caller owns the execution boundary and closing input checks;
    /// a supplied graph does not independently establish freshness or certify the exported contracts.
    /// Project paths are relative to the engine root and must occur in its actual source inventory.
    /// The selected SDK and prepared references supply compilation facts; no product build is requested.
    /// Script extraction selects surface, runtime-reference and optional editor-reference instead of project,
    /// with repeated source-reference manifests resolving exact engine owners. Its SDK evaluation is preparation,
    /// with one explicit framework-project supplying the framework closure instead of evaluating all API owners;
    /// never part of a prepared-snapshot query. Missing origin metadata requires current compiler artifacts.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels evaluated capture and extraction before atomic publication.
    /// </param>
    /// <returns>
    /// Zero after publishing complete facts for every selected project, including explicit empty public surfaces.
    /// Source API certification remains separate from this extraction result.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Required options are absent, an option is duplicated or unsupported, or a project is not an actual owner.
    /// </exception>
    /// <exception cref="IOException">
    /// Inputs are unavailable, escape ownership, or cannot be published.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A selected compilation or public signature is unavailable or unresolved.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels capture, extraction or publication.
    /// </exception>
    public static int ExportApi(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        if (arguments.Contains("--surface", StringComparer.Ordinal))
            return DocumentationScriptApi.Export(arguments, cancellationToken);
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        var requested = new HashSet<string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (index + 1 >= arguments.Length)
                throw new ArgumentException("Incomplete API export option: " + key);
            if (key == "--project")
            {
                if (!requested.Add(arguments[index + 1].Replace('\\', '/')))
                    throw new ArgumentException("Duplicate API project selection.");
            }
            else if (key is not ("--engine-root" or "--dotnet" or "--output" or "--configuration" or "--target" or "--artifacts-path" or "--sdk-snapshot")
                || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown or duplicate API export option: " + key);
        }
        if (!options.TryGetValue("--engine-root", out string? engineRoot) || !options.TryGetValue("--dotnet", out string? dotnet)
            || !options.TryGetValue("--output", out string? output) || requested.Count == 0)
            throw new ArgumentException("Usage: docs export-api --engine-root ROOT --dotnet SDK --project OWNER.csproj [--project OWNER.csproj] --output FACTS.json [--configuration Debug|SourceDebug|Release] [--target TARGET].");
        string configuration = options.GetValueOrDefault("--configuration", "Debug");
        if (configuration is not ("Debug" or "SourceDebug" or "Release"))
            throw new ArgumentException("Unsupported API extraction configuration.");
        string root = Path.GetFullPath(engineRoot);
        IReadOnlyList<ApiProject> inventory = CaptureApi(root, dotnet, configuration,
            options.GetValueOrDefault("--target"), requested, cancellationToken,
            options.GetValueOrDefault("--artifacts-path"),
            options.TryGetValue("--sdk-snapshot", out string? graphPath) ? File.ReadAllText(graphPath) : null);
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(output), JsonSerializer.SerializeToUtf8Bytes(new
        {
            model = "source-clr", configuration, target = options.GetValueOrDefault("--target"),
            sourceApiStatus = "Unverified", projects = inventory
        }, new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Exported {inventory.Count} evaluated CLR project surfaces. Script projections and source API certification remain separate.");
        return 0;
    }

    /// <summary>
    /// Compares explicitly selected reference manifests with one freshly evaluated source compilation batch.
    /// Declaration, XML, constant and inheritance changes are checked without building products or parsing Markdown.
    /// </summary>
    /// <param name="arguments">
    /// Required engine-root, dotnet, output and repeated reference options. All references must select the same
    /// source-clr configuration and target; their actual projects determine the batch, including empty public surfaces.
    /// An explicit cache-root and snapshot pair reads logical reference paths from that protected generation.
    /// Otherwise reference paths select local files. An optional artifacts-path selects the prepared compile output owner.
    /// An optional sdk-snapshot supplies one previously resolved SDK graph with matching properties and selected owners.
    /// The caller owns its stable execution boundary and closing input checks; a supplied graph is not independent freshness evidence.
    /// Repeated project options restrict comparison to declared owners;
    /// an absent owner is an error. Snapshot provenance is reported separately from source compatibility.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels capture or comparison before replacing the report.
    /// </param>
    /// <returns>
    /// Zero when the selected reference facts match, or one with explicit differences. This bounded result does not
    /// certify authored prose, script projections, examples, other targets or publication provenance.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Options, reference selection or evaluated context are missing, duplicated or unsupported.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Required discovery is empty, identities repeat, reference inputs change, or source compilation is unavailable.
    /// </exception>
    /// <exception cref="IOException">
    /// A selected input is absent, escapes its declared source owner, or report publication fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels before a complete comparison report is published.
    /// </exception>
    public static async Task<int> ValidateApiAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        var selectedReferences = new HashSet<string>(StringComparer.Ordinal);
        var selectedProjects = new HashSet<string>(StringComparer.Ordinal);
        var references = new Dictionary<string, byte[]>(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            if (index + 1 == arguments.Length)
                throw new ArgumentException("Incomplete reference validation option: " + arguments[index]);
            string key = arguments[index];
            if (key == "--reference")
            {
                if (!selectedReferences.Add(arguments[index + 1]))
                    throw new ArgumentException("Duplicate reference manifest: " + arguments[index + 1]);
            }
            else if (key == "--project")
            {
                if (!selectedProjects.Add(arguments[index + 1]))
                    throw new ArgumentException("Duplicate source owner selection: " + arguments[index + 1]);
            }
            else if (key is not ("--engine-root" or "--dotnet" or "--output" or "--cache-root" or "--snapshot" or "--artifacts-path" or "--sdk-snapshot")
                || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown or duplicate reference validation option: " + key);
        }
        if (selectedReferences.Count == 0 || !options.TryGetValue("--engine-root", out string? root)
            || !options.TryGetValue("--dotnet", out string? dotnet) || !options.TryGetValue("--output", out string? output))
            throw new ArgumentException("Usage: docs validate-api --engine-root ROOT --dotnet SDK --reference FACTS.json [--reference FACTS.json] --output REPORT.json [--cache-root CACHE --snapshot ID] [--project OWNER.csproj]");
        bool cached = options.TryGetValue("--cache-root", out string? cache);
        if (cached != options.ContainsKey("--snapshot"))
            throw new ArgumentException("A prepared reference comparison requires both cache-root and snapshot.");
        output = Path.GetFullPath(output);
        if (cached)
        {
            string relativeOutput = Path.GetRelativePath(Path.GetFullPath(cache!), output);
            if (!Path.IsPathRooted(relativeOutput) && relativeOutput != ".."
                && !relativeOutput.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                throw new ArgumentException("Comparison reports cannot modify the documentation cache owner.");
        }
        using DocumentationSnapshot? snapshot = cached
            ? await DocumentationStore.OpenAsync(cache!, options["--snapshot"], cancellationToken: cancellationToken).ConfigureAwait(false)
            : null;
        foreach (string selected in selectedReferences)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string path = cached ? selected : Path.GetFullPath(selected);
            byte[] bytes = snapshot is null ? await File.ReadAllBytesAsync(path, cancellationToken).ConfigureAwait(false)
                : await snapshot.ReadReferenceAsync(path, cancellationToken).ConfigureAwait(false);
            if (!references.TryAdd(path, bytes))
                throw new ArgumentException("Repeated reference manifest identity: " + path);
        }
        if (references.ContainsKey(output))
            throw new ArgumentException("A comparison report cannot replace an input manifest.");
        string? configuration = null;
        string? target = null;
        var expected = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        foreach (var reference in references)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using JsonDocument manifest = JsonDocument.Parse(reference.Value);
            JsonElement facts = manifest.RootElement;
            string? selectedConfiguration = facts.GetProperty("configuration").GetString();
            string? selectedTarget = facts.TryGetProperty("target", out JsonElement declaredTarget) ? declaredTarget.GetString() : null;
            if (facts.GetProperty("model").GetString() != "source-clr"
                || selectedConfiguration is not ("Debug" or "SourceDebug" or "Release"))
                throw new InvalidDataException("Unsupported source reference context: " + reference.Key);
            if (configuration is not null && (configuration != selectedConfiguration || target != selectedTarget))
                throw new InvalidDataException("Reference manifests require separate evaluated batches for different configurations or targets.");
            configuration = selectedConfiguration;
            target = selectedTarget;
            if (facts.GetProperty("projects").GetArrayLength() == 0)
                throw new InvalidDataException("Required source reference discovery is empty: " + reference.Key);
            foreach (JsonElement project in facts.GetProperty("projects").EnumerateArray())
            {
                string source = project.GetProperty("project").GetString()!;
                string absolute = PathBoundary.Resolve(Path.GetFullPath(root), source);
                if (source != Path.GetRelativePath(Path.GetFullPath(root), absolute).Replace('\\', '/'))
                    throw new InvalidDataException("Source reference paths must be canonical owner-relative paths: " + source);
                var identities = new HashSet<string>(StringComparer.Ordinal);
                foreach (JsonElement symbol in project.GetProperty("symbols").EnumerateArray())
                    if (!identities.Add(symbol.GetProperty("id").GetString()!))
                        throw new InvalidDataException("Repeated reference symbol: " + symbol.GetProperty("id").GetString());
                if (!expected.TryAdd(source, project.Clone()))
                    throw new InvalidDataException("Repeated source project in reference selection: " + project.GetProperty("project").GetString());
            }
        }
        if (expected.Count == 0)
            throw new InvalidDataException("Required source reference discovery is empty.");
        if (selectedProjects.Count != 0)
        {
            foreach (string project in selectedProjects)
                if (!expected.ContainsKey(project))
                    throw new InvalidDataException("Selected source owner is absent from the reference batch: " + project);
            foreach (string project in expected.Keys.Where(project => !selectedProjects.Contains(project)).ToArray())
                expected.Remove(project);
        }
        IReadOnlyList<ApiProject> actual = CaptureApi(Path.GetFullPath(root), dotnet, configuration!, target,
            expected.Keys, cancellationToken, options.GetValueOrDefault("--artifacts-path"),
            options.TryGetValue("--sdk-snapshot", out string? graphPath) ? await File.ReadAllTextAsync(graphPath, cancellationToken).ConfigureAwait(false) : null);
        var results = new List<object>();
        bool passed = true;
        foreach (ApiProject project in actual)
        {
            cancellationToken.ThrowIfCancellationRequested();
            JsonElement previous = expected[project.project];
            var differences = new List<string>();
            if (previous.GetProperty("name").GetString() != project.name)
                differences.Add("Project identity differs from the actual source owner.");
            var symbols = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
            foreach (JsonElement symbol in previous.GetProperty("symbols").EnumerateArray())
                symbols.Add(symbol.GetProperty("id").GetString()!, symbol);
            foreach (var symbol in project.symbols)
            {
                if (!symbols.Remove(symbol.id, out JsonElement prior))
                    differences.Add("Missing current declaration: " + symbol.id);
                else if (ComparableSymbol(prior) != ComparableSymbol(JsonSerializer.SerializeToElement(symbol)))
                    differences.Add("Declaration, XML, constant, source or type relationships changed: " + symbol.id);
            }
            differences.AddRange(symbols.Keys.Order(StringComparer.Ordinal).Select(static id => "Reference declaration no longer exists: " + id));
            passed &= differences.Count == 0;
            results.Add(new { project.project, project.name, status = differences.Count == 0 ? "Passed" : "Failed",
                referenceIdentity = Convert.ToHexStringLower(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(previous))),
                symbols = project.symbols.Count, differences });
        }
        if (snapshot is null)
            foreach (var reference in references)
                if (!File.ReadAllBytes(reference.Key).AsSpan().SequenceEqual(reference.Value))
                    throw new InvalidDataException("Reference input changed during source validation: " + reference.Key);
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(output, JsonSerializer.SerializeToUtf8Bytes(new
        {
            status = passed ? "Passed" : "Failed", scope = "SelectedSourceReferenceComparison", configuration, target,
            sourceApiStatus = "Unverified",
            documentationSnapshot = snapshot?.identity, documentationSourceTree = snapshot?.sourceTree,
            references = references.OrderBy(static pair => pair.Key, StringComparer.Ordinal).Select(static pair => new
            {
                path = pair.Key, identity = Convert.ToHexStringLower(SHA256.HashData(pair.Value))
            }),
            projects = results
        }, new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Compared {actual.Count} evaluated source owners in one batch: {(passed ? "Passed" : "Failed")}. Publication provenance and other validation remain separate.");
        return passed ? 0 : 1;
    }

    /// <summary>
    /// Writes a deterministic project inventory without claiming evaluated API surfaces or source verification.
    /// </summary>
    /// <param name="arguments">
    /// Explicit engine-root and output options. Output is a derived reference artifact, not a project selection table.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels source enumeration before atomic output publication.
    /// </param>
    /// <returns>
    /// Zero after publishing the current source-project inventory.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An option is missing, duplicated or unsupported.
    /// </exception>
    /// <exception cref="IOException">
    /// A source path escapes the engine boundary or the destination cannot be published.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Required source discovery is empty or project identities are ambiguous.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels before publication.
    /// </exception>
    public static int ExportProjects(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (key is not ("--engine-root" or "--output") || index + 1 >= arguments.Length
                || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown, duplicate or incomplete reference export option: " + key);
        }
        if (!options.TryGetValue("--engine-root", out string? engineRoot) || !options.TryGetValue("--output", out string? output))
            throw new ArgumentException("Usage: docs export-projects --engine-root ROOT --output PROJECTS.json");
        string root = Path.GetFullPath(engineRoot);
        var names = new HashSet<string>(StringComparer.Ordinal);
        var projects = new List<object>();
        foreach (string project in RepositorySourceInventory.Projects(root)
            .OrderBy(project => Path.GetRelativePath(root, project).Replace('\\', '/'), StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(root, project);
            string name = Path.GetFileNameWithoutExtension(project);
            if (!names.Add(name))
                throw new InvalidDataException("Duplicate source-project identity: " + name);
            // API surfaces require evaluated extraction; inventory must never invent them from a project name.
            projects.Add(new { name, path = Path.GetRelativePath(root, project).Replace('\\', '/'), entry = (string?)null,
                surfaces = Array.Empty<string>(), isTest = RepositorySourceInventory.IsDeclaredTestProject(project) });
        }
        if (projects.Count == 0)
            throw new InvalidDataException("Source-project inventory is empty.");
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(output), JsonSerializer.SerializeToUtf8Bytes(projects,
            new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Exported {projects.Count} source projects. API surfaces and source validation remain unverified.");
        return 0;
    }

    private static IReadOnlyList<ApiProject> CaptureApi(
        string root,
        string dotnet,
        string configuration,
        string? target,
        IEnumerable<string> requested,
        CancellationToken cancellationToken,
        string? artifactsPath = null,
        string? sdkSnapshot = null
    ) {
        string[] projects = RepositorySourceInventory.Projects(root).ToArray();
        var selected = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string project in requested)
        {
            string absolute = PathBoundary.Resolve(root, project);
            PathBoundary.RequireUnlinkedPath(root, absolute);
            if (!projects.Contains(absolute, StringComparer.Ordinal))
                throw new ArgumentException("Selected API owner is absent from the actual source inventory: " + project);
            selected.Add(absolute);
        }
        var failures = new List<string>();
        var references = new DocumentationProjectReferences(root, configuration, Path.GetFullPath(dotnet), target,
            artifactsPath is null ? null : Path.GetFullPath(artifactsPath), selectedProjects: selected, sdkSnapshot: sdkSnapshot);
        var models = new DocumentationSourceModels(root, configuration, references, cancellationToken, failures);
        var inventory = new List<ApiProject>();
        foreach (string project in selected.Order(StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string directory = Path.GetDirectoryName(project)!;
            if (models.IsExcludedProduct(directory))
                throw new InvalidDataException("Selected API project is outside the requested product target: " + project);
            var compilation = models.GetCompilation(directory)
                ?? throw new InvalidDataException("Selected API compilation is unavailable: " + project
                    + (failures.Count == 0 ? string.Empty : "\n" + string.Join('\n', failures)));
            var symbols = DocumentationApiExtractor.Capture(root, project, null, compilation, failures, cancellationToken);
            inventory.Add(new(Path.GetRelativePath(root, project).Replace('\\', '/'), Path.GetFileNameWithoutExtension(project), symbols,
                DocumentationApiExtractor.ReadExports(compilation), compilation.AssemblyName!));
        }
        if (failures.Count != 0)
            throw new InvalidDataException("API extraction did not establish complete source facts:\n" + string.Join('\n', failures));
        return inventory;
    }

    internal static string ComparableSymbol(JsonElement symbol)
    {
        var ordered = new SortedDictionary<string, JsonNode?>(StringComparer.Ordinal);
        foreach (JsonProperty property in symbol.EnumerateObject())
        {
            // Source offsets and the retired Wiki page field do not change an API contract.
            if (property.Name is "line" or "page")
                continue;
            ordered.Add(property.Name, property.Name == "documentation"
                ? JsonValue.Create(property.Value.GetString()!.ReplaceLineEndings("\n"))
                : JsonNode.Parse(property.Value.GetRawText()));
        }
        return JsonSerializer.Serialize(ordered);
    }

    private sealed record ApiProject(
        string project,
        string name,
        IReadOnlyList<DocumentationApiExtractor.DocumentationApiEntry> symbols,
        IReadOnlyList<DocumentationApiExtractor.ScriptExport> scriptExports,
        string assembly
    );
}
