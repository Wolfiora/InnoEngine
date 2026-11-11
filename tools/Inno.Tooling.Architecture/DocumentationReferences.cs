using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Exports documentation project facts from the same source inventory used by architecture coverage.
/// </summary>
public static class DocumentationReferences
{
    /// <summary>
    /// Exports selected CLR source contracts using the existing evaluated API extractor, independently of Wiki pages.
    /// This operation does not certify script projection, behavioral prose, examples or product execution.
    /// </summary>
    /// <param name="arguments">
    /// Required engine-root, dotnet, output and one or more project options; optional configuration and target.
    /// Project paths are relative to the engine root and must occur in its actual source inventory.
    /// The selected SDK and prepared references supply compilation facts; no product build is requested.
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
            else if (key is not ("--engine-root" or "--dotnet" or "--output" or "--configuration" or "--target")
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
        var references = new WikiProjectReferences(root, configuration, Path.GetFullPath(dotnet),
            options.GetValueOrDefault("--target"), selectedProjects: selected);
        var models = new DocumentationSourceModels(root, configuration, references, cancellationToken, failures);
        var inventory = new List<object>();
        foreach (string project in selected.Order(StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string directory = Path.GetDirectoryName(project)!;
            if (models.IsExcludedProduct(directory))
                throw new InvalidDataException("Selected API project is outside the requested product target: " + project);
            var compilation = models.GetCompilation(directory)
                ?? throw new InvalidDataException("Selected API compilation is unavailable: " + project
                    + (failures.Count == 0 ? string.Empty : "\n" + string.Join('\n', failures)));
            var symbols = WikiApiCoverageValidator.Capture(root, project, null, compilation, failures, cancellationToken);
            inventory.Add(new { project = Path.GetRelativePath(root, project).Replace('\\', '/'),
                name = Path.GetFileNameWithoutExtension(project), symbols });
        }
        if (failures.Count != 0)
            throw new InvalidDataException("API extraction did not establish complete source facts:\n" + string.Join('\n', failures));
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
                surfaces = Array.Empty<string>(), isTest = WikiProjectCoverageValidator.IsDeclaredTestProject(project) });
        }
        if (projects.Count == 0)
            throw new InvalidDataException("Source-project inventory is empty.");
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(output), JsonSerializer.SerializeToUtf8Bytes(projects,
            new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Exported {projects.Count} source projects. API surfaces and source validation remain unverified.");
        return 0;
    }
}
